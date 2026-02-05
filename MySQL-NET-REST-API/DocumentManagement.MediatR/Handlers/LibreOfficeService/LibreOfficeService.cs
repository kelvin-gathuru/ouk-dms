using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class LibreOfficeService
    {

        public async Task<byte[]> ConvertDocToPdfAsync(byte[] fileBytes, string fileName, string libreOfficePath)
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), "LibreOfficeConvert");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            string inputPath = Path.Combine(tempFolder, fileName);
            string outputPath = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(fileName) + ".pdf");

            // Save the DOC file to temp location
            await File.WriteAllBytesAsync(inputPath, fileBytes);

            ProcessStartInfo startInfo = new()
            {
                FileName = libreOfficePath,
                Arguments = $"--headless --convert-to pdf --outdir \"{tempFolder}\" \"{inputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new() { StartInfo = startInfo };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            Console.WriteLine(output);
            if (!string.IsNullOrEmpty(error))
                Console.WriteLine("Error: " + error);

            // Read the converted PDF file into byte array
            if (File.Exists(outputPath))
            {
                byte[] pdfBytes = await File.ReadAllBytesAsync(outputPath);
                File.Delete(inputPath); // Clean up temp files
                File.Delete(outputPath);
                return pdfBytes;
            }

            return null;
        }
    }
}
