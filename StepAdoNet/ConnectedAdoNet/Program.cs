using System;
using System.Data.SqlClient;
using System.Text;

namespace ConnectedAdoNet
{
    public class Program
    {
        private static string ConvertTagDtoToInsertQuery(TagDto tagToInsert)
            => "INSERT INTO [Tags]([Id], [Name], [IsSystem], [CreatedOn]) " +
                $"VALUES('{tagToInsert.Id}', N'{tagToInsert.Name}', {(tagToInsert.IsSystem ? 1 : 0)}, '{tagToInsert.CreatedOn}')";
        
        public static bool AddTag(TagDto tagToInsert)
        {
            string connectionString =
                "Server=10.8.0.1;" +
                "Database=IDocsDb;" +
                "User Id=iDocsDeveloper;" +
                "Password=dev_iDocsDevStream2019!;";

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var insertTagQuery = ConvertTagDtoToInsertQuery(tagToInsert);
            var insertTagCommand = new SqlCommand(insertTagQuery, sqlConnection);

            // INSERT, UPDATE, DELETE, ALTER + RENAME, CREATE DATABASE, DROP DATABASE
            var affectedRows = insertTagCommand.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public static int ClearAllTags()
        {
            string connectionString =
                "Server=10.8.0.1;" +
                "Database=IDocsDb;" +
                "User Id=iDocsDeveloper;" +
                "Password=dev_iDocsDevStream2019!;";

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var deleteAllTagsQuery = "DELETE FROM [Tags]";
            var deleteAllTagsCommand = new SqlCommand(deleteAllTagsQuery, sqlConnection);
            var affectedRows = deleteAllTagsCommand.ExecuteNonQuery();

            return affectedRows;
        }

        public static EmployeeDto[] ReadAllEmployeesFromCompanies(params string [] companyBins)
        {
            var employees = new List<EmployeeDto>();

            string connectionString =
                "Server=10.8.0.1;" +
                "Database=IDocsDb;" +
                "User Id=iDocsDeveloper;" +
                "Password=dev_iDocsDevStream2019!;";

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            var companyBinsFormatted = new List<string>();
            foreach (var item in companyBins)
                companyBinsFormatted.Add($"'{item}'");

            // ["Kazakhstan" | "Russia" | "USA" | "China"]
            // "Kazakhstan$Russia$USA$China"

            var companyBinsStr = string.Join(" , ", companyBinsFormatted);

            var query = "SELECT Id as id, FirstName as first_name, LastName as last_name, " +
                "IIN as iin, PositionName as position_name from [Employees] where " +
                $"CompanyId in (SELECT Id FROM [Companies] WHERE BIN in ({companyBinsStr}))";

            var readAllEmployeesCommand = new SqlCommand(query, sqlConnection);

            var cursor = readAllEmployeesCommand.ExecuteReader();

            while (cursor.Read())
            {
                var id = cursor.GetGuid(0).ToString();
                var firstName = cursor.GetString(1);
                var lastName = cursor.GetString(2);
                var iin = cursor.GetString(3);
                var position = cursor.GetString(4);

                var employee = new EmployeeDto(id, firstName, lastName, iin, position);
                employees.Add(employee);
            }

            return employees.ToArray();
        }

        public static EmployeeDto[] ReadAllEmployees()
        {
            var employees = new List<EmployeeDto>();

            string connectionString =
                "Server=10.8.0.1;" +
                "Database=IDocsDb;" +
                "User Id=iDocsDeveloper;" +
                "Password=dev_iDocsDevStream2019!;";

            using var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            // SELECT + any modification - SELECT + innert SELECT, SELECT + JOIN, SELECT + GROUP BY
            var readAllEmployeesQuery = "SELECT " +
                "Id as id, FirstName as first_name, LastName as last_name, IIN as iin, " +
                "PositionName as position_name " +
                "FROM [Employees]";

            var readAllEmployeesCommand = new SqlCommand(readAllEmployeesQuery, sqlConnection);

            var cursor = readAllEmployeesCommand.ExecuteReader();

            while (cursor.Read())
            {
                var id = cursor.GetGuid(0).ToString();
                var firstName = cursor.GetString(1);
                var lastName = cursor.GetString(2);
                var iin = cursor.GetString(3);
                var position = cursor.GetString(4);

                var employee = new EmployeeDto(id, firstName, lastName, iin, position);
                employees.Add(employee);
            }

            return employees.ToArray();
        }

        public static string[] GetAllUniquePositions(EmployeeDto[] employees)
        {
            var positionsSet = new SortedSet<string>();

            foreach (var employee in employees)
                positionsSet.Add(employee.PositionName);

            return positionsSet.ToArray();
        }

        public static void Main(string [] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // var employees = ReadAllEmployees();
            // var positions = GetAllUniquePositions(employees);

            // foreach (var position in positions)
            //    Console.WriteLine(position);

            // var tagToInsert = new TagDto("Путешествие в Испанию 2021", false);
            // AddTag(tagToInsert);

            // var affectedRows = ClearAllTags();
            // Console.WriteLine(affectedRows);

            var employees = ReadAllEmployeesFromCompanies("891022301897", "930705300185", "880130400025");
            foreach (var employee in employees)
            {
                Console.WriteLine(employee);
            }

            Console.ReadLine();
        }
    }
}