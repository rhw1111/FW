using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.CommandLine.SSH;
using MSLibrary.Collections;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.DAL.EntityTypeConfigurations;

namespace FW.TestPlatform.Main.DAL
{
    public class MainDBContext:DbContext
    {
        public MainDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TreeEntity> TreeEntities { get; set; } = null!;
        /// <summary>
        /// SSH终结点
        /// </summary>
        public DbSet<SSHEndpoint> SSHEndpoints { get; set; } = null!;

        /// <summary>
        /// 测试主机
        /// </summary>
        public DbSet<TestHost> TestHosts { get; set; } = null!;

        /// <summary>
        /// 脚本模板
        /// </summary>
        public DbSet<ScriptTemplate> ScriptTemplates { get; set; } = null!;

        /// <summary>
        /// 测试案例
        /// </summary>
        public DbSet<TestCase> TestCases { get; set; } = null!;

        /// <summary>
        /// 测试案例历史
        /// </summary>
        public DbSet<TestCaseHistory> TestCaseHistories { get; set; } = null!;


        /// <summary>
        /// 测试实例Slave主机
        /// </summary>
        public DbSet<TestCaseSlaveHost> TestCaseSlaveHosts { get; set; } = null!;

        /// <summary>
        /// 测试数据源
        /// </summary>
        public DbSet<TestDataSource> TestDataSources { get; set; } = null!;

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<ScriptTemplate>(new ScriptTemplateConfig());
            modelBuilde.ApplyConfiguration<SSHEndpoint>(new SSHEndpointConfig());
            modelBuilde.ApplyConfiguration<TestCase>(new TestCaseConfig());
            modelBuilde.ApplyConfiguration<TestCaseHistory>(new TestCaseHistoryConfig());
            modelBuilde.ApplyConfiguration<TestCaseSlaveHost>(new TestCaseSlaveHostConfig());
            modelBuilde.ApplyConfiguration<TestDataSource>(new TestDataSourceConfig());
            modelBuilde.ApplyConfiguration<TestHost>(new TestHostConfig());
            modelBuilde.ApplyConfiguration<User>(new UserConfig());
            modelBuilde.ApplyConfiguration<TreeEntity>(new TreeEntityConfig());
        }
    }
}
