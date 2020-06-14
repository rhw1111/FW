using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities.DAL;

namespace FW.TestPlatform.Main.Entities
{
    public class User : EntityBase<IUserIMP>
    {
        public override IFactory<IUserIMP>? GetIMPFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }
    }

    public interface IUserIMP
    {
        Task Add(User user, CancellationToken cancellationToken = default);
        Task Update(User user, CancellationToken cancellationToken = default);
        Task Delete(User user, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(IUserIMP), Scope = InjectionScope.Transient)]
    public class UserIMP : IUserIMP
    {
        private IUserStore _userStore;

        public UserIMP(IUserStore userStore)
        {
            _userStore = userStore;
        }

        public async Task Add(User user, CancellationToken cancellationToken = default)
        {
            await _userStore.Add(user, cancellationToken);
        }

        public async Task Delete(User user, CancellationToken cancellationToken = default)
        {
            await _userStore.Delete(user.ID, cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken = default)
        {
            await _userStore.Update(user, cancellationToken);
        }
    }

}
