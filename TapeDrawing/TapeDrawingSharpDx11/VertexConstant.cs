using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpDX;

namespace TapeDrawingSharpDx11
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexConstant
    {
        public float Width;
        public float Height;
        public float XFrom;
        public float XTo;
        public float YFrom;
        public float YTo;
        public float r1;
        public float r2;
        
        public Matrix World;
        public Matrix Translate;
       
    }
}
