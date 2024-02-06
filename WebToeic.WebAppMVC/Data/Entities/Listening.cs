using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{ 
    public class Listening
    {
        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public int Part { get; set; }
        //public string? Script { get; set; }
        public string? Photo { get; set; }
        public string ListeningName { get; set; }
        public ICollection<CommentListening>? CommentListenings { get; set; }
        public ICollection<ListeningQuestion>? ListeningQuestions { get; set; }

    }
}
