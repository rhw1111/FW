using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.SSH;
using MSLibrary.StreamingDB.InfluxDB;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.DAL.EntityTypeConfigurations;

namespace FW.TestPlatform.Main.DAL
{
    public class ConfigurationDBContext : DbContext
    {
        public ConfigurationDBContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// InfluxDB终结点
        /// </summary>
        public DbSet<InfluxDBEndpoint> InfluxDBEndpoints { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<InfluxDBEndpoint>(new InfluxDBEndpointConfig());
        }
    }
}
