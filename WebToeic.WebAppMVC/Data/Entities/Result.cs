using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public int CorrectListen { get; set; }
        public int CorrectRead { get; set; }
        public DateTime Time { get; set; }
        public int CorrectNumber { get; set; }
        public int InCorrectNumber { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        //public Guid UserId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
       
    }
}
