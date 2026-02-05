using System;
using System.Security.Cryptography;

public class KeyGenerator
{
    public static Tuple<byte[], byte[]>  GenerateKeyAndIV()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            aesAlg.GenerateIV();
            return new Tuple<byte[], byte[]> (aesAlg.Key, aesAlg.IV);
        }
    }
}
