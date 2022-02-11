using L4.Tables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace L4
{
    public class UnitOfWork : IDisposable
    {
        private readonly SqlConnection _sqlConnection;

        public GenericLazyRepository<MessageEntity> Messages { get; set; }
        public GenericLazyRepository<CommentEntity> Comments { get; set; }
        public GenericLazyRepository<AuthorEntity> Authors { get; set; }


        private void CreateInstanceOfRepository(PropertyInfo repositoryProperty)
        {
            var instance = Activator.CreateInstance(repositoryProperty.PropertyType);
            repositoryProperty.SetValue(this, instance);
        }

        private bool CheckIfMappedTableExist(ITableNameContainer tableNameContainer, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var mappedTableName = tableNameContainer.GetMappedTableName;

            var query = $"SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES " +
                $"WHERE TABLE_NAME = '{mappedTableName}'";
            SqlCommand command = new SqlCommand(query, connection);
            var result = int.Parse(command.ExecuteScalar().ToString());
            return result > 0;
        }

        public void CreateTable(
            Type tableType, string tableName, string connectionString) 
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var query = CreateSqlTabel.CreateTable(tableType, tableName);
            var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void InitializeRepositories(string connectionString)
        {
            var unitOfWorkContainerType = this.GetType();
            var typeProperties = unitOfWorkContainerType.GetProperties();

            var genericRepositoryProperties = new List<PropertyInfo>();
            foreach (var property in typeProperties)
            {
                if (property.PropertyType.GetGenericTypeDefinition() == typeof(GenericLazyRepository<>))
                    genericRepositoryProperties.Add(property);
            }

            foreach (var genericRepositoryProperty in genericRepositoryProperties)
            {
                CreateInstanceOfRepository(genericRepositoryProperty);

                var tableNameContainer = (ITableNameContainer)genericRepositoryProperty.GetValue(this);
                bool tableExistCheckResult = CheckIfMappedTableExist(tableNameContainer, connectionString);
                
                if (!tableExistCheckResult) 
                {
                    var tableType = genericRepositoryProperty.PropertyType.GetGenericArguments()[0];
                    var tableName = tableNameContainer.GetMappedTableName;

                    CreateTable(tableType, tableName, connectionString);
                }
            }
        }

        public UnitOfWork(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);

            InitializeRepositories(connectionString);
        }

        private IEnumerable<string> GetInsertMessageQueries()
        {
            var entitiesToCreate = Messages.GetEntitiesToCreate();
            foreach (var entityToCreate in entitiesToCreate)
            {
                var insertSqlQuery = InsertSqlQueryGenerator
                    .GenerateInsertSqlQuery(entityToCreate, "messages");

                yield return insertSqlQuery;
            }
        }

        public void SaveChanges()
        {
            _sqlConnection.Open();

            var insertMessageQueries = GetInsertMessageQueries().ToArray();

            var sqlTransaction = _sqlConnection.BeginTransaction();

            try
            {
                foreach(var insertMessageQuery in insertMessageQueries)
                {
                    var sqlCommand = new SqlCommand(insertMessageQuery, _sqlConnection);
                    sqlCommand.Transaction = sqlTransaction;

                    sqlCommand.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
            }
            catch(Exception ex)
            {
                sqlTransaction.Rollback();
                throw;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public void Dispose()
        {
            _sqlConnection.Dispose();
        }
    }
}
