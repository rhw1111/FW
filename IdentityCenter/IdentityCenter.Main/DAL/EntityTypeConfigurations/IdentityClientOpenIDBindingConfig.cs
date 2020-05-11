using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.IdentityServer;
using MSLibrary;
using MSLibrary.Serializer;
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityClientOpenIDBindingConfig : IEntityTypeConfiguration<IdentityClientOpenIDBinding>
    {
        public void Configure(EntityTypeBuilder<IdentityClientOpenIDBinding> builder)
        {
            builder.Property((entity) => entity.ClientId).IsRequired().HasColumnName("clientid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientSecret).IsRequired().HasColumnName("clientsecret").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ResponseMode).IsRequired().HasColumnName("responsemode").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ResponseType).IsRequired().HasColumnName("responsetype").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Authority).IsRequired().HasColumnName("authority").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.RequireHttpsMetadata).IsRequired().HasColumnName("requirehttpsmetadata").HasColumnType("bit");
            builder.Property((entity) => entity.TokenUseQuery).IsRequired().HasColumnName("tokenusequery").HasColumnType("bit");
            builder.Property((entity) => entity.AccessDeniedPath).HasColumnName("accessdeniedpath").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.RedirectUrl).IsRequired().HasColumnName("redirecturl").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Scope).HasColumnName("scope").HasColumnType("varchar(2000)")
                    .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                    (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));

        }
    }
}
