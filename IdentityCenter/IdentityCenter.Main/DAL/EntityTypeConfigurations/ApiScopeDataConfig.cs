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
    public class ApiScopeDataConfig : IEntityTypeConfiguration<ApiScopeData>
    {
        public void Configure(EntityTypeBuilder<ApiScopeData> builder)
        {
            builder.Property((entity) => entity.Required).IsRequired().HasColumnName("required").HasColumnType("bit");
            builder.Property((entity) => entity.Emphasize).IsRequired().HasColumnName("emphasize").HasColumnType("bit");
        }
    }
}
