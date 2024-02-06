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
    public class TestQuestionConfiguration : IEntityTypeConfiguration<TestQuestion>
    {
        public void Configure(EntityTypeBuilder<TestQuestion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AudioMp3);
            builder.Property(x => x.CorrectAnswer).IsRequired();
            builder.Property(x => x.UserAnswer);
            builder.Property(x => x.ImageTQ);
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.Option1);
            builder.Property(x => x.Option2);
            builder.Property(x => x.Option3);
            builder.Property(x => x.Option4);
            builder.Property(x => x.Paragraph);
            builder.Property(x => x.Question);
            builder.Property(x => x.TestId).IsRequired();// thuoc tinh nay can check lai neu sai Key

        }
    }
}
