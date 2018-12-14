using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FWTL.Core.Helpers
{
    public static class FileHelper
    {
        public static byte[] Zip(List<FileInfo> files)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        ZipArchiveEntry entry = archive.CreateEntry(file.Name);
                        using (var zipStream = entry.Open())
                        {
                            zipStream.Write(file.Content, 0, file.Content.Length);
                        }
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
