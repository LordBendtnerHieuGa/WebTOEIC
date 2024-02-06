using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Reading
    {
        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public int Part { get; set; }
        //public string? Script { get; set; }
        public string? Photo { get; set; }
        public string ReadingsName { get; set; }
        public ICollection<CommentReading>? CommentReadings { get; set;}
        public ICollection<ReadingQuestion>? ReadingQuestions { get; set; }

    }
}
