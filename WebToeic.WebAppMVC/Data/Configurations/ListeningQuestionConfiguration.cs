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
    public class ListeningQuestionConfiguration : IEntityTypeConfiguration<ListeningQuestion>
    {
        public void Configure(EntityTypeBuilder<ListeningQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Question);
            builder.Property(x => x.CorrectAnswer).IsRequired();
            builder.Property(x => x.Answer1);
            builder.Property(x => x.Answer2);
            builder.Property(x => x.Answer3);
            builder.Property(x => x.Answer4);
            builder.Property(x => x.Explain);
            builder.Property(x => x.Photo);
            builder.Property(x => x.Order).IsRequired();
            builder.Property(x => x.ListeningId);
            builder.Property(x => x.AudioL);
        }
    }
}
