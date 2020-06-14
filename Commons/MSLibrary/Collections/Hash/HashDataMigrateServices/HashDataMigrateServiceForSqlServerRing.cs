using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Transaction;

namespace MSLibrary.Collections.Hash.HashDataMigrateServices
{
    /// <summary>
    /// 针对存储类型为SqlServre、策略为哈希环的数据迁移服务
    /// </summary>
    [Injection(InterfaceType = typeof(HashDataMigrateServiceForSqlServerRing), Scope = InjectionScope.Singleton)]
    public class HashDataMigrateServiceForSqlServerRing : IHashDataMigrateService
    {
        private IHashDataMigrateSqlServerRingDataService _hashDataMigrateSqlServerRingDataService;
        private IHashDataTotalMigrateSqlServerRingDataService _hashDataTotalMigrateSqlServerRingDataService;


        public HashDataMigrateServiceForSqlServerRing(IHashDataMigrateSqlServerRingDataService hashDataMigrateSqlServerRingDataService, IHashDataTotalMigrateSqlServerRingDataService hashDataTotalMigrateSqlServerRingDataService)
        {
            _hashDataMigrateSqlServerRingDataService = hashDataMigrateSqlServerRingDataService;
            _hashDataTotalMigrateSqlServerRingDataService = hashDataTotalMigrateSqlServerRingDataService;
        }

