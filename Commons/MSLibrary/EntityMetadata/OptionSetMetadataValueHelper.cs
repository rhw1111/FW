using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.EntityMetadata
{
    /// <summary>
    /// 选项集元数据帮助器
    /// </summary>
    public static class OptionSetMetadataValueHelper
    {
        /// <summary>
        /// 获取指定选项集指定值的标签
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<string> GetLable(IOptionSetValueMetadataRepository repository,string name,int value)
        {
            var metadata=await repository.QueryByName(name);
            if (metadata==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }

            var item=await metadata.GetItem(value);
            if (item==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, value }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            return await item.GetCurrentLabel();
        }


        /// <summary>
        /// 获取指定选项集指定值的标签
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="name"></param>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static async Task<string> GetLable(IOptionSetValueMetadataRepository repository, string name, string stringValue)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName,fragment);
            }

            var item = await metadata.GetItem(stringValue);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, stringValue }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            return await item.GetCurrentLabel();
        }


        public static async Task<IDictionary<int?, string>> GetLables(IOptionSetValueMetadataRepository repository, string name)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }
            IDictionary<int?, string> dict = new Dictionary<int?, string>();
            await metadata.GetAllItem(async item=> 
            {
                if (dict.ContainsKey(item.Value))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { name }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata, fragment);
                }
                else
                {
                    dict[item.Value] = await item.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });
            if (dict.Count<=0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到选项集项",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }
            return dict;
        }



        public static async Task<IDictionary<string, string>> GetStringLables(IOptionSetValueMetadataRepository repository, string name)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }
            IDictionary<string, string> dict = new Dictionary<string, string>();
            await metadata.GetAllItem(async item =>
            {
                if (dict.ContainsKey(item.StringValue))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { name }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata, fragment);
                }
                else
                {
                    dict[item.StringValue] = await item.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });
            if (dict.Count <= 0)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到选项集项",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }
            return dict;
        }



        public static async Task<IDictionary<string, string>> GetStringChildLables(IOptionSetValueMetadataRepository repository, string name,string childName,int value)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }

            var childMetadata = await repository.QueryByName(childName);
            if (childMetadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { childName }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }


            var item = await metadata.GetItem(value);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, value }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            IDictionary<string, string> dict = new Dictionary<string, string>();
            await metadata.GetAllChildItem(childMetadata.ID,item.ID, async childItem =>
            {
                if (dict.ContainsKey(childItem.StringValue))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { childName }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata,fragment);
                }
                else
                {
                    dict[childItem.StringValue] = await childItem.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });

            return dict;
        }

        public static async Task<IDictionary<string, string>> GetStringChildLables(IOptionSetValueMetadataRepository repository, string name, string childName, string value)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }

            var childMetadata = await repository.QueryByName(childName);
            if (childMetadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { childName }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }


            var item = await metadata.GetItem(value);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, value }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            IDictionary<string, string> dict = new Dictionary<string, string>();
            await metadata.GetAllChildItem(childMetadata.ID, item.ID, async childItem =>
            {
                if (dict.ContainsKey(childItem.StringValue))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { childName }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata, fragment);
                }
                else
                {
                    dict[childItem.StringValue] = await childItem.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });

            return dict;
        }





        public static async Task<IDictionary<int?, string>> GetChildLables(IOptionSetValueMetadataRepository repository, string name, string childName, int value)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName,fragment);
            }

            var childMetadata = await repository.QueryByName(childName);
            if (childMetadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { childName }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }


            var item = await metadata.GetItem(value);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, value }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            IDictionary<int?, string> dict = new Dictionary<int?, string>();
            await metadata.GetAllChildItem(childMetadata.ID, item.ID, async childItem =>
            {
                if (dict.ContainsKey(childItem.Value))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { childName }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata, fragment);
                }
                else
                {
                    dict[childItem.Value] = await childItem.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });

            return dict;
        }


        public static async Task<IDictionary<int?, string>> GetChildLables(IOptionSetValueMetadataRepository repository, string name, string childName, string value)
        {
            var metadata = await repository.QueryByName(name);
            if (metadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }

            var childMetadata = await repository.QueryByName(childName);
            if (childMetadata == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueMetadataByName,
                    DefaultFormatting = "找不到名称为{0}的选项集元数据",
                    ReplaceParameters = new List<object>() { childName }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueMetadataByName, fragment);
            }


            var item = await metadata.GetItem(value);
            if (item == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundOptionSetValueItemInMetadataByValue,
                    DefaultFormatting = "在名称为{0}的选项集元数据下找不到值为{1}的选项集项",
                    ReplaceParameters = new List<object>() { name, value }
                };

                throw new UtilityException((int)Errors.NotFoundOptionSetValueItemInMetadataByValue, fragment);
            }

            IDictionary<int?, string> dict = new Dictionary<int?, string>();
            await metadata.GetAllChildItem(childMetadata.ID, item.ID, async childItem =>
            {
                if (dict.ContainsKey(childItem.Value))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.OptionSetValueItemIsExistInMetadata,
                        DefaultFormatting = "指定名称{0}的选项集元数据中存在相同值的选项",
                        ReplaceParameters = new List<object>() { childName }
                    };

                    throw new UtilityException((int)Errors.OptionSetValueItemIsExistInMetadata, fragment);
                }
                else
                {
                    dict[childItem.Value] = await childItem.GetCurrentLabel();
                }

                await Task.FromResult(0);
            });

            return dict;
        }

    }
}
