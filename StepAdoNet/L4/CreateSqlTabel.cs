using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public static class CreateSqlTabel
    {
        public static string CreateTable(
            Type tableType,string tabelName) {

            var entityType = tableType;
            var entityName = entityType.Name;
            var entityproperties = entityType.GetProperties();

            var QueryStringBilder = new StringBuilder();
            QueryStringBilder.Append($"Create TABLE {tabelName}( ");

            foreach (var property in entityproperties)
            {
                var propertyName = property.Name;
                var propertyType = property.PropertyType;

                if (propertyType == typeof(string)) {
                    QueryStringBilder.Append($"{propertyName} varchar(255) not null,");
                }
                if (propertyType == typeof(int) ||
                    propertyType == typeof(long) ||
                    propertyType == typeof(double) ||
                    propertyType == typeof(decimal)) {
                    QueryStringBilder.Append($"{propertyName} int not null,");
                }
                if (propertyType == typeof(DateTime)) {
                    QueryStringBilder.Append($"{propertyName} datetime not null,");
                }
                QueryStringBilder.Append(')');
            }
            return QueryStringBilder.ToString();
        }
    }
}
