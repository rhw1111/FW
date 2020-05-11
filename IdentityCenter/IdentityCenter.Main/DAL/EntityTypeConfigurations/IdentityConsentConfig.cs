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
    public class IdentityConsentConfig : IEntityTypeConfiguration<IdentityConsent>
    {
        public void Configure(EntityTypeBuilder<IdentityConsent> builder)
        {
            builder.ToTable("IdentityConsent").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.SubjectId).IsRequired().HasColumnName("subjectid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientId).IsRequired().HasColumnName("clientid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Scopes).IsRequired().HasColumnName("scopes").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null),
                (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));
            builder.Property((entity) => entity.CreationTime).IsRequired().HasColumnName("creationtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.Expiration).HasColumnName("expiration").HasColumnType("datetime2(7)");
        }
    }
}
