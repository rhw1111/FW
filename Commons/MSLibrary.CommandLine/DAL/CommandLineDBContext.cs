using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.SSH;
using MSLibrary.CommandLine.DAL.EntityTypeConfigurations;

namespace MSLibrary.CommandLine.DAL
{
    /// <summary>
    /// 命令行相关实体的数据上下文
    /// </summary>
    public class CommandLineDBContext: DbContext
    {
        public CommandLineDBContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// SSH终结点
        /// </summary>
        public DbSet<SSHEndpoint> SSHEndpoints { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<SSHEndpoint>(new SSHEndpointConfig());
        }
    }
}
