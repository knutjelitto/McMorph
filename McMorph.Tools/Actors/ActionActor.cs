using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace McMorph.Tools.Actors
{
    public class ActionActor : Actor<Action>
    {
        public override void Handle(Action action)
        {
            action();
        }
    }
}