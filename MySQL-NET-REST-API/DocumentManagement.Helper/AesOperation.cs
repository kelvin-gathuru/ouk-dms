using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Cryptography;

namespace DocumentManagement.Helper
{
    public class AesOperation
    {
        private static readonly byte[] SALT = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };

        //public static byte[] EncryptStream(byte[] input, string password)
        //{
        //    byte[] encrypted;
        //    using (Aes aes = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
        //        aes.Key = pdb.GetBytes(32);
        //        aes.IV = pdb.GetBytes(16);
        //        ICryptoTransform encryptor = aes.CreateEncryptor();
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        //            {
        //                cryptoStream.Write(input, 0, input.Length);
        //                cryptoStream.FlushFinalBlock();
        //                cryptoStream.Close();
        //            }
        //            encrypted = memoryStream.ToArray();
        //        }
        //    }
        //    return encrypted;
        //}

        //public static byte[] DecryptStream(byte[] input, string password)
        //{

        //    byte[] decrypted;
        //    using (Aes aes = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, SALT);
        //        aes.Key = pdb.GetBytes(32);
        //        aes.IV = pdb.GetBytes(16);
        //        ICryptoTransform decryptor = aes.CreateDecryptor();

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
        //            {
        //                cryptoStream.Write(input, 0, input.Length);
        //                cryptoStream.Close();
        //            }
        //            decrypted = memoryStream.ToArray();
        //        }
        //    }
        //    return decrypted;
        //}
        public static byte[] EncryptFileToBytes(IFormFile file, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream()) // Output encrypted data to memory
                using (CryptoStream csEncrypt = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (Stream fileStream = file.OpenReadStream()) // Read from the uploaded IFormFile stream
                {
                    byte[] buffer = new byte[1024];
                    int read;
                    while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        csEncrypt.Write(buffer, 0, read); // Encrypt the file contents
                    }

                    csEncrypt.FlushFinalBlock(); // Ensure all encrypted data is written
                    return memoryStream.ToArray(); // Return the encrypted data as a byte array
                }
            }
        }

        public static byte[] EncryptBytesToBytes(byte[] inputBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream()) // Output encrypted data to memory
                using (CryptoStream csEncrypt = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    csEncrypt.Write(inputBytes, 0, inputBytes.Length); // Encrypt the input bytes
                    csEncrypt.FlushFinalBlock(); // Ensure all encrypted data is written
                    return memoryStream.ToArray(); // Return the encrypted data as a byte array
                }
            }
        }

        public static byte[] DecryptFileFromBytes(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(encryptedData)) // Encrypted data input
                using (CryptoStream csDecrypt = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (MemoryStream decryptedStream = new MemoryStream()) // Output decrypted data
                {
                    byte[] buffer = new byte[1024];
                    int read;
                    while ((read = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        decryptedStream.Write(buffer, 0, read); // Write decrypted data to memory
                    }

                    return decryptedStream.ToArray(); // Return the decrypted data as byte array
                }
            }
        }

        public static byte[] ConvertStreamToByteArray(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream); // Copy the stream to a MemoryStream
                return memoryStream.ToArray(); // Return the byte array from MemoryStream
            }
        }
        public static byte[] ConvertIFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream); // Copy the file content into the memory stream
                return memoryStream.ToArray(); // Convert the memory stream to byte array
            }
        }


    }

}
