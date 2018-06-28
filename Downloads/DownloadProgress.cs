using System;

using McMorph.Actors;

namespace McMorph.Downloads
{
    public class DownloadProgress : IDisposable
    {
        private bool disposed = false; // To detect redundant calls
        private readonly string prefix;
        private ActionActor progress = new ActionActor();
        private string lastBar = string.Empty;

        public DownloadProgress(string prefix)
        {
            this.prefix = prefix;
        }

        public void Advance(long received, long total)
        {
            if (total > 0)
            {
                progress.Send(() => Definite(received, total));
            }
            else
            {
                progress.Send(() => Indefinite(received));
            }
        }

        private string DBar(long received, long total, int width)
        {
            const char line = '━';
            const char knob = '┄';

            int chars = (int)(width * received / total);

            var bar = new string(line, chars) + new string(knob, width - chars);

            return bar;
        }

        private string IBar(long total, int width)
        {
            const string boat = "<==>";

            int offset = ((int)(total / (256 * 1024))) % (width - boat.Length);

            var bar = new string('.', offset) + boat + new string('.', width - offset - boat.Length);

            return bar;
        }

        private string Start()
        {
            return "download " + this.prefix + ": ┣";
        }

        private string End()
        {
            return "┤";
        }

        private int Inner(string start, string end)
        {
            return Math.Min(30, Terminal.Width - start.Length - end.Length - 10);
        }

        private void Indefinite(long received)
        {
            var start = Start();
            var end = End();
            
            var width = Inner(start, end);

            if (width >= 20)
            {
                var bar = IBar(received, width);

                if (bar != this.lastBar)
                {
                    Terminal.GotoLineHome();
                    Terminal.Write(start, bar, end);
                    this.lastBar = bar;
                }
            }
        }

        private void Definite(long received, long total)
        {
            var start = Start();
            var end = End();
            
            var width = Inner(start, end);

            if (width >= 10)
            {
                var bar = DBar(received, total, width);

                if (bar != this.lastBar)
                {
                    Terminal.GotoLineHome();
                    Terminal.Write(start, bar, end);
                    this.lastBar = bar;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    progress.Done();
                    Terminal.ClearLine();
                    Terminal.Write(this.prefix);
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}