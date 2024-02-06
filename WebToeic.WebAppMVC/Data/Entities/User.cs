using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class User : IdentityUser
    {   
        public int? MaxPoint { get; set; }
        public int? Rank { get; set; }
        public DateTime DOB { get; set; }
        public string? Address { get; set; }  
        public string? ImageU { get; set; }
        public ICollection<CommentVocabulary>? CommentVocabularies { get; set; }
        public ICollection<CommentReading>? CommentReadings { get; set; }
        public ICollection<CommentListening>? CommentListenings { get; set; }
        public ICollection<CommentGrammar>? CommentGrammars { get; set; }
        public ICollection<Result>? Results { get; set; }

    }
}
