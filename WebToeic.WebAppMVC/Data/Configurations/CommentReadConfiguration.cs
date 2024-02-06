using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Data.Configurations
{
    public class CommentReadConfiguration : IEntityTypeConfiguration<CommentReading>
    {
        public void Configure(EntityTypeBuilder<CommentReading> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.UserNameCmtR).IsRequired();
            builder.Property(x => x.ReadingNameCmtR).IsRequired();

            builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User).WithMany(u => u.CommentReadings)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Reading).WithMany(v => v.CommentReadings)
                   .HasForeignKey(r => r.ReadingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
