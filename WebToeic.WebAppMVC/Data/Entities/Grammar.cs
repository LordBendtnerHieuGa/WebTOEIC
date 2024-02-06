using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Grammar
    {
        [Key]
        public int Id { get; set; } 
        public string ImageG { get; set; }
        public string HtmlContent { get; set; } 
        public string MarkDownContent { get; set; }
        public string GrammarName { get; set; }
        public ICollection<CommentGrammar>? CommentGrammars { get; set; }
    }
}
