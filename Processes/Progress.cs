using System;
using System.Text;
using System.Threading.Tasks;

namespace McMorph.Processes
{
    public class Progress : IDisposable
    {
        private const int width = 30;
        private const int milliSeconds = 150;
        private int state  = 0;
        (int col, int row)? pos = null;

        public void Advance()
        {
            if (!this.pos.HasValue)
            {
                this.pos = Terminal.GetPosition();
                this.state = 0;
            }
            var newState = (int)(DateTime.Now.Ticks / (10000 * milliSeconds));
            if (newState != this.state)
            {
                Terminal.SetPosition(this.pos.Value);
                Terminal.Write("[", MakeIndefinite(this.state, width - 2), "]");
                this.state = newState;
            }
        }

        public string MakeIndefinite(int step, int width)
        {
            const string pattern = "―•―     ";
            var builder = new StringBuilder();
            builder.Append(pattern.Substring((pattern.Length - step) % pattern.Length));
            while (builder.Length < width)
            {
                builder.Append(pattern);
            }
            builder.Length = width;

            return builder.ToString();
        }

        public void Dispose()
        {
            Terminal.GotoLineHome();
            Terminal.ClearLine();
        }
    }
}