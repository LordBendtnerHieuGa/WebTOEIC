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
    public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
    {
        public void Configure(EntityTypeBuilder<Reading> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Level).IsRequired();
            builder.Property(x => x.Part).IsRequired();
            builder.Property(x => x.Photo);
            builder.Property(x => x.ReadingsName).IsRequired();

            builder.HasMany(x => x.ReadingQuestions)
                   .WithOne(v => v.Reading)
                   .HasForeignKey(v => v.ReadingId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
