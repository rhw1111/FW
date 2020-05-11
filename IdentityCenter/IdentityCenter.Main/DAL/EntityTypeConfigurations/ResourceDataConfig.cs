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
    public class ResourceDataConfig : IEntityTypeConfiguration<ResourceData>
    {
        public void Configure(EntityTypeBuilder<ResourceData> builder)
        {
            builder.ToTable("IdentityAuthorizationCode").HasKey((entity) => entity.ID);

            builder.HasDiscriminator<string>("resource_type")
            .HasValue<ResourceData>("0")
            .HasValue<IdentityResourceData>("1")
            .HasValue<ApiResourceData>("2");

           
            builder.Property<string>("resource_type").HasColumnName("resource_type").HasColumnType("varchar(10)");

            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.DisplayName).HasColumnName("displayname").HasColumnType("nvarchar(250)");
            builder.Property((entity) => entity.Description).HasColumnName("description").HasColumnType("nvarchar(250)");
            builder.Property((entity) => entity.Enabled).IsRequired().HasColumnName("enabled").HasColumnType("bit");

            builder.Property((entity) => entity.Properties).IsRequired().HasColumnName("properties").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<Dictionary<string, string>>(v, null));
            builder.Property((entity) => entity.UserClaims).IsRequired().HasColumnName("userclaims").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));

        }
    }
}
