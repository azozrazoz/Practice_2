using System;
using System.Text;

namespace L4
{
    internal static class InsertSqlQueryGenerator
    {
        public static string GenerateInsertSqlQuery<TEntity>(
            TEntity entityToCreate, string tableName)
        {
            var entityTypeInfo = typeof(TEntity);
            var properties = entityTypeInfo.GetProperties();

            var queryStringBuilder = new StringBuilder();
            queryStringBuilder.Append($"INSERT INTO [{tableName}](");

            foreach (var property in properties)
            {
                queryStringBuilder.Append(property.Name);
                queryStringBuilder.Append(',');
            }

            queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);

            queryStringBuilder.Append(")");
            queryStringBuilder.Append("VALUES(");

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(entityToCreate);
                var propertyType = property.PropertyType;

                if (propertyType == typeof(string))
                    queryStringBuilder.Append($"N'{propertyValue}'");

                if (propertyType == typeof(int) ||
                    propertyType == typeof(long) ||
                    propertyType == typeof(double) ||
                    propertyType == typeof(decimal))
                    queryStringBuilder.Append($"{propertyValue}");

                if (propertyType == typeof(int?) ||
                    propertyType == typeof(long?) ||
                    propertyType == typeof(double?) ||
                    propertyType == typeof(decimal?))
                {
                    if(propertyValue == null)
                        queryStringBuilder.Append("NULL");
                    else
                        queryStringBuilder.Append($"{propertyValue}");
                }

                if (propertyType == typeof(bool))
                    queryStringBuilder.Append((bool)propertyValue ? 1 : 0);
                
                if (propertyType == typeof(DateTime))
                    queryStringBuilder.Append($"'{propertyValue}'");


                queryStringBuilder.Append(",");
            }

            queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);

            queryStringBuilder.Append(")");

            return queryStringBuilder.ToString();
        }
    }
}
