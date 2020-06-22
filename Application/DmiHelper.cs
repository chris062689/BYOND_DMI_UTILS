using DMISharp;
using FS.Utilities.Application.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System.Linq;

namespace FS.Utilities.Application
{
    public static class DmiHelper
    {
        public static DMIState CreateDmiState(string name, SpriteMeta meta, int? frames)
        {
            return new DMIState(name, meta.IconDirectionDepth, frames ?? meta.FramesPerDirection, meta.IconSize.X, meta.IconSize.Y);
        }

        public static void SetFrame(DMIState state, Image<Rgba32> image, DirectionDepth depth, StateDirection direction, int frame)
        {
            if (depth == DirectionDepth.One)
            {
                state.SetFrame(image, frame);
            }
            else
            {
                state.SetFrame(image, direction, frame);
            }
        }

        public static Rectangle CreateRectangle(StateDirection direction, Point currentPosition, SpriteMeta meta)
        {
            var newPosition = CreateNewPoint(direction, currentPosition, meta);
            return new Rectangle(newPosition.X, newPosition.Y, meta.IconSize.X, meta.IconSize.Y);
        }

        public static Point CreateNewPoint(StateDirection direction, Point currentPosition, SpriteMeta meta)
        {
            var offsetPixelDirection = meta.OffsetPixelsDirections.Where(x => x.Key == direction).Select(y => y.Value).First();
            return new Point(offsetPixelDirection.X + currentPosition.X + meta.SpacingBetweenSize.X, offsetPixelDirection.Y + currentPosition.Y + meta.SpacingBetweenSize.Y);
        }
    }
}
