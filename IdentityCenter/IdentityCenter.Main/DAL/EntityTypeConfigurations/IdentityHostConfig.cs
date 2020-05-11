using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.Serializer;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityHostConfig : IEntityTypeConfiguration<IdentityHost>
    {
        public void Configure(EntityTypeBuilder<IdentityHost> builder)
        {
            builder.ToTable("IdentityHost").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClaimContextGeneratorName).IsRequired().HasColumnName("claimcontextgeneratorname").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.EnvironmentClaimGeneratorName).IsRequired().HasColumnName("environmentclaimgeneratorname").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ExternalCallbackUri).IsRequired().HasColumnName("externalcallbackuri").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ExternalIdentityBindPage).IsRequired().HasColumnName("externalidentitybindpage").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ExternalLogoutCallbackUri).IsRequired().HasColumnName("externallogoutcallbackuri").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.LoggedPage).IsRequired().HasColumnName("loggedpage").HasColumnType("varchar(150)");
            builder.OwnsOne((entity) => entity.LocalLoginSetting, (setting) =>
             {
                 setting.Property((entity) => entity.AllowLocalLogin).IsRequired().HasColumnName("localLoginsetting_allowLocallogin").HasColumnType("bit");
                 setting.Property((entity) => entity.AllowRememberLogin).IsRequired().HasColumnName("localLoginsetting_allowrememberlogin").HasColumnType("bit");
                 setting.Property((entity) => entity.RememberLoginDuration).IsRequired().HasColumnName("localLoginsetting_rememberloginduration").HasColumnType("int");
                 setting.Property((entity) => entity.ShowLogoutPrompt).IsRequired().HasColumnName("localLoginsetting_showlogoutprompt").HasColumnType("bit");
             });
            builder.Property((entity) => entity.AllowedCorsOrigins).IsRequired().HasColumnName("allowedcorsorigins").HasColumnType("varchar(2000)")
                    .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                    (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));

            builder.Property((entity) => entity.ServerOptionsConfiguration).IsRequired().HasColumnName("serveroptionsconfiguration").HasColumnType("nvarchar(max)");
            builder.Property((entity) => entity.SigningCredentialConfiguration).IsRequired().HasColumnName("signingcredentialconfiguration").HasColumnType("nvarchar(max)");
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
