﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppAddTestCase), Scope = InjectionScope.Singleton)]
    public class AppAddTestCase : IAppAddTestCase
    {
        public async Task<TestCaseViewData> Do(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            try
            {
                TestCase source = new TestCase()
                {
                    Name = model.Name,
                    OwnerID = model.OwnerID,
                    EngineType = model.EngineType,
                    MasterHostID = model.MasterHostID,
                    Configuration = model.Configuration,
                    Status = model.Status
                };               
                await source.Add();

                result = new TestCaseViewData()
                {
                    ID = source.ID,
                    EngineType = source.EngineType,
                    Configuration = source.Configuration,
                    Name = source.Name,
                    CreateTime = source.CreateTime.ToCurrentUserTimeZone(),
                    ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            

            return result;
        }

        public async Task<TestCaseViewData> Update(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCaseViewData result;
            try
            {
                TestCase source = new TestCase()
                {
                    ID = model.ID,
                    Name = model.Name,
                    OwnerID = model.OwnerID,
                    EngineType = model.EngineType,
                    MasterHostID = model.MasterHostID,
                    Configuration = model.Configuration,
                    Status = model.Status
                };
                await source.Update(cancellationToken);

                result = new TestCaseViewData()
                {
                    EngineType = source.EngineType,
                    Configuration = source.Configuration,
                    Name = source.Name,
                    ModifyTime = source.ModifyTime.ToCurrentUserTimeZone()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return result;
        }

        public async Task<TestCaseViewData> Delete(TestCaseAddModel model, CancellationToken cancellationToken = default)
        {
            TestCase source = new TestCase()
            {
                ID = model.ID
            };
            await source.Delete(cancellationToken);
            TestCaseViewData viewData = new TestCaseViewData
            {
                ID = model.ID
            };
            return viewData;
        }
    }
}
