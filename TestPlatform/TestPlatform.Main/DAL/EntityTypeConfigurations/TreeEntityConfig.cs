using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class TreeEntityConfig : IEntityTypeConfiguration<TreeEntity>
    {
        public void Configure(EntityTypeBuilder<TreeEntity> builder)
        {
            builder.ToTable("treeentity").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("char(36)");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Type).IsRequired().HasColumnName("type").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Value).IsRequired().HasColumnName("value").HasColumnType("mediumtext");

            builder.Property((entity) => entity.ParentID).HasColumnType("char(36)");
            builder.HasOne((entity) => entity.Parent).WithMany().HasForeignKey(entity => entity.ParentID);
            var parentProperty = builder.Property((entity) => entity.Parent).Metadata;
            parentProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            parentProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime");
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
