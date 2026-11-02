using System.Drawing;

namespace ScreenshotHelp
{
    public class ImageComparer
    {
        public bool AreImagesSame(string image1Path, string image2Path)
        {
            using (Bitmap image1 = new Bitmap(image1Path))
            using (Bitmap image2 = new Bitmap(image2Path))
            {
                // Size different hai to image same nahi hai
                if (image1.Width != image2.Width ||
                    image1.Height != image2.Height)
                {
                    return false;
                }

                // Har pixel compare karega
                for (int x = 0; x < image1.Width; x++)
                {
                    for (int y = 0; y < image1.Height; y++)
                    {
                        if (image1.GetPixel(x, y) != image2.GetPixel(x, y))
                        {
                            return false;
                        }
                    }
                }

                // Sabhi pixels same hain
                return true;
            }
        }
    }
}