using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TapeDrawing.Core.Primitives;

namespace TapeDrawingSharpDx11
{
    static class KeyConverter
    {
        static KeyConverter()
        {
            Dictionary.Add(18, KeyboardKey.Alt);
            Dictionary.Add(17, KeyboardKey.Control);
            Dictionary.Add(16, KeyboardKey.Shift);

            Dictionary.Add((int)Keys.Return, KeyboardKey.Return);
            Dictionary.Add((int)Keys.Escape, KeyboardKey.Escape);
            Dictionary.Add((int)Keys.Space, KeyboardKey.Space);
            Dictionary.Add((int)Keys.Delete, KeyboardKey.Delete);
            Dictionary.Add((int)Keys.Pause, KeyboardKey.Pause);

            Dictionary.Add((int)Keys.Up, KeyboardKey.Up);
            Dictionary.Add((int)Keys.Down, KeyboardKey.Down);
            Dictionary.Add((int)Keys.Right, KeyboardKey.Right);
            Dictionary.Add((int)Keys.Left, KeyboardKey.Left);

            Dictionary.Add((int)Keys.PageUp, KeyboardKey.PageUp);
            Dictionary.Add((int)Keys.PageDown, KeyboardKey.PageDown);
            Dictionary.Add((int)Keys.Home, KeyboardKey.Home);
            Dictionary.Add((int)Keys.End, KeyboardKey.End);

            Dictionary.Add((int)Keys.A, KeyboardKey.A);
            Dictionary.Add((int)Keys.B, KeyboardKey.B);
            Dictionary.Add((int)Keys.C, KeyboardKey.C);
            Dictionary.Add((int)Keys.D, KeyboardKey.D);
            Dictionary.Add((int)Keys.E, KeyboardKey.E);
            Dictionary.Add((int)Keys.F, KeyboardKey.F);
            Dictionary.Add((int)Keys.G, KeyboardKey.G);
            Dictionary.Add((int)Keys.H, KeyboardKey.H);
            Dictionary.Add((int)Keys.I, KeyboardKey.I);
            Dictionary.Add((int)Keys.J, KeyboardKey.J);
            Dictionary.Add((int)Keys.K, KeyboardKey.K);
            Dictionary.Add((int)Keys.L, KeyboardKey.L);
            Dictionary.Add((int)Keys.M, KeyboardKey.M);
            Dictionary.Add((int)Keys.N, KeyboardKey.N);
            Dictionary.Add((int)Keys.O, KeyboardKey.O);
            Dictionary.Add((int)Keys.P, KeyboardKey.P);
            Dictionary.Add((int)Keys.Q, KeyboardKey.Q);
            Dictionary.Add((int)Keys.R, KeyboardKey.R);
            Dictionary.Add((int)Keys.S, KeyboardKey.S);
            Dictionary.Add((int)Keys.T, KeyboardKey.T);
            Dictionary.Add((int)Keys.U, KeyboardKey.U);
            Dictionary.Add((int)Keys.V, KeyboardKey.V);
            Dictionary.Add((int)Keys.W, KeyboardKey.W);
            Dictionary.Add((int)Keys.X, KeyboardKey.X);
            Dictionary.Add((int)Keys.Y, KeyboardKey.Y);
            Dictionary.Add((int)Keys.Z, KeyboardKey.Z);

            Dictionary.Add((int)Keys.F1, KeyboardKey.F1);
            Dictionary.Add((int)Keys.F2, KeyboardKey.F2);
            Dictionary.Add((int)Keys.F3, KeyboardKey.F3);
            Dictionary.Add((int)Keys.F4, KeyboardKey.F4);
            Dictionary.Add((int)Keys.F5, KeyboardKey.F5);
            Dictionary.Add((int)Keys.F6, KeyboardKey.F6);
            Dictionary.Add((int)Keys.F7, KeyboardKey.F7);
            Dictionary.Add((int)Keys.F8, KeyboardKey.F8);
            Dictionary.Add((int)Keys.F9, KeyboardKey.F9);
            Dictionary.Add((int)Keys.F10, KeyboardKey.F10);
            Dictionary.Add((int)Keys.F11, KeyboardKey.F11);
            Dictionary.Add((int)Keys.F12, KeyboardKey.F12);

            Dictionary.Add((int)Keys.D0, KeyboardKey.D0);
            Dictionary.Add((int)Keys.D1, KeyboardKey.D1);
            Dictionary.Add((int)Keys.D2, KeyboardKey.D2);
            Dictionary.Add((int)Keys.D3, KeyboardKey.D3);
            Dictionary.Add((int)Keys.D4, KeyboardKey.D4);
            Dictionary.Add((int)Keys.D5, KeyboardKey.D5);
            Dictionary.Add((int)Keys.D6, KeyboardKey.D6);
            Dictionary.Add((int)Keys.D7, KeyboardKey.D7);
            Dictionary.Add((int)Keys.D8, KeyboardKey.D8);
            Dictionary.Add((int)Keys.D9, KeyboardKey.D9);

            Dictionary.Add((int)Keys.NumPad0, KeyboardKey.NumPad0);
            Dictionary.Add((int)Keys.NumPad1, KeyboardKey.NumPad1);
            Dictionary.Add((int)Keys.NumPad2, KeyboardKey.NumPad2);
            Dictionary.Add((int)Keys.NumPad3, KeyboardKey.NumPad3);
            Dictionary.Add((int)Keys.NumPad4, KeyboardKey.NumPad4);
            Dictionary.Add((int)Keys.NumPad5, KeyboardKey.NumPad5);
            Dictionary.Add((int)Keys.NumPad6, KeyboardKey.NumPad6);
            Dictionary.Add((int)Keys.NumPad7, KeyboardKey.NumPad7);
            Dictionary.Add((int)Keys.NumPad8, KeyboardKey.NumPad8);
            Dictionary.Add((int)Keys.NumPad9, KeyboardKey.NumPad9);
        }

        private readonly static Dictionary<int, KeyboardKey> Dictionary=new Dictionary<int, KeyboardKey>();

        public static KeyboardKey Convert(int key)
        {
            return Dictionary.FirstOrDefault(p => p.Key == key).Value;
        }
    }
}
