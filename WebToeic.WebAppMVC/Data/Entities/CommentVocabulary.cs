using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class CommentVocabulary
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string UserNameCmtV { get; set; }
        public string VocaNameCmtV { get; set; }
        public int? ParentCommentId { get; set; }
        public CommentVocabulary? ParentComment { get; set; }

        //public Guid UserId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int VocabularyId { get; set; }
        public Vocabulary Vocabulary { get; set; }

        public ICollection<CommentVocabulary>? Replies { get; set; }
    }
}
