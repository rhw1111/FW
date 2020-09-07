using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.Entities;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class UserThirdPartyAccountConfig : IEntityTypeConfiguration<UserThirdPartyAccount>
    {
        public void Configure(EntityTypeBuilder<UserThirdPartyAccount> builder)
        {
            builder.ToTable("UserThirdPartyAccount").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.AccountID).IsRequired().HasColumnName("accountid").HasColumnType("uniqueidentifier");
            builder.HasOne(entity => entity.Account).WithMany().IsRequired().HasForeignKey(entity => entity.AccountID);
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore); ;
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.Source).IsRequired().HasColumnName("source").HasColumnType("varchar(200)");
            builder.Property((entity) => entity.ThirdPartyID).IsRequired().HasColumnName("thirdpartyid").HasColumnType("varchar(200)");
            builder.Property((entity) => entity.ExtensionInfo).IsRequired().HasColumnName("extensioninfo").HasColumnType("nvarchar(max)");
        }
    }
}
