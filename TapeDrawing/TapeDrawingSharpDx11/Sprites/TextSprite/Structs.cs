using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;

namespace TapeDrawingSharpDx11.Sprites.TextSprite
{
    internal class SpriteVertexLayout
    {
        
        internal struct Struct
        {
            internal Vector2 TexCoord;
            internal Vector2 TexCoordSize;
            internal int Color;
            internal Vector2 TopLeft;
            internal Vector2 TopRight;
            internal Vector2 BottomLeft;
            internal Vector2 BottomRight;

            internal static int SizeInBytes { get { return Marshal.SizeOf(typeof(Struct)); } }
        }
    }

    /// <summary>
    /// This structure holds data for sprites with a specific texture
    /// </summary>
    internal class SpriteSegment
    {
        
        internal ShaderResourceView Texture;
        internal List<SpriteVertexLayout.Struct> Sprites = new List<SpriteVertexLayout.Struct>();
    }

    internal class CharTableDescription
    {
        /// <summary>
        /// A Texture2D
        /// </summary>
        internal Texture2D Texture;
        internal ShaderResourceView SRV;
        internal CharDescription[] Chars = new CharDescription[256];
    }

    internal class CharDescription
    {
        /// <summary>
        /// Size of the char excluding overhangs
        /// </summary>
        internal Vector2 CharSize;
        internal float OverhangLeft, OverhangRight, OverhangTop, OverhangBottom;

        internal Vector2 TexCoordsStart;
        internal Vector2 TexCoordsSize;

        internal CharTableDescription TableDescription;

        internal StringMetrics ToStringMetrics(Vector2 position)
        {
            return new StringMetrics
            {
                TopLeft = position,
                Size = new Vector2(CharSize.X, CharSize.Y),
                OverhangTop = OverhangTop,
                OverhangBottom = OverhangBottom,
                OverhangLeft = OverhangLeft,
                OverhangRight = OverhangRight,
            };
        }
    }    
}
