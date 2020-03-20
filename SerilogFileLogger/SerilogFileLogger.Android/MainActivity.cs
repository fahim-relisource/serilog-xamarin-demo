using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Serilog;
using Serilog.Core;
using SerilogFileLogger.Droid.Services;
using System.IO;
using Environment = System.Environment;

namespace SerilogFileLogger.Droid
{

    [Activity(Label = "SerilogFileLogger", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Logger OurLogger { get; private set; }
        public static MainActivity Instance;
        public const string APP_CHANNEL = "com.companyname.serilogfilelogger.android";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Instance = this;

            OurLogger = new LoggerConfiguration()
                .WriteTo.File(
                    path: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XamarinLib-{Date}.log"),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
                    fileSizeLimitBytes: 100000000,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31,
                    encoding: System.Text.Encoding.UTF8
                )
                .WriteTo.AndroidLog()
                .CreateLogger();

            OurLogger.Information("On MainActivity OnCreate");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public static void NotificationBuildAndShow(Context context, Intent intent)
        {
            var dataBundle = intent.GetBundleExtra("NotificationData");
            var message = dataBundle.GetString("Message") ?? "My Message";
            var title = dataBundle.GetString("Title") ?? "My Title";
            var workNo = dataBundle.GetInt("WORK_NO", 0);
            var logger = new OurLoggerService();

            logger.LogInformation($"Building and Showing Notification for Work {workNo}");

            var resultIntent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

            var importance = NotificationImportance.High;
            NotificationChannel notificationChannel = new NotificationChannel(APP_CHANNEL, "Important", importance);
            notificationChannel.EnableVibration(true);
            notificationChannel.LockscreenVisibility = NotificationVisibility.Public;

            var audioAttributes = new AudioAttributes.Builder()
                .SetContentType(AudioContentType.Sonification)
                .SetUsage(AudioUsageKind.Alarm)
                .Build();

            notificationChannel.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone), audioAttributes);

            var notificationBuilder = new NotificationCompat.Builder(context, APP_CHANNEL)
                .SetSmallIcon(Resource.Mipmap.icon_round)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(false)
                .SetChannelId(APP_CHANNEL);
            ;

            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(notificationChannel);
            notificationManager.Notify(6461, notificationBuilder.Build());
            logger.LogInformation($"Showing Notification for Work {workNo} Complete");
        }
    }
}