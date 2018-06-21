using System;

using McMorph.Actors;

namespace McMorph
{
    public static class Terminal
    {
        public static int Width => Console.WindowWidth;
        public static int Height => Console.WindowHeight;
        
        public static void Write(params string[] texts)
        {
            Console.Write(string.Join("", texts));
        }

        public static void WriteLine(params string[] texts)
        {
            Console.WriteLine(string.Join("", texts));
        }

        public static void GotoLineHome()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void ClearLine()
        {
            var left = Console.CursorLeft;
            var top = Console.CursorTop;
            Console.SetCursorPosition(0, top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, top);
        }
    }
}
