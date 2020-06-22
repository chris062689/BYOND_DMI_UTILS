using DMISharp;
using FS.Utilities.Application;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Linq;

namespace FS.Utilities.TurfSprites
{
    class TurfSprites
    {
        /// <summary>
        /// Folder path to RPG Maker tilesets we want to convert to a DMI file.
        /// </summary>
        private const string FolderPath = @"";
        static string FolderDmiOutputPath => Path.Combine(FolderPath, "dmi");

        static void Main(string[] args)
        {
            Directory.CreateDirectory(FolderDmiOutputPath);

            foreach (var filePath in Directory.EnumerateFiles(FolderPath, "*.png", SearchOption.TopDirectoryOnly))
            {
                string destinationFilePath = FolderDmiOutputPath + @$"\{Path.GetFileNameWithoutExtension(filePath)}.dmi";

                Console.WriteLine($"{filePath} -> {destinationFilePath}");

                // Obtain the sprite metadata for the file.
                var meta = SpriteMetaRepository.GetMetaForFile(filePath);

                using (var newDMI = new DMIFile(meta.IconSize.X, meta.IconSize.Y))
                {
                    var blankState = DmiHelper.CreateDmiState(string.Empty, meta, meta.FramesPerDirection);
                    blankState.SetFrame(new Image<Rgba32>(meta.IconSize.X, meta.IconSize.Y), 0);
                    newDMI.AddState(blankState);

                    // Create state
                    var img = Image.Load<Rgba32>(filePath);

                    var currentX = 0;
                    var currentY = 0;

                    int pos = 0;
                    while (currentX + meta.IconSize.X <= img.Width && currentY + meta.IconSize.Y <= img.Height)
                    {
                        var newImage = img.Clone();
                        newImage.Mutate(x => x.Crop(new Rectangle(currentX, currentY, meta.IconSize.X, meta.IconSize.Y)));

                        if (newImage.GetPixelSpan().ToArray().All(x => x.A == 0x00))
                        {
                            // Skip fully transparent sprites.
                        }
                        else
                        {
                            var newState = new DMIState($"i_{pos}", DirectionDepth.One, 1, meta.IconSize.X, meta.IconSize.Y);
                            newState.SetFrame(newImage, 0);

                            newDMI.AddState(newState);
                        }

                        pos += 1;
                        currentX += meta.IconSize.X;
                        if (currentX >= img.Width)
                        {
                            currentX = 0;
                            currentY += meta.IconSize.Y;
                        }
                    }

                    // Saving
                    newDMI.Save(destinationFilePath);
                }
            }

        }
    }
}
