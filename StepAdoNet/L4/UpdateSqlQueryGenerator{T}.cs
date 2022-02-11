using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public static class UpdateSqlQueryGenerator<TEntity>
    {
        public static string GenerateInsertSqlQuery<TEntity>(
            int primaryKey,
            TEntity entityToUpdate, string tableName)
        {
            return ""; // UPDATE [table] SET VALUES = ...;
        }
    }
}
