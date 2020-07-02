using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksum;

namespace MSLibrary.Compression
{
    [Injection(InterfaceType = typeof(ICompressionService), Scope = InjectionScope.Singleton)]
    public class CompressionService : ICompressionService
    {
        static CompressionService()
        {
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        }
        public async Task GetCompressionStream(Func<Stream, Task> action, params CompressionTextItemFileInfo[] items)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipOutputStream outStream = new ZipOutputStream(ms))
                {
                    Crc32 crc = new Crc32();


                    foreach (var item in items)
                    {
                        using (MemoryStream fileStream = new MemoryStream())
                        {
                            var fileBytes = UTF8Encoding.UTF8.GetBytes(item.Text);
                            await fileStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                            ZipEntry entry = new ZipEntry(item.FileName);
                            entry.IsUnicodeText = true;
                            entry.DateTime = DateTime.Now;
                            entry.Size = fileBytes.Length;

                            crc.Reset();
                            crc.Update(fileBytes);

                            entry.Crc = crc.Value;

                            outStream.PutNextEntry(entry);
                            ///将缓存区对象数据写入流中
                            outStream.Write(fileBytes, 0, fileBytes.Length);
                            ///注意：一定要关闭当前条目，否则压缩包数据会丢失
                            outStream.CloseEntry();

                        }

                    }

                    outStream.Finish();
                    ms.Position = 0;
                    await action(ms);
                }

            }
        }

        public async Task<string> GetCompressionText(string text)
        {
            string result = string.Empty;
            byte[] b = Encoding.Unicode.GetBytes(text);
            using (MemoryStream to = new MemoryStream())
            await using (ZipOutputStream zip = new ZipOutputStream(to))
            {
                ZipEntry entry = new ZipEntry("ToBase64String");
                entry.IsUnicodeText = true;
                zip.PutNextEntry(entry);

                zip.Write(b, 0, b.Length);
                zip.CloseEntry();
                zip.Finish();

                result = Convert.ToBase64String(to.ToArray());
            }


            return result;
        }

        public async Task<string> GetUnCompressionText(string compressionText)
        {
            string result = string.Empty;

            byte[] b = Convert.FromBase64String(compressionText);
            using (MemoryStream from = new MemoryStream(b))
            using (ZipInputStream zip = new ZipInputStream(from))
            await using (MemoryStream to = new MemoryStream())
            {
                ZipEntry entry = zip.GetNextEntry();
                entry.IsUnicodeText = true;
                byte[] buffer = new byte[1024];
                int len = 0;
                while ((len = zip.Read(buffer, 0, buffer.Length)) > 0)
                {
                    to.Write(buffer, 0, len);
                }
                b = to.ToArray();
                result = Encoding.Unicode.GetString(b);
            }
            return result;
        }


        public async Task<CompressionTextItemFileInfo[]> GetUnCompressionFileInfo(Stream stream)
        {
            List<CompressionTextItemFileInfo> result = new List<CompressionTextItemFileInfo>();


            using (var zipStream = new ZipInputStream(stream))
            {
                ZipEntry ent = null;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    ent.IsUnicodeText = true;
                    if (!string.IsNullOrEmpty(ent.Name))
                    {
                        var fileName = ent.Name.Replace('/', '\\');

                        if (!fileName.EndsWith("\\"))
                        {
                            CompressionTextItemFileInfo info = new CompressionTextItemFileInfo();
                            info.FileName = ent.Name;
                            info.Text = string.Empty;

                            using (var textStream = new MemoryStream())
                            {
                                int size = 2048;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = zipStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                        await textStream.WriteAsync(data, 0, data.Length);
                                    else
                                        break;
                                }
                                info.Text = UTF8Encoding.UTF8.GetString(textStream.ToArray());
                            }


                            result.Add(info);
                        }



                    }
                }

            }


            return result.ToArray();
        }
    }
}
