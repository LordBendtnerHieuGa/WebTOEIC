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
    public class SlideBannerConfiguration : IEntityTypeConfiguration<SlideBanner>
    {
        public void Configure(EntityTypeBuilder<SlideBanner> builder)
        {
            builder.ToTable("SlideBanners");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.SlideName);
            builder.Property(x => x.SlideContent);
            builder.Property(x => x.ImageS);

        }
    }
}
