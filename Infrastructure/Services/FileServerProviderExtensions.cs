using Microsoft.AspNetCore.Builder;
using Services.IServices.V2;

namespace Services.Services.V2
{
    /// <summary>
    /// Wrapper for UseFileServer to easily use the FileServerOptions registered in the IFileServerProvider
    /// </summary>
    public static class FileServerProviderExtensions
    {
        public static IApplicationBuilder UseFileServerProvider(this IApplicationBuilder application,
            IFileServerProvider fileServerprovider)
        {
            foreach (var option in fileServerprovider.FileServerOptionsCollection)
            {
                application.UseFileServer(option);
            }
            return application;
        }
    }
}
