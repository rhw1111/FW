using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.RemoteService
{
    public class BasicHttpBindingService<T>
    {
        private static Dictionary<string, ChannelFactory<T>> _serviceFactories=new Dictionary<string, ChannelFactory<T>>();
        private static object _lockObj = new object();
        public void Execute(string serviceUrl,Action<T> callback)
        {
            ChannelFactory<T> channelFactory;

            if (!_serviceFactories.TryGetValue(serviceUrl, out channelFactory))
            {
                lock(_lockObj)
                {
                    if (!_serviceFactories.TryGetValue(serviceUrl, out channelFactory))
                    {
                        BasicHttpBinding binding;
                        Uri address = new Uri(serviceUrl);
                        if (address.Scheme=="https")
                        {
                            binding = new BasicHttpBinding( BasicHttpSecurityMode.Transport);
                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                        }
                        else
                        {
                            binding = new BasicHttpBinding( BasicHttpSecurityMode.None);
                        }

                        binding.CloseTimeout = new TimeSpan(0,10,0);
                        binding.OpenTimeout= new TimeSpan(0, 10, 0);
                        binding.ReceiveTimeout= new TimeSpan(0, 10, 0); ;
                        binding.SendTimeout= new TimeSpan(0, 10, 0); ;
                        binding.MaxReceivedMessageSize = 65536000;

                        EndpointAddress addr = new EndpointAddress(serviceUrl);
                        channelFactory = new ChannelFactory<T>(binding, addr);
                        _serviceFactories.Add(serviceUrl, channelFactory);
                    }
                    
                }
            }

            var channel=channelFactory.CreateChannel();

            try
            {
                callback(channel);
                ((ICommunicationObject)channel).Close();
            }
            catch(Exception ex)
            {
                try
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch
                {
                    
                }

                var fragment = new TextFragment()
                {
                    Code = TextCodes.CommunicationServiceError,
                    DefaultFormatting = "与服务{0}通信时出现错误，错误内容为{1}",
                    ReplaceParameters = new List<object>() { serviceUrl, string.Format("messgae:{0},stack:{1}", ex.Message, ex.StackTrace) }
                };


                throw new UtilityException((int)Errors.CommunicationServiceError, fragment);
            }

        }

        public async Task ExecuteAsync(string serviceUrl, Func<T, Task> callback)
        {
            ChannelFactory<T> channelFactory;

            if (!_serviceFactories.TryGetValue(serviceUrl, out channelFactory))
            {
                lock (_lockObj)
                {
                    if (!_serviceFactories.TryGetValue(serviceUrl, out channelFactory))
                    {
                        BasicHttpBinding binding;
                        Uri address = new Uri(serviceUrl);
                        if (address.Scheme == "https")
                        {
                            binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                        }
                        else
                        {
                            binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                        }

                        binding.CloseTimeout = new TimeSpan(0, 10, 0);
                        binding.OpenTimeout = new TimeSpan(0, 10, 0);
                        binding.ReceiveTimeout = new TimeSpan(0, 10, 0); ;
                        binding.SendTimeout = new TimeSpan(0, 10, 0); ;
                        binding.MaxReceivedMessageSize = 65536000;

                        EndpointAddress addr = new EndpointAddress(serviceUrl);
                        channelFactory = new ChannelFactory<T>(binding, addr);
                        _serviceFactories.Add(serviceUrl, channelFactory);
                    }

                }
            }

            var channel = channelFactory.CreateChannel();

            try
            {
                await callback(channel);
                ((ICommunicationObject)channel).Close();
            }
            catch (Exception ex)
            {
                try
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch
                {

                }
                throw ex;
            }
        }
    }
}
