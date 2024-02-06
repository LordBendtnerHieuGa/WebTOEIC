using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Data.Configurations
{
    public class CommentListenConfiguration : IEntityTypeConfiguration<CommentListening>
    {
        public void Configure(EntityTypeBuilder<CommentListening> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.UserNameCmtL).IsRequired();
            builder.Property(x => x.ListenNameCmtL).IsRequired();

            builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User).WithMany(u => u.CommentListenings)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Listening).WithMany(l => l.CommentListenings)
                   .HasForeignKey(r => r.ListeningId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
