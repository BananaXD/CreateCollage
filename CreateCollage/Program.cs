using System.Drawing;
using ImageMagick;

namespace CreateCollage {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");


            string dir = @"C:\Users\ometz\Downloads\Telegram Desktop\Andalucia full menu pics";
            string[] files = Directory.GetFiles(dir);
            files = files.OrderBy(file => int.Parse(Path.GetFileNameWithoutExtension(file))).ToArray();

            var Images = files.Select(file => new MagickImage(file));

            var baseImage = Images.First();

            int base_width = baseImage.Width;
            int base_height = baseImage.Height;

            int nImagesWidth = 6;
            int nImagesHeight = Images.Count() / nImagesWidth;

            int margin = 15;

            int width = (base_width * nImagesWidth) + (margin * (nImagesWidth + 1));
            int height = (base_height * nImagesHeight) + (margin * (nImagesHeight + 1));

            bool fFlip = false;
            int[] toFlip = { 0, 1, 6, 7, 8, 9, 10, 16 };

            MagickGeometry size = new(width, height);
            MagickImage outputImage = new(new MagickColor("#4287f5"), width, height);

            int iImage = 0;
            for (int iRow = 0; iRow < nImagesHeight; iRow++) {
                for (int iCol = 0; iCol < nImagesWidth; iCol++) {
                    Console.Write("\rAt image: {0}/{1}", iImage, Images.Count());
                    var image = Images.ElementAt(iImage);

                    if (base_width > image.Width) {
                        image.Resize(base_width, base_height);
                    }
                    else if (base_width < image.Width) {
                        image.Resize(base_width, base_height);
                    }

                    if (iImage is 9 or 13) {
                        image.Rotate(180);
                    }

                    if (fFlip && toFlip.Contains(iImage)) {
                        image.Flip();
                        image.Rotate(180);
                    }

                    double x = margin + (iCol * base_width) + (iCol * margin);
                    double y = margin + (iRow * base_height) + (iRow * margin);

                    outputImage.Draw(new DrawableComposite(x, y, image));

                    iImage++;
                }
            }

            outputImage.Write("outputImage5.png");
        }
    }
}