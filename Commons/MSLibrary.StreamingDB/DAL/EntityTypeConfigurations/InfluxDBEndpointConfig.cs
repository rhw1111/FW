using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.StreamingDB.InfluxDB;

namespace MSLibrary.StreamingDB.DAL.EntityTypeConfigurations
{
    public class InfluxDBEndpointConfig : IEntityTypeConfiguration<InfluxDBEndpoint>
    {
        public void Configure(EntityTypeBuilder<InfluxDBEndpoint> builder)
        {
            builder.ToTable("influxdbendpoint").HasKey((entity) => entity.ID);
            //builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Address).IsRequired().HasColumnName("address").HasColumnType("varchar(150)");
            //builder.Property((entity) => entity.IsAuth).IsRequired().HasColumnName("isauth").HasColumnType("bit");
            builder.Property((entity) => entity.UserName).HasColumnName("username").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Password).HasColumnName("password").HasColumnType("varchar(150)");
            //builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            //builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");

        }



    }
}
