using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityProviderConfig : IEntityTypeConfiguration<IdentityProvider>
    {
        public void Configure(EntityTypeBuilder<IdentityProvider> builder)
        {
            builder.ToTable("IdentityProvider").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.SchemeName).IsRequired().HasColumnName("schemename").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Type).IsRequired().HasColumnName("type").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Configuration).IsRequired().HasColumnName("configuration").HasColumnType("nvarchar(max)");
            builder.Property((entity) => entity.DisplayName).HasColumnName("displayname").HasColumnType("nvarchar(150)");
            builder.Property((entity) => entity.Icon).HasColumnName("icon").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Active).IsRequired().HasColumnName("active").HasColumnType("bit");
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)").Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