        public async Task Migrate(HashGroup hashGroup, Func<HashDataMigrateContext, Task> callback)
        {

            List<Exception> exceptions = new List<Exception>();

            //获取新增哈希组中按code顺序排列的前10个哈希节点
            var nodeList = await hashGroup.GetHashNodeOrderByCode(0, 10);

            List<Task> taskList = new List<Task>();

            //先对新增的节点做处理
            for (var index = 0; index <= nodeList.Count - 1; index++)
            {
                var vIndex = index;
                taskList.Add(Task.Run(async () =>
                {
                    var itemIndex = vIndex;
                    var nextItem = nodeList[vIndex];

                    try
                    {
                        while (nextItem != null)
                        {

                            HashDataMigrateContext migrateContext = new HashDataMigrateContext()
                            {
                                ExecuteHashNode = nextItem,
                                ExecuteHashRealNode = nextItem.RealNode
                            };

                            var realNode = nextItem.RealNode;
                            //判断节点的状态
                            switch (nextItem.Status)
                            {
                                case 0:
                                case 2:
                                    //需要找到后一个状态为1（原有节点）或3（待删除）的节点
                                    var firstNewNode = await hashGroup.GetFirstByGreaterCode(nextItem.Code, 1, 3);
                                    if (firstNewNode == null)
                                    {
                                        firstNewNode = await hashGroup.GetMinCode(1, 3);
                                    }

                                    if (firstNewNode == null)
                                    {
                                        firstNewNode = await hashGroup.GetFirstByGreaterCode(0, 1, 3);
                                    }

                                    if (firstNewNode != null)
                                    {
                                        //判断两个节点是否指向同一个真实节点，如果是，则不做处理
                                        //如果不是，则需要将firstNewNode指向的真实节点的部分数据转移到nextItem的真实节点上去
                                        if (firstNewNode.RealNodeId != nextItem.RealNodeId)
                                        {

                                            //获取nextItem节点的前一个节点
                                            long startNodeCode = 0;
                                            var preItem = await hashGroup.GetFirstLessNode(nextItem);
                                            if (preItem != null)
                                            {
                                                startNodeCode = preItem.Code;
                                            }

                                            migrateContext.ExecuteStatus = 1;
                                            await callback(migrateContext);
                                            //转移数据
                                            //首先从firstNewNode真实节点中获取要转移的数据，分批新增到目标真实节点
                                            await _hashDataMigrateSqlServerRingDataService.GetMigrateDataFromSource(startNodeCode, nextItem.Code, firstNewNode.RealNode.NodeKey, async (datas) =>
                                            {
                                                await _hashDataMigrateSqlServerRingDataService.AddMigrateDataToTarget(nextItem.RealNode.NodeKey, datas);
                                            }
                                            );

                                            //如果nextItem的状态为0，则修改nextItem的状态为2
                                            if (nextItem.Status == 0)
                                            {
                                                var updateNextItemNode = new HashNode()
                                                {
                                                    ID = nextItem.ID,
                                                    Status = 2
                                                };

                                                await hashGroup.UpdateNodeStatus(updateNextItemNode);


                                                //需要再转移一次数据
                                                await _hashDataMigrateSqlServerRingDataService.GetMigrateDataFromSource(startNodeCode, nextItem.Code, firstNewNode.RealNode.NodeKey, async (datas) =>
                                                {
                                                    await _hashDataMigrateSqlServerRingDataService.AddMigrateDataToTarget(nextItem.RealNode.NodeKey, datas);
                                                }
                                                );
                                            }

                                            //从源真实节点中删除
                                            await _hashDataMigrateSqlServerRingDataService.DeleteMigrateDataFromSource(startNodeCode, nextItem.Code, firstNewNode.RealNode.NodeKey);

                                            //修改item状态为1
                                            /*var updateItem = new HashNode()
                                            {
                                                ID = nextItem.ID,
                                                Status = 1
                                            };

                                            await hashGroup.UpdateNode(updateItem);*/

                                            migrateContext.ExecuteStatus = 2;
                                            await callback(migrateContext);
                                        }
                                        else
                                        {
                                            //修改item状态为2
                                            var updateItem = new HashNode()
                                            {
                                                ID = nextItem.ID,
                                                Status = 2
                                            };
                                            await hashGroup.UpdateNodeStatus(updateItem);
                                        }
                                    }
                                    else
                                    {
                                        //修改item状态为2
                                        var updateItem = new HashNode()
                                        {
                                            ID = nextItem.ID,
                                            Status = 2
                                        };
                                        await hashGroup.UpdateNodeStatus(updateItem);
                                    }

                                    break;
                                default:
                                    break;
                            }

                            //获取下一个要处理的节点
                            itemIndex = itemIndex + 10;
                            var nextItemList = await hashGroup.GetHashNodeOrderByCode(itemIndex, 1);

                            if (nextItemList.Count == 0)
                            {
                                nextItem = null;
                            }
                            else
                            {
                                nextItem = nextItemList[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }


                }));
            }

            //等待每个任务完成
            foreach (var itemTask in taskList)
            {
                await itemTask;
            }

            if (exceptions.Count > 0)
            {
                StringBuilder strError = new StringBuilder();
                foreach (var errorItem in exceptions)
                {
                    strError.Append($"Message:{errorItem.Message},StackTrace:{errorItem.StackTrace}\r\n\r\n");
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.HashDataMigrateErrorByGroup,
                    DefaultFormatting = "哈希组{0}执行数据迁移时发生错误，错误详细信息为{1}",
                    ReplaceParameters = new List<object>() { hashGroup.Name, strError.ToString() }
                };

                throw new UtilityException((int)Errors.HashDataMigrateErrorByGroup,fragment);
            }

            //修改状态为2的节点状态为1
            int count = 1000;
            while (count == 1000)
            {
                var qResult = await hashGroup.GetHashNode(2, 1, count);
                count = qResult.Results.Count;
                foreach (var item in qResult.Results)
                {
                    //修改item状态为1
                    var updateItem = new HashNode()
                    {
                        ID = item.ID,
                        Status = 1
                    };
                    await hashGroup.UpdateNodeStatus(updateItem);
                }
            }

            //再对删除的节点做处理
            for (var index = 0; index <= nodeList.Count - 1; index++)
            {
                var vIndex = index;
                taskList.Add(Task.Run(async () =>
                {
                    var itemIndex = vIndex;
                    var nextItem = nodeList[vIndex];

                    try
                    {
                        while (nextItem != null)
                        {

                            HashDataMigrateContext migrateContext = new HashDataMigrateContext()
                            {
                                ExecuteHashNode = nextItem,
                                ExecuteHashRealNode = nextItem.RealNode
                            };

                            var realNode = nextItem.RealNode;
                            //判断节点的状态
                            switch (nextItem.Status)
                            {
                                case 3:
                                case 4:
                                    ////需要找到后一个状态为0（新节点）或1（原有节点）
                                    var firstNewNode = await hashGroup.GetFirstByGreaterCode(nextItem.Code, 0, 1);
                                    if (firstNewNode == null)
                                    {
                                        firstNewNode = await hashGroup.GetMinCode(0, 1);
                                    }

                                    if (firstNewNode == null)
                                    {
                                        firstNewNode = await hashGroup.GetFirstByGreaterCode(0, 0, 1);
                                    }

                                    if (firstNewNode != null)
                                    {
                                        //判断两个节点是否指向同一个真实节点，如果是，则不做处理
                                        //如果不是，则需要将item指向的真实节点的全部数据转移到firstNewNode的真实节点上去
                                        if (firstNewNode.RealNodeId != nextItem.RealNodeId)
                                        {
                                            migrateContext.ExecuteStatus = 1;
                                            await callback(migrateContext);


                                            //获取nextItem节点的前一个节点
                                            long startNodeCode = 0;
                                            var preItem = await hashGroup.GetFirstLessNode(nextItem);
                                            if (preItem != null)
                                            {
                                                startNodeCode = preItem.Code;
                                            }


                                            //转移数据
                                            //首先从源真实节点中获取要转移的数据，分批新增到目标真实节点
                                            await _hashDataMigrateSqlServerRingDataService.GetMigrateDataFromSource(startNodeCode, nextItem.Code, nextItem.RealNode.NodeKey, async (datas) =>
                                            {
                                                await _hashDataMigrateSqlServerRingDataService.AddMigrateDataToTarget(firstNewNode.RealNode.NodeKey, datas);
                                            }
                                         );

                                            //修改item状态为4
                                            if (nextItem.Status == 3)
                                            {
                                                //修改item状态为4
                                                var updateItem = new HashNode()
                                                {
                                                    ID = nextItem.ID,
                                                    Status = 4
                                                };
                                                await hashGroup.UpdateNodeStatus(updateItem);

                                                //需要再转移一次数据
                                                await _hashDataMigrateSqlServerRingDataService.GetMigrateDataFromSource(startNodeCode, nextItem.Code, nextItem.RealNode.NodeKey, async (datas) =>
                                                {
                                                    await _hashDataMigrateSqlServerRingDataService.AddMigrateDataToTarget(firstNewNode.RealNode.NodeKey, datas);
                                                }
                                             );
                                            }



                                            //从源真实节点中删除
                                            await _hashDataMigrateSqlServerRingDataService.DeleteMigrateDataFromSource(startNodeCode, nextItem.Code, nextItem.RealNode.NodeKey);

                                            //修改item状态为5
                                            /*var updateItemNode = new HashNode()
                                            {
                                                ID = nextItem.ID,
                                                Status = 5
                                            };

                                            await hashGroup.UpdateNode(updateItemNode);
                                            */

                                            migrateContext.ExecuteStatus = 2;
                                            await callback(migrateContext);
                                        }
                                        else
                                        {
                                            //修改item状态为4
                                            var updateItem = new HashNode()
                                            {
                                                ID = nextItem.ID,
                                                Status = 4
                                            };
                                            await hashGroup.UpdateNodeStatus(updateItem);
                                        }
                                    }
                                    else
                                    {
                                        //修改item状态为4
                                        var updateItem = new HashNode()
                                        {
                                            ID = nextItem.ID,
                                            Status = 4
                                        };
                                        await hashGroup.UpdateNodeStatus(updateItem);
                                    }
                                    break;
                                default:

                                    break;
                            }

                            //获取下一个要处理的节点
                            itemIndex = itemIndex + 10;
                            var nextItemList = await hashGroup.GetHashNodeOrderByCode(itemIndex, 1);

                            if (nextItemList.Count == 0)
                            {
                                nextItem = null;
                            }
                            else
                            {
                                nextItem = nextItemList[0];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }
                }));
            }

            //等待每个任务完成
            foreach (var itemTask in taskList)
            {
                await itemTask;
            }

            if (exceptions.Count > 0)
            {
                StringBuilder strError = new StringBuilder();
                foreach (var errorItem in exceptions)
                {
                    strError.Append($"Message:{errorItem.Message},StackTrace:{errorItem.StackTrace}\r\n\r\n");
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.HashDataMigrateErrorByGroup,
                    DefaultFormatting = "哈希组{0}执行数据迁移时发生错误，错误详细信息为{1}",
                    ReplaceParameters = new List<object>() { hashGroup.Name, strError.ToString() }
                };


                throw new UtilityException((int)Errors.HashDataMigrateErrorByGroup, fragment);
            }

            //修改状态为4的节点状态为5
            count = 1000;
            while (count == 1000)
            {
                var qResult = await hashGroup.GetHashNode(4, 1, count);
                count = qResult.Results.Count;
                foreach (var item in qResult.Results)
                {
                    //修改item状态为5
                    var updateItem = new HashNode()
                    {
                        ID = item.ID,
                        Status = 5
                    };
                    await hashGroup.UpdateNodeStatus(updateItem);
                }
            }
        }

        public async Task TotalMigrate(HashGroup hashGroup, Func<HashDataMigrateContext, Task> callback)
        {
            List<Exception> exceptions = new List<Exception>();
            //获取哈希组中前10个真实节点
            var nodeList = await hashGroup.GetHashRealNodeByCreateTime(0, 10);
            List<Task> taskList = new List<Task>();
            for (var index = 0; index <= nodeList.Count - 1; index++)
            {
                var vIdex = index;
                taskList.Add(Task.Run(async () =>
                {
                    var itemIndex = vIdex;
                    var nextItem = nodeList[vIdex];

                    try
                    {
                        while (nextItem != null)
                        {
                            HashDataMigrateContext migrateContext = new HashDataMigrateContext()
                            {
                                ExecuteHashRealNode = nextItem
                            };

                            migrateContext.ExecuteStatus = 1;
                            await callback(migrateContext);
                            //获取该真实节点的所有数据
                            var datas = await _hashDataTotalMigrateSqlServerRingDataService.GetAllDataFromSource(nextItem.NodeKey, 1000);

                            var deleteDatas = new Dictionary<string, TableDataInfo>();
                            Dictionary<string, Dictionary<string, TableDataInfo>> addDatas = new Dictionary<string, Dictionary<string, TableDataInfo>>();

                            foreach (var dataItem in datas)
                            {
                                foreach (var rowItem in dataItem.Value.Rows)
                                {
                                    //计算每条数据经过计算后的应该存储的真实节点
                                    var targetRealNodeKey = await hashGroup.GetHashNodeKey(rowItem.RowKey, 0, 1);

                                    if (!addDatas.TryGetValue(targetRealNodeKey, out Dictionary<string, TableDataInfo> dictTableInfo))
                                    {
                                        dictTableInfo = new Dictionary<string, TableDataInfo>();
                                        addDatas.Add(targetRealNodeKey, dictTableInfo);
                                    }

                                    if (!dictTableInfo.TryGetValue(dataItem.Key, out TableDataInfo tableInfo))
                                    {
                                        tableInfo = new TableDataInfo() { EntityName = dataItem.Value.EntityName };
                                        dictTableInfo.Add(dataItem.Key, tableInfo);
                                    }

                                    tableInfo.Rows.Add(rowItem);
                                    //新节点与原节点不一致则需要记录下来统一删除
                                    if (nextItem.NodeKey != targetRealNodeKey)
                                    {
                                        if (!deleteDatas.TryGetValue(dataItem.Key, out TableDataInfo delTableInfo))
                                        {
                                            delTableInfo = new TableDataInfo() { EntityName = dataItem.Value.EntityName };
                                            deleteDatas.Add(dataItem.Key, delTableInfo);
                                        }
                                        deleteDatas[dataItem.Key].Rows.Add(rowItem);
                                    }
                                }
                            }

                            //新增到计算后的真实节点
                            foreach (var addDataItem in addDatas)
                            {
                                await _hashDataTotalMigrateSqlServerRingDataService.AddMigrateDataToTarget(addDataItem.Key, addDataItem.Value);
                            }

                            //从原来的真实节点删除
                            await _hashDataTotalMigrateSqlServerRingDataService.DeleteMigrateDataFromSource(nextItem.NodeKey, deleteDatas);


                            migrateContext.ExecuteStatus = 2;
                            await callback(migrateContext);


                            //获取下一个要处理的节点
                            itemIndex = itemIndex + 10;
                            var nextItemList = await hashGroup.GetHashRealNodeByCreateTime(itemIndex, 1);

                            if (nextItemList.Count == 0)
                            {
                                nextItem = null;
                            }
                            else
                            {
                                nextItem = nextItemList[0];
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }


                }
                ));
            }



            //等待每个任务完成
            foreach (var itemTask in taskList)
            {
                await itemTask;
            }

            if (exceptions.Count > 0)
            {
                StringBuilder strError = new StringBuilder();
                foreach (var errorItem in exceptions)
                {
                    strError.Append($"Message:{errorItem.Message},StackTrace:{errorItem.StackTrace}\r\n\r\n");
                }


                var fragment = new TextFragment()
                {
                    Code = TextCodes.HashDataTotalMigrateErrorByGroup,
                    DefaultFormatting = "哈希组{0}执行整体数据迁移时发生错误，错误详细信息为{1}",
                    ReplaceParameters = new List<object>() { hashGroup.Name, strError.ToString() }
                };

                throw new UtilityException((int)Errors.HashDataTotalMigrateErrorByGroup, fragment);
            }
        }

    }

    /// <summary>
    /// 表数据信息
    /// </summary>
    public class TableDataInfo
    {

        /// <summary>
        /// 表示该表代表的实体名称
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 数据行
        /// </summary>
        public List<RowDataInfo> Rows { get; } = new List<RowDataInfo>();
    }


    /// <summary>
    /// 行数据信息
    /// </summary>
    public class RowDataInfo
    {
        /// <summary>
        /// 表示该行产生的哈希Code
        /// </summary>
        public long RowCode { get; set; }
        /// <summary>
        /// 表示该行产生的哈希Key
        /// </summary>
        public string RowKey { get; set; }
        /// <summary>
        /// 表示行数据，column:value
        /// </summary>
        public Dictionary<string, object> RowData { get; } = new Dictionary<string, object>();
    }


    /// <summary>
    /// 针对存储类型为SqlServre、策略为哈希环的数据迁移的数据操作服务
    /// </summary>
    public interface IHashDataMigrateSqlServerRingDataService
    {
        /// <summary>
        /// 从源真实节点获取要转移的数据
        /// </summary>
        /// <param name="sourceNodeCode"></param>
        /// <param name="targetNodeCode"></param>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetMigrateDataFromSource(long sourceNodeCode, long targetNodeCode, string sourceRealNodeKey, Func<Dictionary<string, TableDataInfo>, Task> callback);
        /// <summary>
        /// 从源真实节点获取所有数据
        /// </summary>
        /// <param name="sourceNodeCode"></param>
        /// <param name="targetNodeCode"></param>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task GetAllDataFromSource(long sourceNodeCode, long targetNodeCode, string sourceRealNodeKey, Func<Dictionary<string, TableDataInfo>, Task> callback);

        /// <summary>
        /// 将要转移的数据增加到目标真实节点中
        /// </summary>
        /// <param name="targetRealNodeKey"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task AddMigrateDataToTarget(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas);
        /// <summary>
        /// 将要转移的数据从源真实节点中删除
        /// </summary>
        /// <param name="sourceNodeCode"></param>
        /// <param name="targetNodeCode"></param>
        /// <param name="sourceRealNodeKey"></param>
        /// <returns></returns>
        Task DeleteMigrateDataFromSource(long sourceNodeCode, long targetNodeCode, string sourceRealNodeKey);
    }


    [Injection(InterfaceType = typeof(IHashDataMigrateSqlServerRingDataService), Scope = InjectionScope.Singleton)]
    public class HashDataMigrateSqlServerRingDataService : IHashDataMigrateSqlServerRingDataService
    {

        public async Task AddMigrateDataToTarget(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas)
        {
            //解析目标服务器信息
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(targetRealNodeKey);

            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            //遍历需要写入的数据集合
            foreach (var data in datas)
            {
                if (!nodeKey.TableNames.TryGetValue(data.Key, out string tableName))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundHashDataMigrateTargetDB,
                        DefaultFormatting = "将要转移的数据增加到目标真实节点时发生错误，找不到目标数据库节点：{0}",
                        ReplaceParameters = new List<object>() { data.Key }
                    };

                    throw new UtilityException((int)Errors.NotFoundHashDataMigrateTargetDB,
                        fragment);
                }



                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(data.Key);

                DataTable dt = new DataTable();

                //遍历每一条数据在DB中是否存在，若不存在则写入
                foreach (RowDataInfo row in data.Value.Rows)
                {
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                    {
                        SqlTransaction sqlTran = null;
                        if (transaction != null)
                        {
                            sqlTran = (SqlTransaction)transaction;
                        }
                        using (SqlCommand command = new SqlCommand()
                        {
                            Connection = (SqlConnection)conn,
                            CommandType = CommandType.Text,
                            CommandText = string.Format(@"
                                                          select top 1 *
                                                          from {0}
                                                          where {1}
                                                      ", tableName, HashDataMigrateHelper.GetConditions(keyFields, row.RowData)),
                            Transaction = sqlTran,
                        })
                        {
                            command.Prepare();

                            SqlDataReader reader = null;

                            using (reader = await command.ExecuteReaderAsync())
                            {
                                //DB中不存在的数据需要迁移到目标DB中
                                if (!await reader.ReadAsync())
                                {
                                    HashDataMigrateHelper.GetDataTable(ref dt, row.RowData, HashKeyFields.GetUnMigrateFields(data.Key));
                                }
                                reader.Close();
                            }
                        }
                    });
                }

                if (dt.Rows.Count > 0)
                {
                    //批量插入
                    using (SqlConnection destConnection = new SqlConnection(connAll))
                    {
                        destConnection.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destConnection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            await bulkCopy.WriteToServerAsync(dt);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 将要转移的数据从源真实节点中删除
        /// </summary>
        /// <param name="preSourceNodeCode"></param>
        /// <param name="targetNodeCode"></param>
        /// <param name="sourceRealNodeKey"></param>
        /// <returns></returns>
        public async Task DeleteMigrateDataFromSource(long preSourceNodeCode, long targetNodeCode, string sourceRealNodeKey)
        {
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(sourceRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            //遍历该节点下包含的所有需要迁移的表
            foreach (var n in nodeKey.TableNames)
            {
                var tableName = n.Value;

                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(n.Key);

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"
                                                       while exists( select
                                                                         top(1)
                                                                        *
                                                                    from {2}
                                                                    where [dbo].[core_GetKeyUnicode]({0},{1}) < @targetNodeCode
                                                                         and
                                                                          [dbo].[core_GetKeyUnicode]({0},{1}) >= @preSourceNodeCode
                                                                  )
                                                        begin
	                                                        ;with cte as (
					                                                        select
						                                                         top(1000)
						                                                        *
					                                                        from {2}
					                                                        where [dbo].[core_GetKeyUnicode]({0},{1}) < @targetNodeCode
						                                                         and
						                                                          [dbo].[core_GetKeyUnicode]({0},{1}) >= @preSourceNodeCode
					                                                        order by sequence
				                                                          )
	                                                        delete from cte;
                                                        end;
                                                      ", HashDataMigrateHelper.GetKey(keyFields), int.MaxValue, tableName),
                        Transaction = sqlTran,
                    })
                    {
                        var parameter = new SqlParameter("@targetNodeCode", SqlDbType.BigInt)
                        {
                            Value = targetNodeCode
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@preSourceNodeCode", SqlDbType.BigInt)
                        {
                            Value = preSourceNodeCode
                        };
                        command.Parameters.Add(parameter);

                        command.Prepare();


                            await command.ExecuteReaderAsync();
                     

                    }
                });
            }
        }

        /// <summary>
        /// 从源真实节点获取要转移的数据
        /// 目标节点的前一个节点<= results < 目标节点
        /// </summary>
        /// <param name="preSourceNodeCode">目标节点的前一个节点</param>
        /// <param name="targetNodeCode">目标节点</param>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetMigrateDataFromSource(long preSourceNodeCode, long targetNodeCode, string sourceRealNodeKey, Func<Dictionary<string, TableDataInfo>, Task> callback)
        {
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(sourceRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            //遍历该节点下包含的所有需要迁移的表
            foreach (var n in nodeKey.TableNames)
            {
                var tableName = n.Value;

                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(n.Key);

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"
                                                         select
                                                              {0} as nodekey
                                                             ,[dbo].[core_GetKeyUnicode]({0},{1}) as nodecode
                                                             ,*
                                                         from {2}
                                                         where [dbo].[core_GetKeyUnicode]({0},{1}) < @targetNodeCode
                                                              and
                                                               [dbo].[core_GetKeyUnicode]({0},{1}) >= @preSourceNodeCode
                                                      ", HashDataMigrateHelper.GetKey(keyFields), int.MaxValue, tableName),
                        Transaction = sqlTran,
                    })
                    {
                        var parameter = new SqlParameter("@targetNodeCode", SqlDbType.BigInt)
                        {
                            Value = targetNodeCode
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@preSourceNodeCode", SqlDbType.BigInt)
                        {
                            Value = preSourceNodeCode
                        };
                        command.Parameters.Add(parameter);

                        command.Prepare();

                        SqlDataReader reader = null;
                  
                        using (reader = await command.ExecuteReaderAsync())
                        {
                            var result = new Dictionary<string, TableDataInfo>();
                            var tableDataInfo = new TableDataInfo();
                            tableDataInfo.EntityName = tableName;

                            List<RowDataInfo> rowDatainfoList = new List<RowDataInfo>();

                            while (await reader.ReadAsync())
                            {
                                var rowDataInfo = new RowDataInfo();
                                if (reader["nodekey"] != DBNull.Value)
                                {
                                    rowDataInfo.RowKey = reader["nodekey"].ToString();
                                }

                                if (reader["nodecode"] != DBNull.Value)
                                {
                                    rowDataInfo.RowCode = long.Parse(reader["nodecode"].ToString());
                                }

                                for (int i = 2; i <= reader.FieldCount - 1; i++)
                                {
                                    var column = reader.GetName(i);
                                    if (!HashKeyFields.GetUnMigrateFields(n.Key).Contains(column))
                                    {
                                        rowDataInfo.RowData.Add(column, reader.GetValue(i));
                                    }
                                }
                                tableDataInfo.Rows.Add(rowDataInfo);
                            }
                            result.Add(n.Key, tableDataInfo);

                            reader.Close();

                            await callback(result);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 从源真实节点获取所有数据
        /// </summary>
        /// <param name="sourceNodeCode"></param>
        /// <param name="targetNodeCode"></param>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllDataFromSource(long sourceNodeCode, long targetNodeCode, string sourceRealNodeKey, Func<Dictionary<string, TableDataInfo>, Task> callback)
        {
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(sourceRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;
            //遍历该节点下包含的所有需要迁移的表
            foreach (var n in nodeKey.TableNames)
            {
                var tableName = n.Value;

                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(n.Key);

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"
                                                         select
                                                              {0} as nodekey
                                                             ,[dbo].[core_GetKeyUnicode]({0},{1}) as nodecode
                                                             ,*
                                                         from {2}
                                                      ", HashDataMigrateHelper.GetKey(keyFields), int.MaxValue, tableName),
                        Transaction = sqlTran,
                    })
                    {
                        SqlDataReader reader = null;

                        using (reader = await command.ExecuteReaderAsync())
                        {
                            var result = new Dictionary<string, TableDataInfo>();
                            var tableDataInfo = new TableDataInfo();
                            tableDataInfo.EntityName = tableName;

                            List<RowDataInfo> rowDatainfoList = new List<RowDataInfo>();

                            while (await reader.ReadAsync())
                            {
                                var rowDataInfo = new RowDataInfo();
                                if (reader["nodekey"] != DBNull.Value)
                                {
                                    rowDataInfo.RowKey = reader["nodekey"].ToString();
                                }

                                if (reader["nodecode"] != DBNull.Value)
                                {
                                    rowDataInfo.RowCode = long.Parse(reader["nodecode"].ToString());
                                }

                                for (int i = 2; i <= reader.FieldCount - 1; i++)
                                {
                                    var column = reader.GetName(i);
                                    if (!HashKeyFields.GetUnMigrateFields(n.Key).Contains(column))
                                    {
                                        rowDataInfo.RowData.Add(column, reader.GetValue(i));
                                    }
                                }
                                tableDataInfo.Rows.Add(rowDataInfo);
                            }
                            result.Add(n.Key, tableDataInfo);

                            reader.Close();

                            await callback(result);
                        }
                    }
                });
            }
        }

    }

    /// <summary>
    /// 针对存储类型为SqlServre、策略为哈希环的数据整体迁移的数据操作服务
    /// </summary>
    public interface IHashDataTotalMigrateSqlServerRingDataService
    {
        /// <summary>
        /// 从源数据获取所有数据
        /// </summary>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        Task<Dictionary<string, TableDataInfo>> GetAllDataFromSource(string sourceRealNodeKey, int topNum);

        /// <summary>
        /// 将要转移的数据增加到目标真实节点中
        /// </summary>
        /// <param name="targetRealNodeKey"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task AddMigrateDataToTarget(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas);
        /// <summary>
        /// 将要转移的数据从源真实节点中删除
        /// </summary>
        /// <param name="targetRealNodeKey"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task DeleteMigrateDataFromSource(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas);
    }


    /// <summary>
    /// 针对存储类型为SqlServre、策略为哈希环的数据整体迁移的数据操作服务
    /// </summary>
    [Injection(InterfaceType = typeof(IHashDataTotalMigrateSqlServerRingDataService), Scope = InjectionScope.Singleton)]
    public class HashDataTotalMigrateSqlServerRingDataService : IHashDataTotalMigrateSqlServerRingDataService
    {
        /// <summary>
        /// 将要转移的数据增加到目标真实节点中
        /// </summary>
        /// <param name="targetRealNodeKey"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public async Task AddMigrateDataToTarget(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas)
        {
            //解析目标服务器信息
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(targetRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            //遍历需要写入的数据集合
            foreach (var data in datas)
            {
                if (!nodeKey.TableNames.TryGetValue(data.Key, out string tableName))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundHashDataMigrateTargetDB,
                        DefaultFormatting = "将要转移的数据增加到目标真实节点时发生错误，找不到目标数据库节点：{0}",
                        ReplaceParameters = new List<object>() { data.Key }
                    };


                    throw new UtilityException((int)Errors.NotFoundHashDataMigrateTargetDB,
                       fragment);
                }


                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(data.Key);

                DataTable dt = new DataTable();

                //遍历每一条数据在DB中是否存在，若不存在则写入
                foreach (RowDataInfo row in data.Value.Rows)
                {
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                    {
                        SqlTransaction sqlTran = null;
                        if (transaction != null)
                        {
                            sqlTran = (SqlTransaction)transaction;
                        }
                        using (SqlCommand command = new SqlCommand()
                        {
                            Connection = (SqlConnection)conn,
                            CommandType = CommandType.Text,
                            CommandText = string.Format(@"
                                                          select top 1 *
                                                          from {0}
                                                          where {1}
                                                      ", tableName, HashDataMigrateHelper.GetConditions(keyFields, row.RowData)),
                            Transaction = sqlTran,
                        })
                        {
                            command.Prepare();

                            SqlDataReader reader = null;

                            using (reader = await command.ExecuteReaderAsync())
                            {
                                //DB中不存在的数据需要迁移到目标DB中
                                if (!await reader.ReadAsync())
                                {
                                    HashDataMigrateHelper.GetDataTable(ref dt, row.RowData, HashKeyFields.GetUnMigrateFields(data.Key));
                                }
                                reader.Close();
                            }
                        }
                    });
                }

                //批量插入
                using (SqlConnection destConnection = new SqlConnection(connAll))
                {
                    destConnection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destConnection))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        await bulkCopy.WriteToServerAsync(dt);
                    }
                }

            }
        }

        /// <summary>
        /// 将要转移的数据从源真实节点中删除
        /// </summary>
        /// <param name="targetRealNodeKey"></param>
        /// <param name="datas"></param>
        public async Task DeleteMigrateDataFromSource(string targetRealNodeKey, Dictionary<string, TableDataInfo> datas)
        {
            //解析目标服务器信息
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(targetRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            //遍历需要删除的数据集合
            foreach (var data in datas)
            {
                if (!nodeKey.TableNames.TryGetValue(data.Key, out string tableName))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundDeleteTargetDB,
                        DefaultFormatting = "删除目标节点数据时发生错误，找不到目标数据库节点：{0}",
                        ReplaceParameters = new List<object>() { data.Key }
                    };

                    throw new UtilityException((int)Errors.NotFoundDeleteTargetDB,
                        fragment);
                }


                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(data.Key);

                DataTable dt = new DataTable();

                //遍历每一条数据在DB中是否存在，若不存在则写入
                foreach (RowDataInfo row in data.Value.Rows)
                {
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                    {
                        SqlTransaction sqlTran = null;
                        if (transaction != null)
                        {
                            sqlTran = (SqlTransaction)transaction;
                        }
                        using (SqlCommand command = new SqlCommand()
                        {
                            Connection = (SqlConnection)conn,
                            CommandType = CommandType.Text,
                            CommandText = string.Format(@"
                                                           delete from {0}
                                                           where {1}
                                                      ", tableName, HashDataMigrateHelper.GetConditions(keyFields, row.RowData)),
                            Transaction = sqlTran,
                        })
                        {
                            command.Prepare();

                            await command.ExecuteNonQueryAsync();

                        }
                    });
                }
            }
        }

        /// <summary>
        /// 从源数据获取所有数据
        /// </summary>
        /// <param name="sourceRealNodeKey"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, TableDataInfo>> GetAllDataFromSource(string sourceRealNodeKey, int topNum)
        {
            var nodeKey = JsonSerializerHelper.Deserialize<StoreInfo>(sourceRealNodeKey);
            var connAll = nodeKey.DBConnectionNames.ReadAndWrite;

            var result = new Dictionary<string, TableDataInfo>();

            //遍历该节点下包含的所有需要迁移的表
            foreach (var n in nodeKey.TableNames)
            {
                var tableName = n.Value;

                //取得参与Hash计算的key fields
                var keyFields = HashKeyFields.GetKeyFields(n.Key);

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, connAll, async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"
                                                         select top(@topNum)
                                                              {0} as nodekey
                                                             ,[dbo].[core_GetKeyUnicode]({0},{1}) as nodecode
                                                             ,*
                                                         from {2}
                                                      ", HashDataMigrateHelper.GetKey(keyFields), int.MaxValue, tableName),
                        Transaction = sqlTran,
                    })
                    {

                        var parameter = new SqlParameter("@topNum", SqlDbType.Int)
                        {
                            Value = topNum
                        };
                        command.Parameters.Add(parameter);

                        command.Prepare();

                        SqlDataReader reader = null;

                        using (reader = await command.ExecuteReaderAsync())
                        {
                            var tableDataInfo = new TableDataInfo();
                            tableDataInfo.EntityName = tableName;

                            List<RowDataInfo> rowDatainfoList = new List<RowDataInfo>();

                            while (await reader.ReadAsync())
                            {
                                var rowDataInfo = new RowDataInfo();
                                if (reader["nodekey"] != DBNull.Value)
                                {
                                    rowDataInfo.RowKey = reader["nodekey"].ToString();
                                }

                                if (reader["nodecode"] != DBNull.Value)
                                {
                                    rowDataInfo.RowCode = long.Parse(reader["nodecode"].ToString());
                                }

                                for (int i = 2; i <= reader.FieldCount - 1; i++)
                                {
                                    var column = reader.GetName(i);
                                    if (!HashKeyFields.GetUnMigrateFields(n.Key).Contains(column))
                                    {
                                        rowDataInfo.RowData.Add(column, reader.GetValue(i));
                                    }
                                }
                                tableDataInfo.Rows.Add(rowDataInfo);
                            }
                            result.Add(n.Key, tableDataInfo);

                            reader.Close();
                        }
                    }
                });
            }

