using CG.Events.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CG.Events.QuickStart.Hosted
{
    class Program
    {
        public static void Main(string[] args)
        {
            // We'll use hosting to make this sample more fun.
            CreateHostBuilder(args).Build().RunDelegate((host, token) =>
            {
                // Create our registered event aggregator.
                var events = host.Services.GetRequiredService<IEventAggregator>();

                // Grab our test event.
                var @event = events.GetEvent<TestEvent>();

                // Subscribe and send the events to a delegate ...
                @event.Subscribe((args) => 
                {
                    Console.WriteLine($"action called at: {DateTime.Now}. Args: '{string.Join(",", args)}'");
                });

                // Subscribe and let the event itself deal with notifications.
                @event.Subscribe(); 

                // Publish the event with loads of arguments.
                @event.Publish("a", "b", "c");

                // Publish the event with no arguments.
                @event.Publish();
            });
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddEventAggregation(); // <-- let's use our event aggregator.
                });
    }

    class TestEvent : EventBase 
    {
        private readonly ILogger<TestEvent> _logger;

        // Because we're using the DI container built into Microsft's
        //   hosting package, to create this event, we can happily inject
        //   any registered service into this ctor ... 
        public TestEvent(
            ILogger<TestEvent> logger
            )
        {
            _logger = logger;
        }

        protected override void OnInvoke(
            params object[] args
            )
        {
            // This method is an example of how the logic for an event
            //   can be isolated from the logic that subscribes and/or 
            //   publishes that event - which, can be handy sometimes.
            _logger.LogInformation($"OnInvoke called at: {DateTime.Now}. Args: '{string.Join(",", args)}'");
            
            // Can call this, or not *shrugs* whatever.
            base.OnInvoke();
        }
    }
}
