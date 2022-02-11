using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimisticConcurrencyV1
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int BonusCoinAmount { get; set; }
        public int ConcurrencyRowVersion { get; set; }

        public StudentEntity(
            string fullName, 
            int bonusCoinAmount)
        {
            FullName = fullName;
            BonusCoinAmount = bonusCoinAmount;
        }
    }
}