            return result;
        }
    }

    /// <summary>
    /// 记录实体参与Hash计算的字段
    /// </summary>
    public class HashKeyFields
    {
        /// <summary>
        /// 保存所有参与Hash计算的字段
        /// tableName,keyField
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _keyFields = new Dictionary<string, Dictionary<string, string>>();


        /// <summary>
        /// 所有不需要迁移的字段
        /// </summary>
        private static Dictionary<string, Dictionary<string, string>> _unMigrateFields = new Dictionary<string, Dictionary<string, string>>();


        /// <summary>
        /// 保存参与Hash计算的字段
        /// </summary>
        /// <param name="entityName">表名</param>
        /// <param name="fields">参与计算的字段</param>
        public static void SetKeyFields(string entityName, params string[] fields)
        {

            _keyFields.Add(entityName, new Dictionary<string, string>());

            foreach (var field in fields)
            {
                _keyFields[entityName].Add(field, "");
            }
        }

        /// <summary>
        /// 取得所有参与Hash计算的字段
        /// </summary>
        /// <param name="entityName">表名</param>
        /// <returns>所有参与计算的字段</returns>
        public static List<string> GetKeyFields(string entityName)
        {
            List<string> result = new List<string>();
            if (_keyFields.TryGetValue(entityName, out Dictionary<string, string> fields))
            {
                foreach (var k in fields.Keys)
                {
                    result.Add(k);
                }
            }
            return result;
        }

        /// <summary>
        /// 保存所有不需要迁移的字段
        /// </summary>
        /// <param name="entityName">表名</param>
        /// <param name="fields">参与计算的字段</param>
        public static void SetUnMigrateFields(string entityName, params string[] fields)
        {
            _unMigrateFields.Add(entityName, new Dictionary<string, string>());

            foreach (var field in fields)
            {
                _unMigrateFields[entityName].Add(field, "");
            }
        }

        /// <summary>
        /// 取得所有不需要迁移的字段
        /// </summary>
        /// <param name="entityName">表名</param>
        /// <returns>所有参与计算的字段</returns>
        public static List<string> GetUnMigrateFields(string entityName)
        {
            List<string> result = new List<string>();
            if (_unMigrateFields.TryGetValue(entityName, out Dictionary<string, string> fields))
            {
                foreach (var k in fields.Keys)
                {
                    result.Add(k);
                }
            }
            return result;
        }
    }
    internal static class HashDataMigrateHelper
    {

        /// <summary>
        /// 将多个字段拼接成一个参数
        /// </summary>
        /// <param name="keysFields">参与计算的字段</param>
        /// <param name="RowKey">拼接后的key</param>
        /// <returns></returns>
        public static string GetKey(List<string> keysFields)
        {
            string result = "";
            string key = "cast({0} as nvarchar(400))";
            //cast(id as nvarchar(400)) + '.' + cast(id2 as nvarchar(400)

            for (int i = 0, len = keysFields.Count; i < len; i++)
            {
                result += string.Format(key, keysFields[i]);
                if (i != len - 1)
                {
                    result += "+'.'+";
                }
            }
            return result;
        }


        /// <summary>
        /// 通过KeyFields拼接查询条件
        /// </summary>
        /// <param name="keyFields"></param>
        /// <param name="RowData">colunm,value</param>
        /// <returns></returns>
        public static string GetConditions(List<string> keyFields, Dictionary<string, object> rowData)
        {
            string conditions = "";

            for (int i = 0; i < keyFields.Count; i++)
            {
                var field = keyFields[i];

                if (rowData.TryGetValue(field, out object value))
                {
                    conditions += field + " = '" + GetValueFromDbObject(value) + "'";

                    if (i < keyFields.Count - 1)
                    {
                        conditions += " and ";
                    }
                }
            }
            return conditions;
        }

        private static string GetValueFromDbObject(object dbObject)
        {
            string value = "";
            switch (dbObject.GetType().Name)
            {
                case "String":
                    value = dbObject.ToString();
                    break;
                case "Guid":
                    value = ((Guid)dbObject).ToString();
                    break;
                case "Int":
                    value = ((int)dbObject).ToString();
                    break;
                default:
                    break;
            }

            return value;
        }

        public static void GetDataTable(ref DataTable dt, Dictionary<string, object> rowData, List<string> unMigrateFields)
        {
            //去掉所有不需要迁移的字段
            foreach (var u in unMigrateFields)
            {
                rowData.Remove(u);
            }

            //添加column name
            if (dt.Columns.Count == 0)
            {
                foreach (var r in rowData)
                {
                    dt.Columns.Add(r.Key, r.Value.GetType());
                }
            }

            var newRow = dt.NewRow();

            //添加列值
            foreach (var k in rowData.Keys)
            {
                newRow[k] = rowData[k];
            }
            dt.Rows.Add(newRow);

        }

    }

}
