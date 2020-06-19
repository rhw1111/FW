using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.StreamingDB.InfluxDB;
using MSLibrary.StreamingDB.DAL.EntityTypeConfigurations;

namespace MSLibrary.StreamingDB.DAL
{
    public class StreamingDBDBContext : DbContext
    {
        public StreamingDBDBContext(DbContextOptions options) : base(options)
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
