const mimeToExtension: { [key: string]: string[] } = {
  // Images
  "image/jpeg": ["jpg", "jpeg"],
  "image/png": ["png"],
  "image/gif": ["gif"],
  "image/bmp": ["bmp"],
  "image/webp": ["webp"],
  "image/svg+xml": ["svg"],
  "image/tiff": ["tiff"],
  "image/vnd.microsoft.icon": ["ico"],
  'image/x-icon': ['ico'],
  'image/avif': ['avif'],


  // Documents
  "application/pdf": ["pdf"],
  "application/msword": ["doc"],
  "application/vnd.openxmlformats-officedocument.wordprocessingml.document": ["docx"],
  "application/vnd.ms-excel": ["xls"],
  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet": ["xlsx"],
  "application/vnd.ms-powerpoint": ["ppt"],
  "application/vnd.openxmlformats-officedocument.presentationml.presentation": ["pptx"],
  "application/vnd.oasis.opendocument.text": ["odt"],
  "application/vnd.oasis.opendocument.spreadsheet": ["ods"],
  "application/vnd.oasis.opendocument.presentation": ["odp"],

  // Text
  "text/plain": ["txt"],
  "text/csv": ["csv"],
  "text/html": ["html"],
  "text/xml": ["xml"],
  "text/javascript": ["js"],
  "text/css": ["css"],
  "text/markdown": ["md"],

  // Audio
  "audio/mpeg": ["mp3"],
  "audio/ogg": ["ogg", "oga"],
  "audio/wav": ["wav"],
  "audio/flac": ["flac"],
  "audio/aac": ["aac"],
  "audio/vnd.dlna.adts": ["aac"],
  "audio/x-m4a": ["m4a"],
  "audio/x-m4b": ["m4b"],
  "audio/x-m4p": ["m4p"],
  "audio/x-ms-wma": ["wma"],

  // Video
  "video/mp4": ["mp4"],
  "video/x-msvideo": ["avi"],
  "video/x-matroska": ["mkv"],
  "video/quicktime": ["mov"],
  "video/webm": ["webm"],
  "video/x-flv": ["flv"],
  "video/ogv": ["ogv"],
  "video/ogg": ["ogv"],
  "video/x-ms-wmv": ["wmv"],

  // Archives
  "application/zip": ["zip"],
  "application/x-zip-compressed": ["zip"],
  "application/x-tar": ["tar"],
  "application/gzip": ["gz"],
  "application/x-7z-compressed": ["7z"],
  "application/x-compressed": ["7z"],
  "application/x-rar-compressed": ["rar"],

  // Code files
  "application/javascript": ["js"],
  "application/json": ["json"],
  "application/xml": ["xml"],
  "application/x-httpd-php": ["php"],
  "application/x-sh": ["sh"],
  "application/x-java-archive": ["jar"],
  "text/x-python": ["py"],
  "text/x-c": ["c"],
  "text/x-c++": ["cpp"],
  "text/x-java-source": ["java"],
  "text/x-typescript": ["ts"],

  // Fonts
  "font/woff": ["woff"],
  "font/woff2": ["woff2"],
  "application/x-font-ttf": ["ttf"],
  "application/x-font-opentype": ["otf"],

  // Others
  "application/octet-stream": ["bin"],
  "application/x-msdownload": ["exe"],
  "application/x-iso9660-image": ["iso"]
};

// Function to get extension from MIME type
function isFileExtensionCorrect(file: File): boolean {
  const fileType = file.type; // MIME type
  const fileExtension = file.name.split(".").pop()?.toLowerCase() ?? '';
  return mimeToExtension[fileType]?.includes(fileExtension) ?? false;
}



function checkFileCorruption(file: File): Promise<boolean> {
  return new Promise((resolve) => {
    const reader = new FileReader();

    reader.onload = () => resolve(true);  // File successfully read
    reader.onerror = () => resolve(false); // File unreadable (possibly corrupted)

    reader.readAsArrayBuffer(file);
  });
}

