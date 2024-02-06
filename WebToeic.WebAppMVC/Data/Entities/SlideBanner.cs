using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class SlideBanner
    {
        [Key]
        public int Id { get; set; }
        public string SlideName { get; set; }
        public string SlideContent { get; set; }
        public string ImageS { get; set; }

    }
}
