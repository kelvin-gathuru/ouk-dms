using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;


namespace DocumentManagement.Helper
{
    public class FileHelper
    {

        public static IFormFile ConvertToIFormFile(string filePath)
        {
            try
            {
                // Step 1: Read the file from the file system
                var fileInfo = new System.IO.FileInfo(filePath);

                // Step 2: Open a file stream
                var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

                var formFile = new FormFile(stream, 0, fileInfo.Length, null, fileInfo.Name);

                // Step 3: Create the FormFile object

                return formFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string GetMimeType(string filePath)
        {
            // Create an instance of the provider
            var provider = new FileExtensionContentTypeProvider();

            // Get the file extension
            var fileExtension = Path.GetExtension(filePath);

            // Try to get the MIME type based on the file extension
            if (provider.TryGetContentType(filePath, out string contentType))
            {
                return contentType;
            }

            // Default content type if no match is found
            return "application/octet-stream";
        }


        public static string GetMimeTypeByExtension(string extension)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }
            // Remove leading dot (if any
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(extension, out string mimeType))
            {
                return mimeType;
            }
            return "application/octet-stream"; // Default if MIME type is unknown
        }
    }

}
