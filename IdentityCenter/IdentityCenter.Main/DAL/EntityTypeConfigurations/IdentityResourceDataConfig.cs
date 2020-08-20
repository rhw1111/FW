using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.IdentityServer;
using MSLibrary;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityResourceDataConfig : IEntityTypeConfiguration<IdentityResourceData>
    {
        public void Configure(EntityTypeBuilder<IdentityResourceData> builder)
        {

            builder.Property((entity) => entity.Required).IsRequired().HasColumnName("required").HasColumnType("bit");
            builder.Property((entity) => entity.Emphasize).IsRequired().HasColumnName("emphasize").HasColumnType("bit");
        }
    }
}
