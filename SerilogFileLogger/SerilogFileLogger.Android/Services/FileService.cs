using SerilogFileLogger.Services;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(SerilogFileLogger.Droid.Services.FileService))]
namespace SerilogFileLogger.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetStorageFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}