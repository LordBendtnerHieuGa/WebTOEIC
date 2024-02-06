using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class CommentListening
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string UserNameCmtL { get; set; }
        public string ListenNameCmtL { get; set; }
        public int? ParentCommentId { get; set; }
        public CommentListening? ParentComment { get; set; }

        //public Guid UserId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int ListeningId { get; set; }
        public Listening Listening { get; set; }

        public ICollection<CommentListening>? Replies { get; set; }
    }
}
