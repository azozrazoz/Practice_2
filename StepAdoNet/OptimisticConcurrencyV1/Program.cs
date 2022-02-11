using System;
using System.Data.SqlClient;

namespace OptimisticConcurrencyV1
{
    public class PersonC
    {

    }

    public struct PersonS
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            PersonC p = default(PersonC);
            PersonS p1 = default(PersonS);



            string connectionString =
                "Server=(localdb)\\MSSQLLocalDB;" +
                "Database=TwitterAppV1;" +
                "Trusted_Connection=True";

            var sqlConnection = new SqlConnection(connectionString);
            var studentRepository = new StudentRepository(sqlConnection);

            var getStudent = studentRepository.GetStudentById(1);
            getStudent.BonusCoinAmount += 100;

            studentRepository.UpdateStudent(getStudent);
        }
    }
}
