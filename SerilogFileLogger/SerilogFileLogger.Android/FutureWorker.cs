using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Work;
using SerilogFileLogger.Droid.Services;

namespace SerilogFileLogger.Droid
{
	public class FutureWorker : Worker
	{
		public FutureWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
		{

		}
		public override Result DoWork()
		{
			var logger = new OurLoggerService();
			var workNo = InputData.GetInt("WORK_NO", 0);
			logger.LogInformation($"Work NO: {workNo} Started");
			logger.LogInformation($"Doing some important stuff on work no {workNo}");
			try
			{
				Thread.Sleep(5000);
			}catch(ThreadInterruptedException e)
			{
				logger.LogInformation(e.StackTrace);
			}
			logger.LogInformation($"Work NO: {workNo} Completed");

			return Result.InvokeSuccess();
		}
	}
}