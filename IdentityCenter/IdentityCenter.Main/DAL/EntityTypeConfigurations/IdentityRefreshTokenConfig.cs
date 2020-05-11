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
    public class IdentityRefreshTokenConfig : IEntityTypeConfiguration<IdentityRefreshToken>
    {
        public void Configure(EntityTypeBuilder<IdentityRefreshToken> builder)
        {
            builder.ToTable("IdentityRefreshToken").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Lifetime).IsRequired().HasColumnName("lifetime").HasColumnType("int");
            builder.Property((entity) => entity.Handle).IsRequired().HasColumnName("handle").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.SubjectId).IsRequired().HasColumnName("subjectid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientId).IsRequired().HasColumnName("clientid").HasColumnType("varchar(150)");

            builder.Property((entity) => entity.CreationTime).IsRequired().HasColumnName("creationtime").HasColumnType("datetime2(7)");

            builder.OwnsOne((entity) => entity.AccessToken, (setting) =>
            {
                setting.ToTable("IdentityRefreshToken_AccessToken");
                setting.Property((entity) => entity.Audiences).HasColumnName("audiences").HasColumnType("nvarchar(2000)")
                .HasConversion((v)=>JsonSerializerHelper.Serializer(v,null),(v)=>JsonSerializerHelper.Deserialize<List<string>>(v,null));
                setting.Property((entity) => entity.Issuer).HasColumnName("issuer").HasColumnType("varchar(150)");
                setting.Property((entity) => entity.CreationTime).IsRequired().HasColumnName("creationtime").HasColumnType("datetime2(7)");
                setting.Property((entity) => entity.Lifetime).IsRequired().HasColumnName("lifetime").HasColumnType("int");
                setting.Property((entity) => entity.Type).HasColumnName("type").HasColumnType("varchar(150)");
                setting.Property((entity) => entity.ClientId).HasColumnName("clientid").HasColumnType("varchar(150)");
                setting.Property((entity) => entity.Claims).HasColumnName("claims").HasColumnType("nvarchar(2000)")
                .HasConversion((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));
                setting.Property((entity) => entity.Version).IsRequired().HasColumnName("version").HasColumnType("int");
            });

            builder.Property((entity) => entity.Version).IsRequired().HasColumnName("version").HasColumnType("int");
        }
    }
}
