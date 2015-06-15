using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AForge.Imaging;
using System.Threading.Tasks;

namespace SimilarityFinder
{
    public class ImageData : IDisposable
    {
        private Bitmap backup = null;
        private Bitmap image = null;

        public int Width
        {
            get
            {
                return image == null ? -1 : image.Width;
            }
        }

        public int Height
        {
            get
            {
                return image == null ? -1 : image.Height;
            }
        }

        public Bitmap Backup
        {
            get
            {
                return backup;
            }
        }

        public Bitmap Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        public ImageData(string fileName)
        {
            try
            {
                image = (Bitmap.FromFile(fileName) as Bitmap);
                image = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                backup = image.Clone(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public void RestoreOriginal()
        {
            image = backup.Clone() as Bitmap;
        }

        public void Dispose()
        {
            image.Dispose();
            backup.Dispose();
        }
    }
}
