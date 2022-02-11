using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisticConcurrencyV1
{
    public class StudentRepository : IDisposable
    {
        private readonly SqlConnection _sqlConnection;
        public StudentRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
            _sqlConnection.Open();
        }

        public void AddStudent(StudentEntity entityToInsert)
        {
            var concurrencyRowVersion = 1; 

            var query = "insert into [Students](" +
                "[FullName], [BonusCoinAmount], [ConcurrencyRowVersion]) " +
                $"values (N'{entityToInsert.FullName}', " +
                $"{entityToInsert.BonusCoinAmount}, " +
                $"{concurrencyRowVersion})";

            var queryCommand = new SqlCommand(query, _sqlConnection);
            var inserted = queryCommand.ExecuteNonQuery() != 0;
        }

        public StudentEntity GetStudentById(int studentId)
        {
            var entityFromDb = default(StudentEntity);

            var query = $"select * from [Students] where [Id] = '{studentId}'";
            var queryCommand = new SqlCommand(query, _sqlConnection);

            using var reader = queryCommand.ExecuteReader();
            reader.Read();

            entityFromDb = new StudentEntity(
                reader.GetString(1),
                reader.GetInt32(2));

            entityFromDb.Id = reader.GetInt32(0);
            entityFromDb.ConcurrencyRowVersion = reader.GetInt32(3);

            return entityFromDb;
        }

        public void UpdateStudent(StudentEntity entityToUpdate)
        {
            var entityFromDb = GetStudentById(entityToUpdate.Id);

            if (entityFromDb.ConcurrencyRowVersion != 
                entityToUpdate.ConcurrencyRowVersion)
            {
                throw new InvalidOperationException(
                    "Entity was updated by another connection, please, read data again!");
            }
            else
            {
                entityToUpdate.ConcurrencyRowVersion++;
            }

            var updateQuery = $"update [Students] set " +
                $"[FullName] = N'{entityToUpdate.FullName}', " +
                $"[BonusCoinAmount] = {entityToUpdate.BonusCoinAmount}, " +
                $"[ConcurrencyRowVersion] = {entityToUpdate.ConcurrencyRowVersion}";

            var queryCommand = new SqlCommand(updateQuery, _sqlConnection);
            var updated = queryCommand.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}
