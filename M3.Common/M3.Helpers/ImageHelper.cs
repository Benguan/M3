using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using M3.Models;

namespace M3.Helpers
{
    public class ImageHelper
    {
        public static void GetThumbnail(int maxWidth, int maxHeight, string originFilePath, string saveFilePath, bool isOverride)
        {
            var saveFileInfo = new FileInfo(saveFilePath);
            if (isOverride || !saveFileInfo.Exists)
            {
                var sourceImage = Image.FromFile(originFilePath);
                var originWidth = sourceImage.Width;
                var originHeight = sourceImage.Height;
                var finalWidth = 0;
                var finalHeight = 0;
                if ((double)originWidth / originHeight < (double)maxWidth / maxHeight)
                {

                    finalWidth = (int)((double)originWidth / originHeight * maxHeight);
                    finalHeight = maxHeight;
                }
                else
                {
                    finalWidth = maxWidth;
                    finalHeight = (int)((double)originHeight / originWidth * maxWidth);
                }

                var srcRect = new Rectangle(0, 0, originWidth, originHeight);
                var destRect = new Rectangle(0, 0, finalWidth, finalHeight);

                var finalImage = new Bitmap(finalWidth, finalHeight);

                var graphics = Graphics.FromImage(finalImage);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //设置高质量,低速度呈现平滑程度
                graphics.Clear(Color.Transparent); //清空画布并以透明背景色填充
                graphics.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);
                graphics.Dispose();
                var qualityValue = 100; //图像质量 1-100的范围
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                foreach (var codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                    {
                        encoder = codec;
                    }
                }
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)qualityValue);

                if (!saveFileInfo.Directory.Exists)
                {
                    saveFileInfo.Directory.Create();
                }

                finalImage.Save(saveFilePath, encoder, encoderParameters);

                finalImage.Dispose();
                sourceImage.Dispose();
            }
        }
        public static void CropImage(int originWidth, int originHeight, int startX, int startY, int width, int height, int finalWidth, int finalHeight, string originFilePath, string saveFilePath, bool allowTransparent)
        {
            var sourceImage = Image.FromFile(originFilePath);
            var ratio = (double)sourceImage.Width / originWidth;
            var srcRect = new Rectangle((int)(startX * ratio), (int)(startY * ratio), (int)(width * ratio), (int)(height * ratio));
            var destRect = new Rectangle(0, 0, finalWidth, finalHeight);

            var finalImage = new Bitmap(finalWidth, finalHeight);
            var graphics = Graphics.FromImage(finalImage);
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; //设置高质量插值法
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; //设置高质量,低速度呈现平滑程度
            graphics.Clear(Color.Transparent); //清空画布并以透明背景色填充
            graphics.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);

            if (allowTransparent)
            {
                finalImage.Save(saveFilePath, ImageFormat.Png);
            }
            else
            {
                var qualityValue = 100; //图像质量 1-100的范围
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                foreach (var codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                    {
                        encoder = codec;
                    }
                }
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)qualityValue);

                finalImage.Save(saveFilePath, encoder, encoderParameters);
            }

            finalImage.Dispose();
            sourceImage.Dispose();

            if (System.IO.File.Exists(originFilePath))
            {
                try
                {
                    System.IO.File.Delete(originFilePath);
                }
                catch { }
            }
        }

        public static Category GetGallery(int id)
        {
            return null;
        }
    }
}
