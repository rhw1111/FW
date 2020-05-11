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
    public class ApiResourceDataConfig : IEntityTypeConfiguration<ApiResourceData>
    {
        public void Configure(EntityTypeBuilder<ApiResourceData> builder)
        {
            builder.Property((entity) => entity.ApiSecrets).IsRequired().HasColumnName("apisecrets").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));
        }
    }
}
