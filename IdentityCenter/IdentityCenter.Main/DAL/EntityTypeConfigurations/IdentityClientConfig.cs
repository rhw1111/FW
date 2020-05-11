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
    public class IdentityClientConfig : IEntityTypeConfiguration<IdentityClient>
    {
        public void Configure(EntityTypeBuilder<IdentityClient> builder)
        {
            builder.ToTable("IdentityClient").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientID).IsRequired().HasColumnName("clientid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientSecrets).HasColumnName("clientsecrets").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v,null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v,null));

            builder.Property((entity) => entity.AllowedGrantTypes).IsRequired().HasColumnName("allowedgranttypes").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));

            builder.Property((entity) => entity.AllowedScopes).IsRequired().HasColumnName("allowedscopes").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));


            builder.Property((entity) => entity.RedirectUris).IsRequired().HasColumnName("redirecturis").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));

            builder.Property((entity) => entity.PostLogoutRedirectUris).IsRequired().HasColumnName("postLogoutredirecturis").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));


            builder.Property((entity) => entity.AllowAccessTokensViaBrowser).IsRequired().HasColumnName("allowaccesstokensviabrowser").HasColumnType("bit");
          

            builder.Property((entity) => entity.PostLogoutRedirectUris).IsRequired().HasColumnName("postLogoutredirecturis").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));

            builder.Property((entity) => entity.RequireConsent).IsRequired().HasColumnName("requireconsent").HasColumnType("bit");
            builder.Property((entity) => entity.AccessTokenLifetime).IsRequired().HasColumnName("accesstokenlifetime").HasColumnType("int");
            builder.Property((entity) => entity.AbsoluteRefreshTokenLifetime).IsRequired().HasColumnName("absoluterefreshtokenlifetime").HasColumnType("int");
            builder.Property((entity) => entity.SlidingRefreshTokenLifetime).IsRequired().HasColumnName("slidingrefreshtokenlifetime").HasColumnType("int");
            builder.Property((entity) => entity.IdentityTokenLifetime).IsRequired().HasColumnName("identitytokenlifetime").HasColumnType("int");


            builder.Property((entity) => entity.AllowedCorsOrigins).IsRequired().HasColumnName("allowedcorsorigins").HasColumnType("varchar(1000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));

         
            builder.Property((entity) => entity.RequirePkce).IsRequired().HasColumnName("requirepkce").HasColumnType("bit");
            builder.Property((entity) => entity.AllowOfflineAccess).IsRequired().HasColumnName("allowofflineaccess").HasColumnType("bit");
            builder.Property((entity) => entity.EnableLocalLogin).IsRequired().HasColumnName("enablelocallogin").HasColumnType("bit");
            builder.Property((entity) => entity.Enabled).IsRequired().HasColumnName("enabled").HasColumnType("bit");

            builder.Property((entity) => entity.ExtensionConfiguration).IsRequired().HasColumnName("extensionconfiguration").HasColumnType("nvarchar(max)");

            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
