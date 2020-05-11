using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Cache
{
    public static class CacheInnerHelper
    {
        public static KVCacheVisitor CreateKVCacheVisitor(KVCacheVisitorSetting setting)
        {
            KVCacheVisitor visitor;
            switch (setting.KVCacheType)
            {
                case KVCacheTypes.LocalTimeout:
                    visitor = new KVCacheVisitor()
                    {
                        Name = setting.Name,
                        CacheType = KVCacheTypes.LocalTimeout,
                        CacheConfiguration = string.Format(@"{{
                        ""MaxLength"":{0},
                        ""ExpireSeconds"":{1}
                }}", setting.MaxLength.ToString(), setting.ExpireSeconds.ToString())

                    };
                    break;
                default:
                    visitor = new KVCacheVisitor()
                    {
                        Name = setting.Name,
                        CacheType = KVCacheTypes.LocalVersion,
                        CacheConfiguration = string.Format(@"{{
                        ""MaxLength"":{0},
                        ""VersionCallTimeout"":{1},
                        ""VersionNameMappings"":{{}},
                        ""DefaultVersionName"":{2}
                }}", setting.MaxLength.ToString(), setting.VersionCallTimeout.ToString(), setting.VersionName)

                    };
                    break;
            }

            return visitor;
        }
    }

    public class KVCacheVisitorSetting
    {
        public string Name { get; set; }
        public  string KVCacheType { get; set; } = KVCacheTypes.LocalTimeout;
        public  int MaxLength { get; set; } = 500;
        public  int ExpireSeconds { get; set; } = 600;
        public  int VersionCallTimeout { get; set; }
        public  string VersionName { get; set; }
    }
}
