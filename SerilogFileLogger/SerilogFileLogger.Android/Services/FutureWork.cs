using System;
using AndroidX.Work;
using Java.Util.Concurrent;
using SerilogFileLogger.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SerilogFileLogger.Droid.Services.FutureWork))]
namespace SerilogFileLogger.Droid.Services
{
	public class FutureWork : IFutureWork
	{
		public bool SetNewFutureWork(int workId)
		{
			var logger = new OurLoggerService();
			logger.LogInformation($"Setting new service worker for ${workId} started");

			var workData = new Data.Builder();
			workData.PutInt("WORK_NO", workId);

			Random random = new Random();
			
			OneTimeWorkRequest someWork = new OneTimeWorkRequest
				.Builder(typeof(FutureWorker))
				.SetInputData(workData.Build())
				.SetInitialDelay(random.Next(1, 30), timeUnit: TimeUnit.Seconds)
				.Build();

			WorkManager.Instance.Enqueue(someWork);

			logger.LogInformation($"Setting new service worker for ${workId} complete");
			return true;
		}
	}
}