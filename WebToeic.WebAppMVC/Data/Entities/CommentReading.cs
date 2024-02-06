using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class CommentReading
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string UserNameCmtR { get; set; }
        public string ReadingNameCmtR { get; set; }
        public int? ParentCommentId { get; set; }
        public CommentReading? ParentComment { get; set; }

        //public Guid UserId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int ReadingId { get; set; }
        public Reading Reading { get; set; }

        public ICollection<CommentReading>? Replies { get; set; }
    }
}
