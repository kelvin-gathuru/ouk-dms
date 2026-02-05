using System;
using Microsoft.Extensions.Configuration;

namespace DocumentManagement.Helper;

public class PathHelper
{
    public IConfiguration _configuration;

    public PathHelper(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string TestFile
    {
        get
        {
            return _configuration["TestFile"];
        }
    }
    public string DocumentPath
    {
        get
        {
            return _configuration["DocumentPath"];
        }
    }

    public string SignaturePath
    {
        get
        {
            return _configuration["SignaturePath"];
        }
    }

    public string FileRequestPath
    {
        get
        {
            return _configuration["FileRequestPath"];
        }
    }

    public string SearchIndexPath
    {
        get
        {
            return _configuration["SearchIndexPath"];
        }
    }
    public string AesEncryptionKey
    {
        get
        {
            return _configuration["AesEncryptionKey"];
        }
    }
    public bool AllowEncryption
    {
        get
        {
            return Convert.ToBoolean(_configuration["AllowEncryption"]);
        }
    }
    public string ReminderFromEmail
    {
        get
        {
            return _configuration["ReminderFromEmail"];
        }
    }
    public string TESSDATA
    {
        get
        {
            return _configuration["TESSDATA"];
        }
    }

    public string FrontEndUrl
    {
        get
        {
            return _configuration["FrontEndUrl"];
        }
    }

    public string[] CorsUrls
    {
        get
        {
            return string.IsNullOrEmpty(_configuration["CorsUrls"]) ? new string[] { } : _configuration["CorsUrls"].Split(",");
        }
    }
    public string[] IMAGESSUPPORT
    {
        get
        {
            return string.IsNullOrEmpty(_configuration["IMAGESSUPPORT"]) ? new string[] { } : _configuration["IMAGESSUPPORT"].Split(",");
        }
    }
    public long MaxFileSizeIndexingQuick
    {
        get
        {
            return Convert.ToInt64(_configuration["MaxFileSizeIndexingQuick"]);
        }
    }
    public string TESSSUPPORTLANGUAGES
    {
        get
        {
            return _configuration["TESSSUPPORTLANGUAGES"];
        }
    }
    public string libreOfficePath
    {
        get
        {
            return _configuration["libreOfficePath"];
        }
    }
    public string ChatGPTAPIKey
    {
        get
        {
            return _configuration["ChatGptSettings:ApiKey"];
        }
    }
    public string ChatGPTAPIURL
    {
        get
        {
            return _configuration["ChatGptSettings:ApiUrl"];
        }
    }
    public string SUMMARYFOLDER
    {
        get
        {
            return _configuration["SUMMARYFOLDER"];
        }
    }

    public string GEMINI_APIKEY
    {
        get
        {
            return _configuration["GEMINIAPIKEY"];
        }
    }

}
