using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebToeic.WebAppMVC.Data.Entities
{
    public class Role : IdentityRole
    {
        public Role() { }
        public Role(string member) { }

        //IdentityRole<Guid>
        public string Description { get; set; }
    }
}
