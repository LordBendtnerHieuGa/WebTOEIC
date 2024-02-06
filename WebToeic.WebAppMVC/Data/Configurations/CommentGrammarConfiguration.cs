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
    public class CommentGrammarConfiguration : IEntityTypeConfiguration<CommentGrammar>
    {
        public void Configure(EntityTypeBuilder<CommentGrammar> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Time).IsRequired();
            builder.Property(x => x.UserNameCmtG).IsRequired();
            builder.Property(x => x.GrammarNameCmtG).IsRequired();

            builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User).WithMany(u => u.CommentGrammars)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Grammar).WithMany(g => g.CommentGrammars)
                   .HasForeignKey(r => r.GrammarId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