const signatures: { [key: string]: number[][] } = {
  // Images
  jpg: [[0xFF, 0xD8, 0xFF]], // JPEG
  jpge: [[0xFF, 0xD8, 0xFF]], // JPEG
  png: [[0x89, 0x50, 0x4E, 0x47]], // PNG
  gif: [[0x47, 0x49, 0x46, 0x38]], // GIF87a or GIF89a
  bmp: [[0x42, 0x4D]], // BMP
  tiff: [[0x49, 0x49, 0x2A, 0x00], [0x4D, 0x4D, 0x00, 0x2A]], // TIFF (Little and Big Endian)
  avif: [
    [0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66], // AVIF
    [0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66]  // Sometimes size varies
  ],

  // Documents
  pdf: [[0x25, 0x50, 0x44, 0x46]], // "%PDF"
  doc: [[0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]], // DOC (old MS Word format)
  docx: [[0x50, 0x4B, 0x03, 0x04]], // DOCX, PPTX, XLSX (ZIP format)
  xls: [[0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]], // XLS (old Excel format)
  xlsx: [[0x50, 0x4B, 0x03, 0x04]], // XLSX
  ppt: [[0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1]], // PPT (old PowerPoint format)
  pptx: [[0x50, 0x4B, 0x03, 0x04]], // PPTX
  odt: [[0x50, 0x4B, 0x03, 0x04]], // ODT (ZIP format)


  // Text Files
  txt: [[0xEF, 0xBB, 0xBF], [0xFF, 0xFE], [0xFE, 0xFF]], // UTF-8 BOM, UTF-16 LE, UTF-16 BE

  // Archives
  zip: [[0x50, 0x4B, 0x03, 0x04]], // ZIP
  rar: [[0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00]], // RAR
  "7z": [[0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C]], // 7-Zip
  tar: [[0x75, 0x73, 0x74, 0x61, 0x72]], // TAR
  gz: [[0x1F, 0x8B, 0x08]], // GZIP

  // Audio
  // mp3: [[0x49, 0x44, 0x33], [0xFF, 0xFB]], // ID3v2 & MP3 raw format
  wav: [[0x52, 0x49, 0x46, 0x46]], // WAV
  flac: [[0x66, 0x4C, 0x61, 0x43]], // FLAC

  // Video
  mp4: [[0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70]], // MP4
  avi: [[0x52, 0x49, 0x46, 0x46]], // AVI
  mkv: [[0x1A, 0x45, 0xDF, 0xA3]], // MKV
  mov: [[0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74]], // MOV (QuickTime)
  webm: [[0x1A, 0x45, 0xDF, 0xA3]], // WebM

  // Executables
  dll: [[0x4D, 0x5A]], // Windows DLL (MZ)

  // Others
  iso: [[0x43, 0x44, 0x30, 0x30, 0x31]], // ISO Disk Image
  swf: [[0x43, 0x57, 0x53], [0x46, 0x57, 0x53]], // SWF Flash File
  ico: [[0x00, 0x00, 0x01, 0x00]],
};

async function isFileSignatureValid(file: File): Promise<boolean> {
  const reader = new FileReader();
  return new Promise((resolve) => {
    reader.onload = (event) => {
      const uint8Array = new Uint8Array(event.target?.result as ArrayBuffer);
      const ext = file.name.split(".").pop()?.toLowerCase() ?? "";
      if (ext === "txt") {
        const text = event.target?.result as string;

        // Check if the file contains mostly printable characters
        const nonPrintableCount = Array.from(text).filter((char) => {
          const charCode = char.charCodeAt(0);
          return charCode < 32 && charCode !== 9 && charCode !== 10 && charCode !== 13; // Exclude common control chars (\t, \n, \r)
        }).length;

        // If more than 10% of characters are non-printable, it's NOT a text file
        resolve(nonPrintableCount < text.length * 0.1);
        return;
      }

      if (!signatures[ext]) {
        resolve(true); // Unknown file type
        return;
      }
      if (ext === "mp4" || ext === "m4v" || ext === "m4a" || ext === "f4v" || ext === "mov" || ext == "mp41" || ext == "mp42" || ext == "isom" || ext == "iso2" || ext == "avc1" || ext == "M4V" || ext == "M4A" || ext == "MSNV" || ext == "F4V" || ext == "3gp4") {
        if (
          uint8Array[4] !== 0x66 || // 'f'
          uint8Array[5] !== 0x74 || // 't'
          uint8Array[6] !== 0x79 || // 'y'
          uint8Array[7] !== 0x70    // 'p'
        ) {
          resolve(false);
          return;
        }
        resolve(true);
        return;
      }

      const possibleSignatures = signatures[ext];
      const actualSignature = uint8Array.slice(0, possibleSignatures[0].length);

      const isValid = possibleSignatures.some((expectedSignature) =>
        expectedSignature.every((byte, index) => byte === actualSignature[index])
      );

      resolve(isValid);
    };
    reader.onerror = () => resolve(false);
    const ext = file.name.split(".").pop()?.toLowerCase() ?? "";
    if (ext === "txt") {
      reader.readAsText(file);
    } else if (ext === "mp4" || ext === "m4v" || ext === "m4a" || ext === "f4v" || ext === "mov" || ext == "mp41" || ext == "mp42" || ext == "isom" || ext == "iso2" || ext == "avc1" || ext == "M4V" || ext == "M4A" || ext == "MSNV" || ext == "F4V" || ext == "3gp4") {
      reader.readAsArrayBuffer(file.slice(0, 8));
    } else if (ext === "avif") {
      reader.readAsArrayBuffer(file.slice(0, 12));
    }
    else if (ext === "xls" || ext === "ppt" || ext === "doc" || ext === "mp4") {
      reader.readAsArrayBuffer(file.slice(0, 8));
    }
    else if (ext === "7z") {
      reader.readAsArrayBuffer(file.slice(0, 6));
    }
    else {
      console.log("Reading first 4 bytes of file for signature check", file.slice(0, 4));
      reader.readAsArrayBuffer(file.slice(0, 4)); // Read first 4 bytes for comparison
    }
  });
}

export async function validateFile(file: File) {
  // if (!isFileExtensionCorrect(file)) {
  //   console.warn("File extension does not match its MIME type.");
  //   return false;
  // }

  if (!(await checkFileCorruption(file))) {
    console.error("File is corrupted.");
    return false;
  }

  // if (!(await isFileSignatureValid(file))) {
  //   console.error("File signature does not match expected format.");
  //   return false;
  // }

  console.log("File is valid and safe for upload.");
  return true;
}

