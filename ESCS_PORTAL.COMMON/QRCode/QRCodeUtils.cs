using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ESCS_PORTAL.COMMON.QRCode
{
    public class QRCodeUtils
    {
        public static void GenerateQRCode(string text, string pathFile, int? level = 0)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(text, GetLevel(level));
            QrCodeInfo.SaveRawData(pathFile, QRCodeData.Compression.Uncompressed);
        }
        private static QRCodeGenerator.ECCLevel GetLevel(int? level)
        {
            switch (level)
            {
                case 1:
                    return QRCodeGenerator.ECCLevel.H;
                case 2:
                    return QRCodeGenerator.ECCLevel.L;
                case 3:
                    return QRCodeGenerator.ECCLevel.M;
                case 4:
                    return QRCodeGenerator.ECCLevel.Q;
                default:
                    return QRCodeGenerator.ECCLevel.L;
            }
        }
    }
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
