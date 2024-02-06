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
    public class GrammarConfiguration : IEntityTypeConfiguration<Grammar>
    {
        public void Configure(EntityTypeBuilder<Grammar> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImageG).IsRequired();
            builder.Property(x => x.HtmlContent).IsRequired();
            builder.Property(x => x.MarkDownContent).IsRequired();
            builder.Property(x => x.GrammarName).IsRequired();

        }

    }
}
