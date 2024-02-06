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
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImageT).IsRequired();
            builder.Property(x => x.TestName).IsRequired();

            builder.HasMany(x => x.TestQuestions)
                   .WithOne(t => t.Test)
                   .HasForeignKey(t => t.TestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Results)
                   .WithOne(t => t.Test)
                   .HasForeignKey(t => t.TestId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
