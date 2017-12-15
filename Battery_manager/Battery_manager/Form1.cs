using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battery_Manager
{
    public partial class Form1 : Form
    {
        private readonly Battery battery = new Battery();
        private readonly Regex timeExpression = new Regex(@"^\d+$");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateWindowInfo();
            timer1.Enabled = true;
        }

        private void EnableInputParameters(bool enable)
        {
            NewTimeT.Enabled = enable;
            NewTimeB.Enabled = enable;
        }

        private void UpdateWindowInfo()
        {
            CurrentChargeL.Text = battery.BatteryLifePercent;
            CurrentCharge.Value = Convert.ToInt32(battery.BatteryLifePercent.TrimEnd('%'), 10);
            CurrentTimeT.Text = battery.CurrentTime.ToString();
            StatusT.Text = battery.PowerLineStatus;
            EnableInputParameters((battery.PowerLineStatus == "Online") ? false : true);
            TimeLeftT.Text = battery.BatteryLifeRemaining;
        }

        private void UpdateBattery(bool timeChaged = false)
        {
            battery.UpdateInfo(timeChaged);
            UpdateWindowInfo();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateBattery();
        }

        private void NewTimeB_Click(object sender, EventArgs e)
        {
            if (timeExpression.Match(NewTimeT.Text).Value != String.Empty)
            {
                battery.SetNewTime(Convert.ToInt32(NewTimeT.Text));
                UpdateBattery(true);
            }
            NewTimeT.Text = String.Empty;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            battery.SetNewTime(battery.PreviousTime);
        }
    }
}
