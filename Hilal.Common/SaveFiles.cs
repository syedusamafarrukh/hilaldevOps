using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Hilal.DataViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hilal.Common
{
    public class SaveFiles
    {
        public static FileUrlResponce SaveImage(string rootPath, IFormFile file, string savefolder, string savefolderThumbnail)
        {
            try
            {
                FileUrlResponce image = new FileUrlResponce();

                if (file.Length > 0)
                {
                    var folderPath = "/uploads/" + savefolder;
                    var folderPathThumnail = "\\uploads\\" + savefolderThumbnail + "\\Thumbnail";

                    var filePath = rootPath + folderPath;
                    Directory.CreateDirectory(filePath);
                    Directory.CreateDirectory(rootPath + folderPathThumnail);
                    var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    string thisThumbnailFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + "_Thumbnail" + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    image.URL = Path.Combine(folderPath, FileName).Replace(@"\", "/");
                    string thumbnailUrl = GenerateThumbImage(Path.Combine(filePath, FileName), Path.Combine(rootPath + folderPathThumnail, thisThumbnailFileName), 160, 250);
                    image.ThumbnailUrl = Path.Combine(folderPathThumnail, thisThumbnailFileName).Replace("\\", "/");
                }

                return image;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<FileUrlResponce> SaveImages(string rootPath, IEnumerable<IFormFile> files)
        {
            try
            {
                List<FileUrlResponce> images = new List<FileUrlResponce>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var folderPath = "/uploads/" + DateTime.UtcNow.ToString("yyyy") + "/" + DateTime.UtcNow.ToString("MMMM") + "/" + DateTime.UtcNow.ToString("dd");
                        var folderPathThumnail = "\\uploads\\" + DateTime.UtcNow.ToString("yyyy") + "\\" + DateTime.UtcNow.ToString("MMMM") + "\\" + DateTime.UtcNow.ToString("dd");

                        FileUrlResponce url = new FileUrlResponce();
                        var filePath = Path.Combine(rootPath, folderPath);
                        var path = Directory.CreateDirectory(filePath);
                        var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                        string thisThumbnailFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + "_Thumbnail" + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        url.URL = Path.Combine(folderPath, FileName).Replace(@"\", "/");
                        string thumbnailUrl = GenerateThumbImage(Path.Combine(filePath, FileName), Path.Combine(rootPath + folderPathThumnail, thisThumbnailFileName), 160, 250);
                        url.ThumbnailUrl = Path.Combine(folderPathThumnail, thisThumbnailFileName).Replace("\\", "/");

                        images.Add(url);
                    }
                }

                return images;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileUrlResponce SaveFile(string rootPath, IFormFile file, string folder)
        {
            try
            {
                FileUrlResponce image = new FileUrlResponce();

                if (file.Length > 0)
                {
                    var folderPath = "/uploads/" + folder + "/" + DateTime.UtcNow.ToString("yyyy") + "/" + DateTime.UtcNow.ToString("MMMM") + "/" + DateTime.UtcNow.ToString("dd");
                    var filePath = rootPath + folderPath;
                    var path = Directory.CreateDirectory(filePath);
                    var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    image.URL = Path.Combine(folderPath, FileName).Replace(@"\", "/");
                    image.ThumbnailUrl = "";
                }
                return image;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GenerateThumbImage(string originalImagePath, string thumbImagePath, int newWidth, int newHeight)
        {
            try
            {

                using (Bitmap srcBmp = new Bitmap(originalImagePath))
                {
                    float ratio = 1;
                    float minSize = Math.Min(newWidth, newHeight);

                    if (srcBmp.PropertyIdList.Contains(0x112))
                    {
                        var prop = srcBmp.GetPropertyItem(0x112);
                        if (prop.Type == 3 && prop.Len == 2)
                        {
                            UInt16 orientationExif = BitConverter.ToUInt16(srcBmp.GetPropertyItem(0x112).Value, 0);
                            if (orientationExif == 8)
                            {
                                srcBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            }
                            else if (orientationExif == 3)
                            {
                                srcBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            }
                            else if (orientationExif == 6)
                            {
                                srcBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            }
                        }
                    }

                    if (srcBmp.Width > srcBmp.Height)
                    {
                        ratio = minSize / (float)srcBmp.Width;
                    }
                    else
                    {
                        ratio = minSize / (float)srcBmp.Height;
                    }

                    SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                    Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);

                    using (Graphics graphics = Graphics.FromImage(target))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            target.Save(thumbImagePath);
                        }
                    }
                }
                return thumbImagePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<FileUrlResponce> SendMyFileToS3(IFormFile file, string bucketName, string subDirectoryInBucket, bool isVedio, string accessKey, string accessSecret, string baseUrl)
        {
            try
            {
                baseUrl = baseUrl.Trim();
                FileUrlResponce reponse = new FileUrlResponce();
                var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                using (var source = file.OpenReadStream())
                {
                    source.Position = 0; Stream thumbStream = new MemoryStream(); source.CopyTo(thumbStream); var client = new AmazonS3Client(accessKey, accessSecret, RegionEndpoint.USEast1); PutObjectRequest putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = subDirectoryInBucket + "/" + FileName,
                        InputStream = source,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.PublicRead
                    }; PutObjectResponse response = await client.PutObjectAsync(putRequest);

                    reponse.URL = baseUrl + putRequest.Key;

                    if (!isVedio)
                    {
                        thumbStream.Seek(0, SeekOrigin.Begin);
                        thumbStream.Position = 0;

                        Bitmap srcBmp = new Bitmap(thumbStream);
                        float ratio = 1;
                        float minSize = Math.Min(256, 256);

                        if (srcBmp.PropertyIdList.Contains(0x112)) //0x112 = Orientation
                        {
                            var prop = srcBmp.GetPropertyItem(0x112);
                            if (prop.Type == 3 && prop.Len == 2)
                            {
                                UInt16 orientationExif = BitConverter.ToUInt16(srcBmp.GetPropertyItem(0x112).Value, 0);
                                if (orientationExif == 8)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }
                                else if (orientationExif == 3)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }
                                else if (orientationExif == 6)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                }
                            }
                        }

                        if (srcBmp.Width > srcBmp.Height)
                        {
                            ratio = minSize / (float)srcBmp.Width;
                        }
                        else
                        {
                            ratio = minSize / (float)srcBmp.Height;
                        }

                        SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                        Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);


                        Stream saveableThumbStream = new MemoryStream();

                        using (Graphics graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                            graphics.Dispose();

                            target.Save(saveableThumbStream, ImageFormat.Png);
                        }

                        PutObjectRequest putRequestThumb = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = subDirectoryInBucket + "/thumb_" + FileName,
                            InputStream = saveableThumbStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        PutObjectResponse responseThumb = await client.PutObjectAsync(putRequestThumb);
                        reponse.ThumbnailUrl = baseUrl + putRequestThumb.Key;
                    }
                    else
                    {
                        
                        reponse.ThumbnailUrl = baseUrl + putRequest.Key;
                    }

                }
                return reponse;
            }
            catch (Exception ex) { throw ex; }
        }


        public async Task<FileUrlResponce> SendMyFileToS33(IFormFile file, string bucketName, string subDirectoryInBucket, bool isVedio, string accessKey, string accessSecret, string baseUrl)
        {
            try
            {
                baseUrl = baseUrl.Trim();
                FileUrlResponce reponse = new FileUrlResponce();
                var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                using (var source = file.OpenReadStream())
                {
                    source.Position = 0; Stream thumbStream = new MemoryStream(); source.CopyTo(thumbStream); var client = new AmazonS3Client(accessKey, accessSecret, RegionEndpoint.USEast1); PutObjectRequest putRequest = new PutObjectRequest
                    {
                        BucketName = bucketName,
                        Key = subDirectoryInBucket + "/" + FileName,
                        InputStream = source,
                        ContentType = file.ContentType,
                        CannedACL = S3CannedACL.PublicRead
                    }; PutObjectResponse response = await client.PutObjectAsync(putRequest);

                    reponse.URL = baseUrl + putRequest.Key;

                    if (!isVedio)
                    {
                        thumbStream.Seek(0, SeekOrigin.Begin);
                        thumbStream.Position = 0;

                        Bitmap srcBmp = new Bitmap(thumbStream);
                        float ratio = 1;
                        float minSize = Math.Min(256, 256);

                        if (srcBmp.PropertyIdList.Contains(0x112)) //0x112 = Orientation
                        {
                            var prop = srcBmp.GetPropertyItem(0x112);
                            if (prop.Type == 3 && prop.Len == 2)
                            {
                                UInt16 orientationExif = BitConverter.ToUInt16(srcBmp.GetPropertyItem(0x112).Value, 0);
                                if (orientationExif == 8)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }
                                else if (orientationExif == 3)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }
                                else if (orientationExif == 6)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                }
                            }
                        }

                        if (srcBmp.Width > srcBmp.Height)
                        {
                            ratio = minSize / (float)srcBmp.Width;
                        }
                        else
                        {
                            ratio = minSize / (float)srcBmp.Height;
                        }

                        SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                        Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);


                        Stream saveableThumbStream = new MemoryStream();

                        using (Graphics graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                            graphics.Dispose();

                            target.Save(saveableThumbStream, ImageFormat.Png);
                        }

                        PutObjectRequest putRequestThumb = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = subDirectoryInBucket + "/thumb_" + FileName,
                            InputStream = saveableThumbStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        PutObjectResponse responseThumb = await client.PutObjectAsync(putRequestThumb);
                        reponse.ThumbnailUrl = baseUrl + putRequestThumb.Key;
                    }
                    else
                    {

                        thumbStream.Seek(0, SeekOrigin.Begin);
                        thumbStream.Position = 0;
                        var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        ffMpeg.GetVideoThumbnail(reponse.URL, "video_thumbnail.jpg");


                        //Random random = new Random();
                        //string randomeNumber = Guid.NewGuid().ToString() + random.Next(1, 9999999);
                        //Bitmap srcBmp = GetThumbnail(baseUrl + putRequest.Key, randomeNumber);

                        Bitmap srcBmp = new Bitmap(thumbStream);
                        float ratio = 1;
                        float minSize = Math.Min(256, 256);
                        if (srcBmp.PropertyIdList.Contains(0x112)) //0x112 = Orientation
                        {
                            var prop = srcBmp.GetPropertyItem(0x112);
                            if (prop.Type == 3 && prop.Len == 2)
                            {
                                UInt16 orientationExif = BitConverter.ToUInt16(srcBmp.GetPropertyItem(0x112).Value, 0);
                                if (orientationExif == 8)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }
                                else if (orientationExif == 3)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }
                                else if (orientationExif == 6)
                                {
                                    srcBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                }
                            }
                        }

                        if (srcBmp.Width > srcBmp.Height)
                        {
                            ratio = minSize / (float)srcBmp.Width;
                        }
                        else
                        {
                            ratio = minSize / (float)srcBmp.Height;
                        }

                        SizeF newSize = new SizeF(srcBmp.Width * ratio, srcBmp.Height * ratio);
                        Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);


                        Stream saveableThumbStream = new MemoryStream();

                        using (Graphics graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                            graphics.Dispose();

                            target.Save(saveableThumbStream, ImageFormat.Png);
                        }

                        PutObjectRequest putRequestThumb = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = subDirectoryInBucket + "/thumb_" + FileName,
                            InputStream = saveableThumbStream,
                            ContentType = file.ContentType,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        PutObjectResponse responseThumb = await client.PutObjectAsync(putRequestThumb);
                        reponse.ThumbnailUrl = baseUrl + putRequestThumb.Key;

                        //reponse.ThumbnailUrl = baseUrl + putRequest.Key;
                    }

                }
                return reponse;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<FileUrlResponce> sendMylocalFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3, string baseUrl)
        {
            try
            {
                var client = new AmazonS3Client("AKIASQU5J7MAMNVRFTBW", "sPDtqvLHigRt9HOEbjUUwk21++o1E3xwpsKDrMvW", Amazon.RegionEndpoint.EUCentral1);
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = "waseetemirates",
                    Key = fileNameInS3,
                    FilePath = localFilePath,
                    CannedACL = S3CannedACL.PublicRead
                };
                PutObjectResponse response = await client.PutObjectAsync(putRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    // this model is up to you, in my case I have to use it following;
                    return new FileUrlResponce
                    {
                        URL = baseUrl + putRequest.Key
                    };
                }
                else
                {
                    // this model is up to you, in my case I have to use it following;
                    return new FileUrlResponce
                    {
                        URL = baseUrl + putRequest.Key
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static FileUrlResponce SaveLocalImage(IFormFile file)
        {
            try
            {
                FileUrlResponce image = new FileUrlResponce();

                if (file.Length > 0)
                {
                    string randomeNumber = Guid.NewGuid().ToString();
                    string randomeNumber1 = Guid.NewGuid().ToString();
                    var folderPath = randomeNumber;
                    var folderPathThumnail = randomeNumber1;

                    var filePath = folderPath;
                    Directory.CreateDirectory(filePath);
                    Directory.CreateDirectory(folderPathThumnail);
                    var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    string thisThumbnailFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N") + "_Thumbnail" + Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    image.URL = Path.Combine(folderPath, FileName).Replace(@"\", "/");
                    string thumbnailUrl = GenerateThumbImage(Path.Combine(filePath, FileName), Path.Combine(folderPathThumnail, thisThumbnailFileName), 160, 250);
                    image.ThumbnailUrl = Path.Combine(folderPathThumnail, thisThumbnailFileName).Replace("\\", "/");
                }

                return image;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Bitmap GetThumbnail(string video, string thumbnail)
        {
            var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(5000);

            return LoadImage(thumbnail);
        }

        static Bitmap LoadImage(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path));
            return (Bitmap)Image.FromStream(ms);
        }


    }
}