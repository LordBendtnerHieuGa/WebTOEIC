using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }
        public string? AudioMp3 { get; set; }
        public string CorrectAnswer { get; set; }
        public string? UserAnswer { get; set; }
        public string? ImageTQ { get; set; }
        public int Number { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Paragraph { get; set; }
        public string? Question { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }  
    }
}
