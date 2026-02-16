
using Android.Content;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace SkyeMinder.Services
{
    public static class FileSaver
    {
        public static async Task SaveToDownloadsAsync(string content, string fileName)
        {
#if ANDROID
#pragma warning disable CA1416
            var context = Android.App.Application.Context;
            if (context == null)
            {
                await Shell.Current.DisplayAlertAsync("Export Failed", "App context is unavailable.", "OK");
                return;
            }

            var resolver = context.ContentResolver;
            if (resolver == null)
            {
                await Shell.Current.DisplayAlertAsync("Export Failed", "Content resolver is unavailable.", "OK");
                return;
            }

            var values = new ContentValues();
            values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
            if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                values.Put(MediaStore.IMediaColumns.MimeType, "text/csv");
            else if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                values.Put(MediaStore.IMediaColumns.MimeType, "application/pdf");
            values.Put(MediaStore.IMediaColumns.RelativePath, Android.OS.Environment.DirectoryDownloads);

            var uri = resolver.Insert(MediaStore.Downloads.ExternalContentUri, values);
            if (uri == null)
            {
                await Shell.Current.DisplayAlertAsync("Export Failed", "Could not create file URI.", "OK");
                return;
            }

            using var stream = resolver.OpenOutputStream(uri);
            if (stream == null)
            {
                await Shell.Current.DisplayAlertAsync("Export Failed", "Could not open output stream.", "OK");
                return;
            }

            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                byte[] pdfBytes = Convert.FromBase64String(content);
                using var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write(pdfBytes);
            }
            else
            {
                using var writer = new StreamWriter(stream);
                writer.Write(content);
            }
#pragma warning restore CA1416
#endif
        }
        public static async Task SaveToAppDataAsync(string content, string fileName)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllTextAsync(filePath, content);
        }
        public static async Task SaveToAppDataAsync(byte[] bytes, string fileName)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            await File.WriteAllBytesAsync(filePath, bytes);
        }
        public static string GetAppDataFilePath(string fileName)
        {
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }

    }
}