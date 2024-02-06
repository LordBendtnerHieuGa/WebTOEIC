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
    public class VocabularyContentConfiguration : IEntityTypeConfiguration<VocabularyContent>
    {
        public void Configure(EntityTypeBuilder<VocabularyContent> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AudioMp3).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.ImageVC).IsRequired();
            builder.Property(x => x.Meaning).IsRequired();
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.Transcribed).IsRequired();
            builder.Property(x => x.VocabularyContentId);
            builder.Property(x => x.Sentence).IsRequired();

        }
    }
}
