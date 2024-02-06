using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class ReadingQuestion
    {
        [Key]
        public int Id { get; set; }
        public string? Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Explain { get; set; }
        public string? Photo { get; set; }
        public int Order { get; set; }
        public int ReadingId { get; set; }
        public Reading Reading { get; set; }
    }
}
