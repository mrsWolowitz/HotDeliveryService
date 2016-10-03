using SchedulerTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestWindow
{

    public partial class ClientTasks : Form
    {
        CancellationTokenSource _Cts1;
        CancellationTokenSource _Cts2;
        private Scheduler Scheduler { get; set; }
        public ClientTasks()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Scheduler = new Scheduler();
            if (Scheduler == null)
                return;
        }

        private async void StartCreateDeliveriesBtn_Click(object sender, EventArgs e)
        {
            if (_Cts1 != null)
                return;
            using (_Cts1 = new CancellationTokenSource())
            {
                await Scheduler.CreateDeliveries(_Cts1.Token);
            }
            _Cts1 = null;

        }

        private void StopTasksBtn_Click(object sender, EventArgs e)
        {
            _Cts1?.Cancel();
            _Cts2?.Cancel();
        }

        private async void ExpireDeliveries_Click(object sender, EventArgs e)
        {
            if (_Cts2 != null)
                return;
            using (_Cts2 = new CancellationTokenSource())
            {

                await Scheduler.ExpireDeliveries(_Cts2.Token);
            }
            _Cts2 = null;
        }
    }
}

