using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.MaxPoint);
            builder.Property(x => x.Rank);
            builder.Property(x => x.DOB).IsRequired();
            builder.Property(x => x.Address);
            builder.Property(x => x.ImageU);
        }
    }
}
