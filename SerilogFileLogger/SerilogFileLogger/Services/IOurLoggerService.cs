using System;
using System.Collections.Generic;
using System.Text;

namespace SerilogFileLogger.Services
{
	public interface IOurLoggerService
	{
		void LogInformation(string message);
	}
}
