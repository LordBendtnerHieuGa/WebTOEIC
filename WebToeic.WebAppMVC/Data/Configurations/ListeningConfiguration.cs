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
    public class ListeningConfiguration : IEntityTypeConfiguration<Listening>
    {
        public void Configure(EntityTypeBuilder<Listening> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Level).IsRequired();
            builder.Property(x => x.Part).IsRequired();
            builder.Property(x => x.Photo);
            builder.Property(x => x.ListeningName).IsRequired();

            builder.HasMany(x => x.ListeningQuestions)
                   .WithOne(v => v.Listening)
                   .HasForeignKey(v => v.ListeningId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
