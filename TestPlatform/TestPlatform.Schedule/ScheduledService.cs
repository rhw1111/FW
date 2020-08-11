using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MSLibrary.Logger;
using MSLibrary.Configuration;
using MSLibrary.Schedule.Application;
using FW.TestPlatform.Main;
using FW.TestPlatform.Main.Configuration;

namespace FW.TestPlatform.Schedule
{
    public class ScheduledService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IAppHostContextInitGenerate _appHostContextInitGenerate;
        private readonly IAppGetScheduleController _appGetScheduleController;

        private IScheduleController _scheduleController;

        public ScheduledService(IHostApplicationLifetime appLifetime, IAppHostContextInitGenerate appHostContextInitGenerate, IAppGetScheduleController appGetScheduleController)
        {
            _appLifetime = appLifetime;
            _appHostContextInitGenerate = appHostContextInitGenerate;
            _appGetScheduleController = appGetScheduleController;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LoggerHelper.LogInformation("Test", $"Start");


            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);


            //var init = await _appHostContextInitGenerate.Do(cancellationToken);
            //init.Init();

            //_scheduleController = await _appGetScheduleController.Do(cancellationToken);

            PhysicalFileProvider fileProvider= new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "F")); ;
            var fileChangeToken = fileProvider.Watch("*.*");
            fileChangeToken.RegisterChangeCallback((f)=>
            {
                LoggerHelper.LogInformation("Test", $"Fire");

            }, default);

            /*var watcher = new FileSystemWatcher();
            watcher.Path = "F";
            watcher.NotifyFilter = NotifyFilters.Attributes |
                                   NotifyFilters.CreationTime |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName |
                                   NotifyFilters.Size;
            watcher.Filter = "*.cap";

            watcher.Created += new FileSystemEventHandler((o, e) =>
            {
                FileInfo fileInfo = new FileInfo(e.FullPath);

                long old_length;
                do
                {
                    old_length = fileInfo.Length;
                    Thread.Sleep(3000);

                } while (old_length != fileInfo.Length);

                LoggerHelper.LogInformation("Test",$"{fileInfo.FullName}");
                //completedAction(new FileDataInfo() { FileName = fileInfo.FullName, CreateTime = fileInfo.CreationTimeUtc });
            });

            watcher.EnableRaisingEvents = true;*/


        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            /*if (_scheduleController != null)
            {
                await _scheduleController.Stop();
            }*/
        }

        private void OnStarted()
        {
            //var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            //LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"{nameof(ScheduledService)} OnStarted has been called.");


            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            //var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            //LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"{nameof(ScheduledService)} OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            //var applicationConfiguration = ConfigurationContainer.Get<ApplicationConfiguration>(ConfigurationNames.Application);
            //LoggerHelper.LogInformation($"{applicationConfiguration.ApplicationName}", $"{nameof(ScheduledService)} OnStopped has been called.");

            // Perform post-stopped activities here
        }

    }
}
