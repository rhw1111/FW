using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.IdentityServer;
using MSLibrary;
using MSLibrary.Serializer;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityAuthorizationCodeConfig : IEntityTypeConfiguration<IdentityAuthorizationCode>
    {
        public void Configure(EntityTypeBuilder<IdentityAuthorizationCode> builder)
        {
            builder.ToTable("IdentityAuthorizationCode").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Code).IsRequired().HasColumnName("code").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClientId).IsRequired().HasColumnName("clientid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.CodeChallenge).HasColumnName("codechallenge").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.CodeChallengeMethod).HasColumnName("codechallengemethod").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.SubjectData).IsRequired().HasColumnName("subjectdata").HasColumnType("nvarchar(1500)");
            builder.Property((entity) => entity.Lifetime).HasColumnName("lifetime").HasColumnType("int");
            builder.Property((entity) => entity.IsOpenId).HasColumnName("isopenid").HasColumnType("bit");
            builder.Property((entity) => entity.RedirectUri).HasColumnName("redirecturi").HasColumnType("varchar(300)");
            builder.Property((entity) => entity.Nonce).HasColumnName("nonce").HasColumnType("varchar(300)");
            builder.Property((entity) => entity.StateHash).HasColumnName("statehash").HasColumnType("varchar(300)");
            builder.Property((entity) => entity.WasConsentShown).HasColumnName("wasconsentshown").IsRequired().HasColumnType("bit");
            builder.Property((entity) => entity.SessionId).HasColumnName("sessionid").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Properties).IsRequired().HasColumnName("properties").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v,null),(v)=>JsonSerializerHelper.Deserialize<Dictionary<string,string>>(v,null));
            builder.Property((entity) => entity.RequestedScopes).IsRequired().HasColumnName("requestedscopes").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<string[]>(v, null));
            builder.Property((entity) => entity.CreationTime).IsRequired().HasColumnName("creationtime").HasColumnType("datetime2(7)").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        }
    }
}
