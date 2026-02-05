using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Helper
{
    public class MimeTypeHelper
    {
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Office Extension
                { "application/msword", ".doc" },
                { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx" },
                { "application/vnd.ms-powerpoint", ".ppt" },
                { "application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx" },
                { "application/vnd.ms-excel", ".xls" },
                { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx" },
                { "text/csv", ".csv" },
                { "application/pdf", ".pdf" },
                { ".txt", "text/plain" },
                { ".zip", "application/zip" },
                { ".json", "application/json" },

                //Images
                { "image/jpeg", ".jpeg" },
                { "image/png", ".png" },
                { "image/gif", ".gif" },
                { "image/bmp", ".bmp" },
                { "image/tiff", ".tiff" },
                { "image/svg+xml", ".svg" },
                { "image/webp", ".webp" },
                { "image/x-icon", ".ico" },
                { "image/heif", ".heif" },
                { "image/heic", ".heic" },
                { "image/avif", ".avif" },
                { "image/apng", ".apng" },
                { "image/wmf", ".wmf" },
                { "image/emf", ".emf" },
                { "image/vnd.djvu", ".djv" },
                { "image/eps", ".eps" },
                { "image/ps", ".ps" },
                { "image/ai", ".ai" },
                { "image/cdr", ".cdr" },
                { "image/indd", ".indd" },
                { "image/idml", ".idml" },
                { "image/pdf", ".pdf" },
                { "image/psd", ".psd" },
                { "image/tga", ".tga" },
                { "image/dds", ".dds" },

                //Audio
                { "audio/3gpp", ".3gp" },
                { "audio/audible", ".aa" },
                { "audio/aac", ".aac" },
                { "audio/vnd.audible.aax", ".aax" },
                { "audio/act", ".act" },
                { "audio/aiff", ".aiff" },
                { "audio/alac", ".alac" },
                { "audio/amr", ".amr" },
                { "audio/ape", ".ape" },
                { "audio/basic", ".au" },
                { "audio/amr-wb", ".awb" },
                { "audio/dss", ".dss" },
                { "audio/dvf", ".dvf" },
                { "audio/flac", ".flac" },
                { "audio/x-gsm", ".gsm" },
                { "audio/iklax", ".iklax" },
                { "audio/ivs", ".ivs" },
                { "application/vnd.smaf", ".mmf" },
                { "audio/mpeg", ".mp3" },
                { "audio/mpc", ".mpc" },
                { "audio/msv", ".msv" },
                { "application/vnd.noblenet-mime", ".nmf" },
                { "audio/ogg", ".ogg" },
                { "audio/mogg", ".mogg" },
                { "audio/opus", ".opus" },
                { "application/vnd.lotus-organizer", ".org" },
                { "audio/x-realaudio", ".ra" },
                { "audio/x-pn-realaudio", ".rm" },
                { "audio/raw", ".raw" },
                { "audio/rf64", ".rf64" },
                { "audio/sln", ".sln" },
                { "audio/tta", ".tta" },
                { "audio/voc", ".voc" },
                { "audio/vox", ".vox" },
                { "audio/wav", ".wav" },
                { "audio/x-ms-wma", ".wma" },
                { "audio/wavpack", ".wv" },

                //Video
                { "video/webm", ".webm" },
                { "video/x-flv", ".flv" },
                { "video/x-ms-vob", ".vob" },
                { "video/ogg", ".ogv" },
                { "video/x-drc", ".drc" },
                { "video/x-msvideo", ".avi" },
                { "video/MP2T", ".mts" },
                { "video/x-ms-wmv", ".wmv" },
                { "video/x-yuv", ".yuv" },
                { "video/vnd.vivo", ".viv" },
                { "video/mp4", ".mp4" },
                { "video/x-f4v", ".f4v" },
                { "audio/x-f4a", ".f4a" }
            };

        public static string GetFileExtension(string contentType)
        {
            if (MimeTypes.TryGetValue(contentType, out var extension))
            {
                return extension;
            }
            else
            {
                return "";
            }
        }
    }
}
