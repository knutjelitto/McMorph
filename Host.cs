using System;

namespace McMorph
{
    public static class Host
    {
        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void Write(params string[] texts)
        {
            foreach (var text in texts)
            {
                Write(text);
            }
        }

        public static void WriteLine(params string[] texts)
        {
            Write(texts);
            Console.WriteLine();
        }

        public static void LineHome()
        {
            Write("\r");
        }

        public static void LineClear()
        {
            LineHome();
            Write(new string(' ', Console.WindowWidth - 1));
            LineHome();
        }
    }
}