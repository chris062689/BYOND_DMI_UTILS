using DMISharp;
using FS.Utilities.Application.Entities;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

namespace FS.Utilities.Application
{
    public static class SpriteMetaRepository
    {
        private static IEnumerable<StateDirection> CommonRpgMakerStateDirection_Four = new List<StateDirection>() { StateDirection.South, StateDirection.West, StateDirection.East, StateDirection.North };
        private static IEnumerable<StateDirection> CommonRpgMakerStateDirection_One = new List<StateDirection>() { StateDirection.South };

        private static List<KeyValuePair<StateDirection, Point>> CommonOffsetPixelDirections = new List<KeyValuePair<StateDirection, Point>>()
            {
                new KeyValuePair<StateDirection, Point>(StateDirection.South, new Point(0, 0)),
                new KeyValuePair<StateDirection, Point>(StateDirection.West, new Point(0, 0)),
                new KeyValuePair<StateDirection, Point>(StateDirection.East, new Point(0, 0)),
                new KeyValuePair<StateDirection, Point>(StateDirection.North, new Point(0, 0))
            };

        private static SpriteMeta GenericTile_32 = new SpriteMeta()
        {
            IconSize = new Point(32, 32)
        };
        private static SpriteMeta GenericTile_48 = new SpriteMeta()
        {
            IconSize = new Point(48, 48)
        };
        private static SpriteMeta GenericTile_64 = new SpriteMeta()
        {
            IconSize = new Point(64, 64)
        };

        public static SpriteMeta GetMetaForFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (string.IsNullOrEmpty(filePath) == false)
            {
                return new SpriteMeta()
                {
                    IconSize = new Point(32, 32),
                    AnimationDelay = 2,
                    IconStandingFrame = 2,
                    SpacingBetweenSize = new Point(0, 0),
                    IconDirectionDepth = DirectionDepth.Four,
                    DirectionFlow = CommonRpgMakerStateDirection_Four,
                    FramesPerDirection = 3,
                    OffsetPixelsDirections = CommonOffsetPixelDirections
                };
            }
            else
            {
                throw new Exception($"File '{fileName}' cannot be translated to sprite metadata.");
            }
        }
    }
}
