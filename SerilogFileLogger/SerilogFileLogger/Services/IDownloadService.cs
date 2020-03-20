using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SerilogFileLogger.Services
{
	public interface IDownloadService
	{
		Task<string> DownloadFileAsync(string url, string fileName);
	}
}
