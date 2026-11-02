using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace ScreenshotHelp
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\Screenshots";

            Directory.CreateDirectory(folderPath);

            ImageComparer comparer = new ImageComparer();

            while (true)
            {
                try
                {
                    // ==========================================
                    // 1. TAKE 6 SCREENSHOTS
                    // ==========================================

                    Console.WriteLine("Starting new batch...");

                    for (int i = 1; i <= 6; i++)
                    {
                        string fileName =
                            "Screenshot_" +
                            DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                            ".png";

                        string filePath =
                            Path.Combine(folderPath, fileName);

                        Rectangle screen =
                            Screen.PrimaryScreen.Bounds;

                        using (Bitmap bitmap =
                            new Bitmap(
                                screen.Width,
                                screen.Height))
                        {
                            using (Graphics graphics =
                                Graphics.FromImage(bitmap))
                            {
                                graphics.CopyFromScreen(
                                    screen.Left,
                                    screen.Top,
                                    0,
                                    0,
                                    screen.Size);
                            }

                            bitmap.Save(
                                filePath,
                                ImageFormat.Png);
                        }

                        Console.WriteLine(
                            "Screenshot " + i +
                            " saved: " + filePath);

                        // 5 minute wait
                        Thread.Sleep(1 * 60 * 1000);
                    }


                    // ==========================================
                    // 2. GET ALL 6 IMAGES
                    // ==========================================

                    string[] images =
                        Directory.GetFiles(
                            folderPath,
                            "Screenshot_*.png");

                    Console.WriteLine(
                        "Total images: " + images.Length);


                    // ==========================================
                    // 3. FIND DUPLICATES
                    // ==========================================

                    bool[] duplicate =
                        new bool[images.Length];

                    for (int i = 0; i < images.Length; i++)
                    {
                        if (duplicate[i])
                            continue;

                        for (int j = i + 1;
                             j < images.Length;
                             j++)
                        {
                            if (duplicate[j])
                                continue;

                            Console.WriteLine(
                                "Comparing image " +
                                (i + 1) +
                                " with image " +
                                (j + 1));

                            bool same =
                                comparer.AreImagesSame(
                                    images[i],
                                    images[j]);

                            if (same)
                            {
                                Console.WriteLine(
                                    "Duplicate found: " +
                                    images[j]);

                                duplicate[j] = true;
                            }
                        }
                    }


                    // ==========================================
                    // 4. SEND ONLY UNIQUE IMAGES
                    // ==========================================

                    WhatsAppSender sender =
                        new WhatsAppSender();

                    for (int i = 0;
                         i < images.Length;
                         i++)
                    {
                        if (!duplicate[i])
                        {
                            Console.WriteLine(
                                "Unique image: " +
                                images[i]);

                            sender.SendImage(images[i]);
                        }
                    }


                    // ==========================================
                    // 5. CLEAN FOLDER
                    // ==========================================

                    foreach (string image in images)
                    {
                        if (File.Exists(image))
                        {
                            File.Delete(image);
                        }
                    }
                     
                    Console.WriteLine(
                        "All screenshots deleted.");

                    Console.WriteLine(
                        "Starting next batch...");
                }
                catch (Exception ex)
                {
                    File.WriteAllText(
                        @"C:\Screenshots\HelperError.txt",
                        ex.ToString());

                    Console.WriteLine(
                        "Error: " + ex.Message);
                }
            }
        }
    }
}