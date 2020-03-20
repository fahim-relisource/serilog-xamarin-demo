using SerilogFileLogger.Services;
using SerilogFileLogger.Services.Impl;
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
		private static int serviceCounter = 1;
		private static int fileCounter = 1;

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

		private void RunServicebutton_Clicked(object sender, EventArgs e)
		{
			ourLogger.LogInformation("RunServiceButton Clicked");
			var futureService = DependencyService.Get<IFutureWork>();
			futureService.SetNewFutureWork(serviceCounter);
			serviceCounter++;
		}

		private async void FileDownloadButton_Clicked(object sender, EventArgs e)
		{
			string fileUrl = $"https://dummyimage.com/2560x1600/08f24a/e61c6d.jpg&text=File+Number+{fileCounter}";
			ourLogger.LogInformation($"File Download Clicked For Downloading file {fileUrl}");
			var fileDownloadService = new DownloadService(DependencyService.Get<IFileService>());
			await Task.Factory.StartNew(async () =>
			{
				string filePath = await fileDownloadService.DownloadFileAsync(fileUrl, $"FileNumber{fileCounter}.jpg");
				if(filePath != null)
				{
					ourLogger.LogInformation($"Downloaded file location: {filePath}");
					DownloadImageFile.Source = ImageSource.FromFile(filePath);
				}
			});
			fileCounter++;
		}
	}
}
