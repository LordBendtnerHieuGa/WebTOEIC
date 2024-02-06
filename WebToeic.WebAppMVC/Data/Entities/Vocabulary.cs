using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Vocabulary
    {
        [Key]
        public int Id { get; set; } 
        public string ImageV { get; set; }
        public string VocabularyName { get; set; }
        public ICollection<CommentVocabulary>? CommentVocabularies { get; set; }
        public ICollection<VocabularyContent>? VocabularyContents { get; set; }

    }
}
