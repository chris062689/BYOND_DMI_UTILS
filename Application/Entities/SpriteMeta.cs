using DMISharp;
using SixLabors.Primitives;
using System.Collections.Generic;

namespace FS.Utilities.Application.Entities
{
    public class SpriteMeta
    {
        public Point IconSize { get; set; } = new Point(32, 32);
        public int? AnimationDelay { get; set; } = null;
        public int? IconStandingFrame { get; set; } = null;
        public Point SpacingBetweenSize { get; set; } = new Point(0, 0);
        public int FramesPerDirection { get; set; } = 1;
        public DirectionDepth IconDirectionDepth { get; set; } = DirectionDepth.One;
        public IEnumerable<StateDirection> DirectionFlow { get; set; } = new List<StateDirection>();
        public IEnumerable<KeyValuePair<StateDirection, Point>> OffsetPixelsDirections { get; set; } = new List<KeyValuePair<StateDirection, Point>>();
    }
}
