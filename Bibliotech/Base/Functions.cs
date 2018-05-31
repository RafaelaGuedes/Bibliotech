﻿using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.QrCode;

namespace Bibliotech.Base
{
    public class Functions
    {
        public static bool IsNull(object obj)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null);
        }

        public static bool IsNullExcludingProperties(object obj, params string[] properties)
        {
            if (obj == null)
                return true;
            return !obj.GetType().GetProperties().Any(p => p.GetValue(obj) != null && !properties.Contains(p.Name));
        }

        public static MemoryStream GenerateQRCode(string content, int width = -1, int height = -1)
        {
            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

            if (width > 0 && height > 0)
                writer.Options = new QrCodeEncodingOptions { Width = width, Height = height };

            var result = writer.Write(content);

            MemoryStream memoryStream = new MemoryStream();
            result.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            return memoryStream;
        }

        public static MemoryStream ConvertHtmlToImage(string htmlContent, string format, string arguments = null)
        {
            HtmlToImageConverter htmlConverter = new HtmlToImageConverter();

            if (arguments != null)
                htmlConverter.CustomArgs = arguments;

            var imgBytes = htmlConverter.GenerateImage(htmlContent, format);
            return new MemoryStream(imgBytes);
        }

        public static string GetPngImageSrc(MemoryStream memoryStream)
        {
            return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
        }
    }
}