using Hilal.Common;
using Hilal.DataViewModel.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hilal.Api.Models
{
    public static class CommonFunctions
    {
        public static Bitmap WatermarkImage(Bitmap image, Bitmap watermark)
        {
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                watermark.SetResolution(imageGraphics.DpiX, imageGraphics.DpiY);

                int x = (image.Width - watermark.Width) / 2;
                int y = (image.Height - watermark.Height) / 2;

                imageGraphics.DrawImage(watermark, x, y, watermark.Width, watermark.Height);

            }

            return image;
        }

        public static async Task<FileUrlResponce> GetWaterMarkImage(string fileee, IWebHostEnvironment _env)
        {
            var path = "7413e660-fbb4-49c6-babe-d5bd259a0ce2/20210706050014389_0da93d6f5d464668a8d05b8d8aef209f.png";
            //throw new Exception(fileee);
            using (Image watermarkImage = Image.FromFile("sold.png"))
            using (Image image = Image.FromFile(fileee))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                string randomeNumber = Guid.NewGuid().ToString();
                image.Save(randomeNumber + ".jpg");

                var file = await new SaveFiles().sendMylocalFileToS3(randomeNumber + ".jpg", "waseetemirates", "Category" + SystemGlobal.GetSubDirectoryName(), randomeNumber + ".jpg", "https://waseetemirates.s3.eu-central-1.amazonaws.com/");
                //var FileTos3 = await AmazonServices.sendMyFileToS3(DirectoryPath, "", "PaySlip", randomeNumber);
                if (System.IO.File.Exists(randomeNumber + ".jpg"))
                {
                    System.IO.File.Delete(randomeNumber + ".jpg");
                }
                return file;
            }
        }

        public static void try1()
        {
            //string root = HttpContext.Current.Server.MapPath("pp.jpg");
            System.Drawing.Image bitmap = (System.Drawing.Image)Bitmap.FromFile("pp.jpg"); // set image     
            System.Drawing.Font font = new Font("Arial", 20, FontStyle.Italic, GraphicsUnit.Pixel);
            Color color = Color.FromArgb(255, 255, 0, 0);
            Point atpoint = new Point(bitmap.Width / 2, bitmap.Height / 2);
            SolidBrush brush = new SolidBrush(color);
            Graphics graphics = Graphics.FromImage(bitmap);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            graphics.DrawString("Sold", font, brush, atpoint, sf);
            graphics.Dispose();
            MemoryStream m = new MemoryStream();
            bitmap.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] convertedToBytes = m.ToArray();
            string randomeNumber = Guid.NewGuid().ToString();
            //string saveto = HttpContext.Current.Server.MapPath("~/Images/test.jpg");
            System.IO.File.WriteAllBytes(randomeNumber + ".jpg", convertedToBytes);
        }
    }
}
