using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Foundation;
using Serilog;
using Serilog.Core;
using UIKit;

namespace SerilogFileLogger.iOS
{
	public class Application
	{
		public static Logger OurLogger
		{
			get; private set;
		}
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			OurLogger = new LoggerConfiguration()
			 .WriteTo.File(
				 path: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "iOSLib-{Date}.txt"),
				 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
				 fileSizeLimitBytes: 100000000,
				 rollingInterval: RollingInterval.Day,
				 rollOnFileSizeLimit: true,
				 retainedFileCountLimit: 31,
				 encoding: Encoding.UTF8
			 )
			 .WriteTo.NSLog()
			 .CreateLogger();
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
