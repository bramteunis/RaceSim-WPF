using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace UserInterface
{
    public static class Images
    {
        private static Dictionary<string,Bitmap> cache = new Dictionary<string, Bitmap>();
        public static Bitmap LoadBitmap(string url) {
            if (!cache.ContainsKey(url))
            {
                cache.Add(url, new Bitmap(url));
            }
            return cache[url];
        }
        public static void ClearBitmap() {
            cache.Clear();
        }
        public static Bitmap MakeBitmap(int x, int y)
        {
            // Creates empty bitmap
            string Cachekey = "empty";
            if (cache.ContainsKey(Cachekey) == false)
            {
                cache.Add(Cachekey, new Bitmap(x, y));
                Graphics graphics = Graphics.FromImage(cache[Cachekey]);
                graphics.FillRectangle(new SolidBrush(System.Drawing.Color.Black), 0, 0, x, y);
            }

            return (Bitmap)cache[Cachekey].Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
