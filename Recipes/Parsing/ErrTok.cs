namespace McMorph.Recipes
{
    public class ErrTok : Token
    {
        public ErrTok(int lineNo, string message)
            : base(lineNo)
        {
            this.Message = message;
        }

        public string Message { get; }

        public override string ToString()
        {
            return Message;
        }
    }
}