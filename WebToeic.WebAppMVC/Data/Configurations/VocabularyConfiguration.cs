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
    public class VocabularyConfiguration : IEntityTypeConfiguration<Vocabulary>
    {
        public void Configure(EntityTypeBuilder<Vocabulary> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImageV).IsRequired();
            builder.Property(x => x.VocabularyName).IsRequired();

            builder.HasMany(x => x.VocabularyContents)
                   .WithOne(v => v.Vocabulary)
                   .HasForeignKey(v => v.VocabularyContentId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
