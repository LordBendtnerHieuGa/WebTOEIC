using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class CommentGrammar
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string UserNameCmtG { get; set; }
        public string GrammarNameCmtG { get;set; }

        public int? ParentCommentId { get; set; }
        public CommentGrammar? ParentComment { get; set; }
      
        public string UserId { get; set; }
        public User User { get; set; }

        public int GrammarId { get; set; }
        public Grammar Grammar { get; set; }

        public ICollection<CommentGrammar>? Replies { get; set; }
    }
}
