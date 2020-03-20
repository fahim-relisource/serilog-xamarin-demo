using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SerilogFileLogger.Services.Impl
{
	// Taken From https://damienaicheh.github.io/xamarin/xamarin.forms/2018/07/10/download-a-file-with-progress-bar-in-xamarin-forms-en.html
	public class DownloadService : IDownloadService
	{
		private HttpClient _client;

		private readonly IFileService _fileService;
		private int bufferSize = 4095;

		public DownloadService(IFileService fileService)
		{
			_client = new HttpClient();
			_fileService = fileService;
		}

		public async Task<string> DownloadFileAsync(string url, string fileName)
		{
			var logger = DependencyService.Get<IOurLoggerService>();
			try
			{
				logger.LogInformation($"Trying to download file {fileName} from {url}");
				// Step 1 : Get call
				var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

				if (!response.IsSuccessStatusCode)
				{
					logger.LogInformation(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
				}

				// Step 3 : Get total of data
				var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
				var canSendProgress = totalData != -1L;

				// Step 4 : Get total of data
				var filePath = Path.Combine(_fileService.GetStorageFolderPath(), fileName);

				// Step 5 : Download data
				using (var fileStream = OpenStream(filePath))
				{
					using (var stream = await response.Content.ReadAsStreamAsync())
					{
						var totalRead = 0L;
						var buffer = new byte[bufferSize];
						var isMoreDataToRead = true;

						do
						{

							var read = await stream.ReadAsync(buffer, 0, buffer.Length);

							if (read == 0)
							{
								isMoreDataToRead = false;
							}
							else
							{
								// Write data on disk.
								await fileStream.WriteAsync(buffer, 0, read);

								totalRead += read;

								if (canSendProgress)
								{
									// ToDO log file progress
									logger.LogInformation($"{fileName} Download Progress {(totalRead * 1d) / (totalData * 1d) * 100}%");
								}
							}
						} while (isMoreDataToRead);
					}
					logger.LogInformation($"{fileName} File Download Complete.");
				}

				return filePath;
			}
			catch (Exception e)
			{
				// Manage the exception as you need here.
				logger.LogInformation($"Exception Occured when downloading file {e.StackTrace}");

				return null;
			}
		}

		private Stream OpenStream(string path)
		{
			return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize);
		}
	}
}