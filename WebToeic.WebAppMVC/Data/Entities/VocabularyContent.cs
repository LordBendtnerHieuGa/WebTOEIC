using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class VocabularyContent
    {
        [Key]
        public int Id { get; set; }
        public string AudioMp3 { get; set; }
        public string Content { get; set;}
        public string ImageVC { get; set; }
        public string Meaning { get; set; }
        public int Number { get; set; }
        public string Sentence { get; set; }
        public string Transcribed { get; set; }
        public int VocabularyContentId { get; set; }
        public Vocabulary Vocabulary { get; set; }

    }
}
