using SerilogFileLogger.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SerilogFileLogger.iOS.Services.OurLoggerService))]
namespace SerilogFileLogger.iOS.Services
{
	class OurLoggerService : IOurLoggerService
	{
		public void LogInformation(string message)
		{
			Application.OurLogger.Information(message);
		}
	}
}