using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string ImageT { get; set; }
        public string TestName { get; set; }
        public ICollection<TestQuestion>? TestQuestions { get; set; }
        public ICollection<Result>? Results { get; set; }

    }
}
