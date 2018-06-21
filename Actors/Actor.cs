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
            var options = new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 1 };
            _action = new ActionBlock<T>(message =>
            {
                Handle(message);
            }, options);
        }
 
        public void Send(T message)
        {
            _action.Post(message);
        }

        public void Done()
        {
            this._action.Complete();
            this._action.Completion.Wait();
        }

        public abstract void Handle(T message);
    }
}