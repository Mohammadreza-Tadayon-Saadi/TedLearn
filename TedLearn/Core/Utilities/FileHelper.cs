using Core.Generators;
using Microsoft.AspNetCore.Http;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Drawing;

namespace Core.Utilities;

public class FileHelper
{
    public static async Task<string> CreateFileAsync(IFormFile file, string directory, bool hasName = false)
    {
        string filePath = "";
        string fileName = file.FileName;

        //Save File
        if (!hasName)
            fileName = Generator.GenerateUniqName() + Path.GetExtension(fileName);

        filePath = Path.Combine(directory, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    public static void DeleteFile(string fileName, string directory)
    {
        string filePath = Path.Combine(directory, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}

public static class FileChecker
{
    public static bool IsImage(this IFormFile imageFile)
    {
        var file = new FileInfo(imageFile.FileName.ToLower());
        var extention = file.Extension;

        var extentions = new[] { ".png", ".jpeg", ".jpg" };
        if (extentions.Contains(extention.ToLower()))
        {
            try
            {
                var img = Image.FromStream(imageFile.OpenReadStream());
                return true;
            }
            catch
            {
                return false;
            }
        }


        return false;
    }

    public static bool IsVideo(IFormFile videoFile)
    {
        var file = new FileInfo(videoFile.FileName.ToLower());
        var extention = file.Extension;

        var extentions = new[] { ".mp4", ".mkv", "avi" };

        if (extentions.Contains(extention.ToLower()))
        {
            // List of known video MIME types
            var videoMimeTypes = new[] { "video/mp4", "video/avi", "video/mkv" };//, "video/quicktime" };

            // Check if the file MIME type is a known video MIME type
            if (!videoMimeTypes.Contains(videoFile.ContentType))
                // The file is not a video file
                return false;

            try
            {
                // Read the first few bytes of the file
                var signatureBytes = new byte[4];
                using (var stream = videoFile.OpenReadStream())
                {
                    stream.Read(signatureBytes, 0, signatureBytes.Length);
                }

                // List of known video file signatures
                var videoSignatures = new[] { 
                    new byte[] { 0x00, 0x00, 0x00, 0x18 }, // MP4
                    new byte[] { 0x52, 0x49, 0x46, 0x46 }, // AVI
                    new byte[] { 0x1a, 0x45, 0xdf, 0xa3 }, // Matroska (MKV)
                };
                //new byte[] { 0x00, 0x00, 0x00, 0x14 }, }; // QuickTime

                // Check if the file signature matches a known video file signature
                if (!videoSignatures.Any(s => s.SequenceEqual(signatureBytes)))
                    // The file is not a video file
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public static bool IsRarFile(IFormFile rarFile)
    {
        var file = new FileInfo(rarFile.FileName.ToLower());
        var extention = file.Extension;

        var extentions = new[] { ".rar" };

        if (extentions.Contains(extention.ToLower()))
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                rarFile.CopyTo(stream);
            }

            try
            {
                using (var archive = ArchiveFactory.Open(tempFilePath))
                {
                    var isRarFile = archive.Type == ArchiveType.Rar;

                    return isRarFile;
                }
            }
            catch
            {
                // If an exception is thrown, the file is not a valid archive
                return false;
            }
            finally
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
        }

        return false;
    }

}
