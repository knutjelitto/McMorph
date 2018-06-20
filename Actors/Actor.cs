using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace McMorph.Actors
{
    public abstract class Actor<T> where T : class
    {
        private readonly ActionBlock<T> _action;
 
        public Actor()
        {
            _action = new ActionBlock<T>(message =>
            {
                Handle(message);
            });
        }
 
        public void Send(T message)
        {
            _action.Post(message);
        }

        public abstract void Handle(T message);
    }
}