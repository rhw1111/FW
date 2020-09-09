using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using IdentityCenter.Main.IdentityServer;
using MSLibrary;
using MSLibrary.Serializer;
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityClientBindingConfig : IEntityTypeConfiguration<IdentityClientBinding>
    {
        public void Configure(EntityTypeBuilder<IdentityClientBinding> builder)
        {
            builder.ToTable("IdentityClientBinding").HasKey((entity) => entity.ID);
            builder.HasDiscriminator<string>("binding_type").HasValue<IdentityClientOpenIDBinding>("0");
            builder.Property<string>("binding_type").HasColumnName("binding_type").HasColumnType("varchar(10)");

            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.IdentityServerUrl).HasColumnName("identityserverurl").IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.IdentityServerInnerUrl).HasColumnName("identityserverinnerurl").IsRequired().HasColumnType("varchar(150)");         
            builder.Property((entity) => entity.AllowReturnBaseUrls).HasColumnName("allowreturnbaseurls").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));
            builder.Property((entity) => entity.AllowedCorsOrigins).HasColumnName("allowedcorsorigins").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
