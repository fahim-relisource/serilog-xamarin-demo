using System;
using System.Collections.Generic;
using System.Text;

namespace SerilogFileLogger.Services
{
	public interface IFutureWork
	{
		bool SetNewFutureWork(int workId);
	}
}
