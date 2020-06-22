using DMISharp;
using FS.Utilities.Application;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Linq;

namespace FS.Utilities.CharSprites
{
    class Program
    {
        /// <summary>
        /// Folder path where the character sheets are located.
        /// </summary>
        const string FolderPath = @"";
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

                // Create a new DMI file.
                using (var newDMI = new DMIFile(meta.IconSize.X, meta.IconSize.Y))
                {
                    // Create an initial blank state for each icon file.
                    var blankState = new DMIState(string.Empty, meta.IconDirectionDepth, meta.FramesPerDirection, meta.IconSize.X, meta.IconSize.Y);

                    foreach (var direction in meta.DirectionFlow)
                    {
                        for (int i = 0; i < meta.FramesPerDirection; i++)
                        {
                            DmiHelper.SetFrame(blankState, new Image<Rgba32>(meta.IconSize.X, meta.IconSize.Y), meta.IconDirectionDepth, direction, i);
                        }
                    }
                    newDMI.AddState(blankState);

                    // Create state
                    var img = Image.Load<Rgba32>(filePath);

                    int spriteNumber = 0;
                    var currentPosition = new Point(0, 0);

                    bool eof = false;
                    while (eof == false)
                    {
                        var iconState = $"c_{spriteNumber}";

                        // Create the standing state.
                        DMIState standingState = null;
                        if (meta.IconStandingFrame != null)
                        {
                            standingState = DmiHelper.CreateDmiState(iconState, meta, 1);
                            newDMI.AddState(standingState);
                        }

                        // Create the animation state.
                        DMIState animationState = null;
                        if (meta.AnimationDelay.HasValue)
                        {
                            animationState = DmiHelper.CreateDmiState(iconState, meta, meta.FramesPerDirection);
                            animationState.SetDelay(Enumerable.Repeat((double)meta.AnimationDelay, meta.FramesPerDirection).ToArray(), 0, -1);
                            animationState.SetMovement(true);

                            newDMI.AddState(animationState);
                        }

                        // For each direction, extract the sprite.
                        foreach (var direction in meta.DirectionFlow)
                        {
                            if (eof) { break; }

                            // For each frame in each direction, extract the sprite.
                            for (int frame = 0; frame < meta.FramesPerDirection; frame++)
                            {
                                var area = DmiHelper.CreateRectangle(direction, currentPosition, meta);
                                if (area.Y + area.Height > img.Height)
                                {
                                    Console.WriteLine($"EOF Reached.");

                                    // Rewind the DMI stack to remove these states.
                                    newDMI.RemoveState(standingState);
                                    newDMI.RemoveState(animationState);

                                    eof = true;
                                    break;
                                }

                                Console.WriteLine($"{spriteNumber} {direction} (M) Frame {frame} - {area.X}, {area.Y} to {area.X + area.Width},{area.Y + area.Height}");

                                var newImage = img.Clone();
                                newImage.Mutate(x => x.Crop(area));

                                if (meta.AnimationDelay.HasValue)
                                {
                                    DmiHelper.SetFrame(animationState, newImage, meta.IconDirectionDepth, direction, frame);
                                }

                                if (meta.IconStandingFrame.HasValue && meta.IconStandingFrame == frame + 1)
                                {
                                    DmiHelper.SetFrame(standingState, newImage.Clone(), meta.IconDirectionDepth, direction, 0);
                                }

                                currentPosition.X += meta.IconSize.X + meta.SpacingBetweenSize.X;
                            }

                            if (currentPosition.X >= img.Width)
                            {
                                currentPosition.X = 0;
                                currentPosition.Y += meta.IconSize.Y + meta.SpacingBetweenSize.Y;
                            }
                        }

                        spriteNumber += 1;
                    }

                    // Saving
                    newDMI.Save(destinationFilePath);
                }
            }
        }
    }
}
