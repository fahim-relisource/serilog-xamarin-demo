using SerilogFileLogger.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SerilogFileLogger
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		private readonly IOurLoggerService ourLogger;
		private static int taskCounter = 1;

		public MainPage()
		{
			InitializeComponent();
			ourLogger = DependencyService.Get<IOurLoggerService>();
			ourLogger.LogInformation("MainPage Constructed");
		}

		private async void RunAsyncButton_Clicked(object sender, EventArgs e)
		{
			ourLogger.LogInformation("LongRunningTask Clicked");
			await Task.Factory.StartNew(async () =>
			{
				await LongRunningTask(taskCounter);
			});
			taskCounter++;
		}

		public async Task LongRunningTask(int taskId)
		{
			ourLogger.LogInformation("LongRunningTask Start Task:" + taskId);
			await Task.Factory.StartNew(() =>
			{
				var newMessage = $"Long Running Task With Message Task:'{taskId}'";
				ourLogger.LogInformation(newMessage);
			});
			ourLogger.LogInformation("LongRunningTask will delay for 2 seconds for Task:" + taskId);
			var random = new Random();
			await Task.Delay(random.Next(1, 20) * 1000);
			ourLogger.LogInformation("LongRunningTask End Task:" + taskId);
		}
	}
}
