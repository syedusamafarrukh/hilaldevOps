//using GroupDocs.Watermark;
//using GroupDocs.Watermark.Watermarks;
using Hilal.DataViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hilal.Common
{
    public static class CommonFunctions
    {
        //public static void GetWaterMarkImage(string filePath)
        //{
        //    using (Watermarker watermarker = new Watermarker("pp.jpg"))
        //    {
        //        // Set the Text and Watermark Font
        //        Font font = new Font("Arial", 30, FontStyle.Bold | FontStyle.Italic);
        //        TextWatermark watermark = new TextWatermark("Sold", font);

        //        // Set Watermark Properties
        //        watermark.ForegroundColor = Color.Black;
        //        watermark.TextAlignment = TextAlignment.Right;
        //        watermark.X = 700;
        //        watermark.Y = 700;
        //        watermark.RotateAngle = -70;
        //        watermark.Opacity = 0.4;
        //        // watermark.BackgroundColor = Color.Blue;

        //        // Add the configured watermark to JPG Image
        //        string randomeNumber = Guid.NewGuid().ToString();
        //        watermarker.Add(watermark);
        //        watermarker.Save(randomeNumber +".jpg");
        //    }

        //}

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

        public static async Task<FileUrlResponce> GetWaterMarkImage(string fileee)
        {
            using (Image image = Image.FromFile(fileee))
            using (Image watermarkImage = Image.FromFile("sold.png"))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
                string randomeNumber = Guid.NewGuid().ToString();
                image.Save(randomeNumber + ".jpg");

                var file = await new SaveFiles().sendMylocalFileToS3(randomeNumber + ".jpg", "alhalalmarket", "Category" + SystemGlobal.GetSubDirectoryName(), randomeNumber + ".jpg", "https://alhalalmarket.s3.eu-central-1.amazonaws.com/");
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
