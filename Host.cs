using System;

using McMorph.Actors;

namespace McMorph
{
    public static class Host
    {
        private static HostActor actor = new HostActor();

        public static void Write(string text)
        {
            actor.Send(() => Console.Write(text));
        }

        public static void Write(params string[] texts)
        {
            Write(string.Join("", texts));
        }

        public static void WriteLine(params string[] texts)
        {
            Write(string.Join("", texts) + "\r\n");
        }

        public static void LineHome()
        {
            Write("\r");
        }

        public static void LineClear()
        {
            Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
        }

        private class HostActor : Actor<Action>
        {
            public override void Handle(Action action)
            {
                action();
            }
        }
    }
}
