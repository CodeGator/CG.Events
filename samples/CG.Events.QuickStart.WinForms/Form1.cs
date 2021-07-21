using CG.Events.Models;
using System;
using System.Windows.Forms;

namespace CG.Events.QuickStart.WinForms
{
    public partial class Form1 : Form
    {
        IEventAggregator _events = new EventAggregator();

        public Form1()
        {
            // Subscribe to a simple event.
            _events.GetEvent<TestEvent>()
                .Subscribe((args) =>
                {
                    // Notice the use of Windows synchronization here. I decided
                    //   not to include this inside the event aggregator because
                    //   it's really not the concern of the aggregator whether
                    //   Windows synchronization happens, or not. 

                    if (this.InvokeRequired)
                    {
                        Invoke((Action)(() => 
                        {
                            label1.Text = $"action called at: '{DateTime.Now}'";
                        }));
                    }
                    else
                    {
                        label1.Text = $"action called at: '{DateTime.Now}'";
                    }
                },
            true // <-- notice we have the ability to use a strong reference.
            );

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Publish a simple event.
            _events.GetEvent<TestEvent>()
                .Publish();
        }
    }

    class TestEvent : EventBase { }
}
