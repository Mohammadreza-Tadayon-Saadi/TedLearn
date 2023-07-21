using Core.Generators;
using MediaInfoLib;
using Microsoft.AspNetCore.Http;
using SharpCompress.Archives;
using SharpCompress.Common;
using System.Drawing;
using System.IO;

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

        var extentions = new[] { ".mp4", ".mkv" , "mov" };

        if (extentions.Contains(extention.ToLower()))
        {
            try
            {
                var mediaInfo = new MediaInfo();
                mediaInfo.Open(file.FullName);
                var duration = mediaInfo.Get(StreamKind.Video, 0, "Duration");
                var format = mediaInfo.Get(StreamKind.General, 0, "Format");
                mediaInfo.Close();

                if (duration.HasValue() && format.HasValue())
                {
                    //var knownVideoFormats = new[] { "avi", "mp4", "mkv", "wmv", "mov" };
                    // Check if the duration is greater than zero
                    if (double.TryParse(duration, out var durationInSeconds) && durationInSeconds > 0)
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public static bool IsRarFile(IFormFile iFile)
    {
        var file = new FileInfo(iFile.FileName.ToLower());
        var extention = file.Extension;

        if (extention == ".rar")
        {
            try
            {
                using (var archive = ArchiveFactory.Open(file.FullName))
                {
                    return archive.Type == ArchiveType.Rar;
                }
            }
            catch
            {
                // If an exception is thrown, the file is not a valid archive
                return false;
            }
        }

        return false;
    }
}
