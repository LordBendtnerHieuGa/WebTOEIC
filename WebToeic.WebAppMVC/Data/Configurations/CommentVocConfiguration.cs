using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using WebToeic.WebAppMVC.Data.Entities;

namespace WebToeic.WebAppMVC.Data.Configurations
{
    public class CommentVocConfiguration : IEntityTypeConfiguration<CommentVocabulary>
    {
        public void Configure(EntityTypeBuilder<CommentVocabulary> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.UserNameCmtV).IsRequired();
            builder.Property(x => x.VocaNameCmtV).IsRequired();

            builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User).WithMany(u => u.CommentVocabularies)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Vocabulary).WithMany(v => v.CommentVocabularies)
                   .HasForeignKey(r => r.VocabularyId)
                   .OnDelete(DeleteBehavior.Cascade);
        
        }
    }
}
