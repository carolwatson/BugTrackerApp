using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTFileService
    {
        //for image service

        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        public string ConvertByteArrayToFileAsync(byte[]? fileData, string extension);

        public string GetFileIcon(string file);

        public string FormatFileSize(long bytes);


    }
}
