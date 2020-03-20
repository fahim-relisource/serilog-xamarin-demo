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

			AlarmManager alarmManager = MainActivity.Instance.GetSystemService(Context.AlarmService) as AlarmManager;
			List<PendingIntent> intentArray = new List<PendingIntent>();

			Bundle notificationDataBundle = new Bundle();
			notificationDataBundle.PutString("Title", "Serilog File Notification");
			notificationDataBundle.PutString("Message", $"Showing serilog file notification for task {workNo}");
			notificationDataBundle.PutInt("WORK_NO", workNo);

			Intent notificationIntent = new Intent(MainActivity.Instance, typeof(NotificationAlertReceiver));
			notificationIntent.PutExtra("NotificationData", notificationDataBundle);
			var random = new Random();
			var randomSeconds = random.Next(20, 60);

			int alarmRequestCode = workNo * 10 + workNo;
			PendingIntent alarmIntent = PendingIntent.GetBroadcast(
					MainActivity.Instance,
					alarmRequestCode,
					notificationIntent,
					PendingIntentFlags.Immutable
				);

			TimeSpan myTimeSpan = new TimeSpan(0, 0, randomSeconds);
			DateTime scheduleTime = DateTime.Now + myTimeSpan;
			logger.LogInformation($"Setting Alarm for {workNo} at {scheduleTime}");

			long alarmTimeEpoch = new DateTimeOffset(scheduleTime).ToUnixTimeMilliseconds();
			long currentTimeEpoch = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();

			if (alarmTimeEpoch > currentTimeEpoch)
			{
				alarmManager.SetAlarmClock(
					new AlarmManager.AlarmClockInfo(
						alarmTimeEpoch,
						alarmIntent
					),
					alarmIntent
				);

				intentArray.Add(alarmIntent);
			}

			logger.LogInformation($"Work NO: {workNo} set up complete");

			return Result.InvokeSuccess();
		}
	}
}