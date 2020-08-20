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
            builder.OwnsMany(o => o.ApiSecrets, s =>
            {
                s.ToTable("apiresourcesecret");
                s.WithOwner().HasForeignKey("apiresourceid");
                s.Property<Guid>("id");
                s.HasKey("id");
                s.Property((entity) => entity.Type).IsRequired().HasColumnName("type").HasColumnType("varchar(200)");
                s.Property((entity) => entity.Value).IsRequired().HasColumnName("value").HasColumnType("varchar(200)");
                s.Property((entity) => entity.Description).IsRequired().HasColumnName("description").HasColumnType("nvarchar(500)");
                s.Property((entity) => entity.Expiration).HasColumnName("expiration").HasColumnType("datetime");
            });

            builder.OwnsMany(o => o.Scopes, s =>
            {
                s.ToTable("apiresourcescope");
                s.WithOwner().HasForeignKey("apiresourceid");
                s.Property<Guid>("id");
                s.HasKey("id");
                s.Property((entity) => entity).IsRequired().HasColumnName("scope").HasColumnType("varchar(200)");
            });



            builder.Property((entity) => entity.AllowedAccessTokenSigningAlgorithms).HasColumnName("allowedaccesstokensigningalgorithms").HasColumnType("varchar(2000)")
                .HasConversion<string>((v) => JsonSerializerHelper.Serializer(v, null), (v) => JsonSerializerHelper.Deserialize<List<string>>(v, null));
        }
    }
}
