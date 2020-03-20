using Android.Content;
using SerilogFileLogger.Droid.Services;

namespace SerilogFileLogger.Droid
{
	[BroadcastReceiver]
	public class NotificationAlertReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			var logger = new OurLoggerService();
			var dataBundle = intent.GetBundleExtra("NotificationData");
			var workNo = dataBundle.GetInt("WORK_NO", 0);
			logger.LogInformation($"Notification Recived for Work NO: {workNo}");
			MainActivity.NotificationBuildAndShow(context, intent);
		}

	}
}