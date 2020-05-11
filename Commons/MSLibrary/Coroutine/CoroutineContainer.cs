using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.Coroutine
{
    /// <summary>
    ///  协程容器
    /// </summary>
    public class CoroutineContainer : EntityBase<ICoroutineContainerIMP>
    {
        private static IFactory<ICoroutineContainerIMP> _coroutineContainerIMPFactory;

        public static IFactory<ICoroutineContainerIMP> CoroutineContainerIMPFactory
        {
            set
            {
                _coroutineContainerIMPFactory = value;
            }
        }

        public override IFactory<ICoroutineContainerIMP> GetIMPFactory()
        {
            return _coroutineContainerIMPFactory;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }

        /// <summary>
        /// 向容器加入动作
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task ApplyAction(Func<IEnumerator<Task>> action)
        {
            await _imp.ApplyAction(this,action);
        }

        /// <summary>
        /// 运行容器
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            await _imp.Run(this);
        }
    }

    public interface ICoroutineContainerIMP
    {
        /// <summary>
        /// 向容器加入动作
        /// </summary>
        /// <param name="container"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task ApplyAction(CoroutineContainer container,Func<IEnumerator<Task>> action);
        /// <summary>
        /// 运行容器
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        Task Run(CoroutineContainer container);
    }

    [Injection(InterfaceType = typeof(ICoroutineContainerIMP), Scope = InjectionScope.Transient)]
    public class CoroutineContainerIMP : ICoroutineContainerIMP
    {
        private static Dictionary<string, IFactory<ICoroutineService>> _coroutineServiceFactories = new Dictionary<string, IFactory<ICoroutineService>>();

        public static Dictionary<string, IFactory<ICoroutineService>> CoroutineServiceFactories
        {
            get
            {
                return _coroutineServiceFactories;
            }
        }

        private ICoroutineService _service;

        public async Task ApplyAction(CoroutineContainer container, Func<IEnumerator<Task>> action)
        {
            var service=GetService(container.Name);
            await service.ApplyAction(action);
        }

        public async Task Run(CoroutineContainer container)
        {
            var service = GetService(container.Name);
            await service.Run();
        }


        private ICoroutineService GetService(string name)
        {
            if (_service==null)
            {
                if (!_coroutineServiceFactories.TryGetValue(name, out IFactory<ICoroutineService> serviceFactory))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundCoroutineContainerByName,
                        DefaultFormatting = "找不到名称为{0}的协程容器",
                        ReplaceParameters = new List<object>() { name }
                    };

                    throw new UtilityException((int)Errors.NotFoundCoroutineContainerByName, fragment);
                }

                _service=serviceFactory.Create();
            }
            return _service;
        }
    }
}
