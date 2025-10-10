using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1. Get Located Folder Path
            //E:\.Net\MVC\MVC Project\Mvc Project Solution\Demo.PL\wwwroot\Files\Images\
            //Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + FolderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            
            // 2. Get File Name and Make it Unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File As Streams
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            // 5. Return File Name
            return FileName;
        }
        // Delete
        public static void DeleteFile(string FileName, string FolderName)
        {
            // 1. Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);
            // 2. Check if Exists or Not
            if (System.IO.File.Exists(FilePath))
            {
                // if Exists Romove it
                System.IO.File.Delete(FilePath);
            }
        }

    }
}
