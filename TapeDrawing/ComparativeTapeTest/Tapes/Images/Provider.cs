using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ComparativeTapeTest.Tapes.Images
{
    static class Provider
    {
        /// <summary>
        /// Возвращает изображение по названию.
        /// </summary>
        /// <param name="p_name">Название .bmp файла.</param>
        /// <returns></returns>
        public static Image GetResource(string p_name)
        {
            if (p_name == null)
                return null;

            string resourceName = string.Format("{0}.bmp", p_name);
            return ToolboxBitmapAttribute.GetImageFromResource(typeof(Provider), resourceName, false);
        }

        public static Stream GetStream(string p_name)
        {
            var ms = new MemoryStream();
            GetResource(p_name).Save(ms, ImageFormat.Png);
            return ms;
        }
    }
}
