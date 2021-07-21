using CG.Events.Models;
using System;
using System.Windows;

namespace CG.Events.QuickStart.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEventAggregator _events = new EventAggregator();

        public MainWindow()
        {
            // Subscribe to a simple event.
            _events.GetEvent<TestEvent>()
                .Subscribe((args) =>
            {
                // Notice the use of the WPF dispatcher here, for Windows
                //   synchronization issues. I decided not to include this
                //   inside the event aggregator because it's really not the
                //   concern of the aggregator whether Windows synchronization
                //   is an issue, or not. 

                if (Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() =>
                    {
                        label1.Content = "event fired!";
                    });
                }
            }, 
            true // <-- notice we have the ability to use a strong reference.
            );

            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Publish a simple event.
            _events.GetEvent<TestEvent>()
                .Publish();
        }
    }

    class TestEvent : EventBase { }
}
