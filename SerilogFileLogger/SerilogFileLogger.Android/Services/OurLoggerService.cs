using SerilogFileLogger.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(SerilogFileLogger.Droid.Services.OurLoggerService))]
namespace SerilogFileLogger.Droid.Services
{
	class OurLoggerService : IOurLoggerService
	{
		public void LogInformation(string message)
		{
			MainActivity.OurLogger.Information(message);
		}
	}
}