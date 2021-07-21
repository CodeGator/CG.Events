using CG.Events.Models;
using System;

namespace CG.Events.QuickStart
{
    class Program
    {
        static void Main(string[] args)
        {
            var ea = new EventAggregator();

            var ev1 = ea.GetEvent<Test1Event>();
            var ev2 = ea.GetEvent<Test2Event>();

            using (ev1.Subscribe((args) => { Console.WriteLine($"Event 1 handler called."); }))
            {
                using (ev2.Subscribe((args) => { Console.WriteLine("Event 2 handler called"); }))
                {
                    ev1.Publish(); // <-- this calls the handler, which is still in scope.
                    ev2.Publish(); // <-- this calls the handler, which is still in scope.
                }

                ev1.Publish(); // <-- this calls the handler, which is still in scope.
                ev2.Publish(); // <-- this does nothing, since the handler has been disposed.
            }

            ev1.Publish(); // <-- this does nothing, since the handler has been disposed.
            ev2.Publish(); // <-- this does nothing, since the handler has been disposed.
        }
    }

    class Test1Event : EventBase { }
    class Test2Event : EventBase { }
}
