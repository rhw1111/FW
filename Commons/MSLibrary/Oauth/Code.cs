using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Oauth
{
    /// <summary>
    /// Oauth中的Code
    /// </summary>
    public class Code:EntityBase<ICodeIMP>
    {
        private static IFactory<ICodeIMP> _codeIMPFactory;

        public static IFactory<ICodeIMP> CodeIMPFactory
        {
            set
            {
                _codeIMPFactory = value;
            }
        }

        public override IFactory<ICodeIMP> GetIMPFactory()
        {
            return _codeIMPFactory;
        }

        public Code():base()
        {
        }


        public Guid Id
        {
            get
            {
                return GetAttribute<Guid>("Id");
            }
            set
            {
                SetAttribute<Guid>("Id", value);
            }
        }

        public string AccessToken
        {
            get
            {
                return GetAttribute<string>("AccessToken");
            }
            set
            {
                SetAttribute<string>("AccessToken", value);
            }
        }

        public string RefreashToken
        {
            get
            {
                return GetAttribute<string>("RefreashToken");
            }
            set
            {
                SetAttribute<string>("RefreashToken", value);
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }


    }


    public interface ICodeIMP
    {
        Task Add(Code code);
        Task Delete(Code code);
    }
}
