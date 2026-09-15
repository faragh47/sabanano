using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Services.IServices.V2;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.Services.V2
{
    /// <summary>
    /// Implements IFileServerProvider in a very simple way, for demonstration only
    /// </summary>
    public class FileServerProvider : IFileServerProvider
    {
        public FileServerProvider(IList<FileServerOptions> fileServerOptions)
        {
            FileServerOptionsCollection = fileServerOptions;
        }

        public IList<FileServerOptions> FileServerOptionsCollection { get; }

        public IFileProvider GetProvider(string virtualPath)
        {
            var options = FileServerOptionsCollection.FirstOrDefault(e => e.RequestPath == virtualPath);
            if (options != null)
                return options.FileProvider;

            throw new FileNotFoundException($"virtual path {virtualPath} is not registered in the fileserver provider");
        }
    }
}
