using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Data.Configurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CorrectListen).IsRequired();
            builder.Property(x => x.CorrectRead).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.CorrectNumber).IsRequired();
            builder.Property(x => x.InCorrectNumber).IsRequired();
            builder.Property(x => x.TestId).IsRequired(); // 2 thuoc tinh nay can check neu sai khoa
            builder.Property(x => x.UserId).IsRequired(); //


            builder.HasOne(x => x.User)
                   .WithMany(r => r.Results)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
