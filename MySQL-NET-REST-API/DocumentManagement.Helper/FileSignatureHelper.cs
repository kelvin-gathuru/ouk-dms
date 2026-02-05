using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Helper
{
    public static class FileSignatureHelper
    {
        public static readonly Dictionary<string, byte[][]> FileSignatures = new()
        {
            { ".pdf", new byte[][] { new byte[] { 0x25, 0x50, 0x44, 0x46 } } }, // "%PDF"
            { ".png", new byte[][] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } }, // "‰PNG"
            { ".jpg", new byte[][] { new byte[] { 0xFF, 0xD8, 0xFF } } },       // JPEG
            { ".jpeg", new byte[][] { new byte[] { 0xFF, 0xD8, 0xFF } } },      // JPEG
            { ".gif", new byte[][] { new byte[] { 0x47, 0x49, 0x46, 0x38 } } }, // "GIF8"
            { ".bmp", new byte[][] { new byte[] { 0x42, 0x4D } } },             // "BM"
            { ".tiff", new byte[][] { new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new byte[] { 0x4D, 0x4D, 0x00, 0x2A } } }, // TIFF (Little & Big Endian)
            { ".zip", new byte[][] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // ZIP
            { ".rar", new byte[][] { new byte[] { 0x52, 0x61, 0x72, 0x21 } } }, // RAR
            { ".7z", new byte[][] { new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } } }, // 7z Archive
            { ".tar", new byte[][] { new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72 } } }, // TAR Archive
            { ".gz", new byte[][] { new byte[] { 0x1F, 0x8B, 0x08 } } }, // GZIP Archive
            { ".mp3", new byte[][] { new byte[] { 0x49, 0x44, 0x33 } } }, // MP3 (ID3 Tag)
            { ".wav", new byte[][] { new byte[] { 0x52, 0x49, 0x46, 0x46 } } }, // "RIFF" (WAV)
            { ".mp4", new byte[][] {  new byte[] { 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }, // ftypisom
            new byte[] { 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }, // ftypmp42
            new byte[] { 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 }  } }, // MP4 (ISO Base Media)
            { ".avi", new byte[][] { new byte[] { 0x52, 0x49, 0x46, 0x46 } } }, // AVI
            { ".wmv", new byte[][] { new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 } } }, // WMV
            { ".flv", new byte[][] { new byte[] { 0x46, 0x4C, 0x56, 0x01 } } }, // FLV
            { ".mov", new byte[][] { new byte[] { 0x6D, 0x6F, 0x6F, 0x76 } } }, // MOV
            { ".doc", new byte[][] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } }, // DOC (Old Format)
            { ".xls", new byte[][] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } }, // XLS (Old Format)
            { ".ppt", new byte[][] { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } }, // PPT (Old Format)
            { ".docx", new byte[][] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // DOCX (ZIP-based)
            { ".xlsx", new byte[][] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // XLSX (ZIP-based)
            { ".pptx", new byte[][] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // PPTX (ZIP-based)
            { ".exe", new byte[][] { new byte[] { 0x4D, 0x5A } } }, // "MZ" (Executable)
            { ".dll", new byte[][] { new byte[] { 0x4D, 0x5A } } }, // DLL
            { ".psd", new byte[][] { new byte[] { 0x38, 0x42, 0x50, 0x53 } } }, // Photoshop
            { ".iso", new byte[][] { new byte[] { 0x43, 0x44, 0x30, 0x30, 0x31 } } }, // ISO
            { ".rtf", new byte[][] { new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66 } } }, // RTF
            { ".txt", new byte[][] { new byte[] { 0xEF, 0xBB, 0xBF }, new byte[] { 0xFF, 0xFE } } }, // UTF-8 / UTF-16 BOM (Optional)
        };


        public static bool IsTextFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            byte[] buffer = new byte[512]; // Read first 512 bytes
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // Check if the file starts with a BOM (UTF-8, UTF-16)
            foreach (var signature in FileSignatures[".txt"])
            {
                if (buffer.Take(signature.Length).SequenceEqual(signature))
                    return true; // It's a text file
            }

            // If no BOM found, check if the bytes are mostly printable ASCII characters
            int nonPrintableCount = 0;
            for (int i = 0; i < bytesRead; i++)
            {
                byte b = buffer[i];

                // Allow common control characters: \n (10), \r (13), \t (9)
                if (b < 32 && b != 9 && b != 10 && b != 13)
                {
                    nonPrintableCount++;
                }
            }

            // If more than 10% of bytes are non-printable, assume it's NOT a text file
            return (nonPrintableCount < bytesRead * 0.10);
        }

        static bool IsMp4File(byte[] fileBytes)
        {
            // MP4 Brand Signatures (ftyp should be followed by one of these)
            string[] mp4Brands = { "isom", "iso2", "mp41", "mp42", "avc1", "qt  " };

            if (fileBytes.Length < 12) return false; // Too small to be an MP4

            // Read the first 32 bytes
            byte[] fileHeader = fileBytes.ToArray();

            // Find "ftyp" anywhere in the first 32 bytes
            for (int i = 0; i < fileHeader.Length - 4; i++)
            {
                if (fileHeader[i] == 0x66 && fileHeader[i + 1] == 0x74 &&
                    fileHeader[i + 2] == 0x79 && fileHeader[i + 3] == 0x70)
                {
                    // Extract the 4-byte brand after "ftyp"
                    string brand = System.Text.Encoding.ASCII.GetString(fileHeader, i + 4, 4);

                    // Check if the brand is a known MP4 type
                    return mp4Brands.Any(b => b == brand);
                }
            }

            return false; // No valid MP4 signature found
        }

        public static bool IsFileSignatureValid(IFormFile file, string extensionValue = "")
        {
            return true;
            //string extension = !string.IsNullOrEmpty(extensionValue) ? extensionValue : Path.GetExtension(file.FileName).ToLower();
            //if (!extension.StartsWith("."))
            //{
            //    extension = "." + extension;
            //}
            //if (extension == ".txt")
            //    return IsTextFile(file);

            //if (!FileSignatures.ContainsKey(extension))
            //    return true; // Unsupported file type


            //using var stream = file.OpenReadStream();


            //byte[] buffer ;// Read up to 8 bytes (covers most signatures)

            //if (extension == ".mp4")
            //{
            //    buffer = new byte[32];
            //    stream.Read(buffer, 0, buffer.Length);
            //    return IsMp4File(buffer);
            //}
            //buffer = new byte[8];
            //stream.Read(buffer, 0, buffer.Length);

            //// Compare against all valid signatures for the given file type
            //return FileSignatures[extension].Any(signature =>
            //    buffer.Take(signature.Length).SequenceEqual(signature)
            //);
        }

    }
}
