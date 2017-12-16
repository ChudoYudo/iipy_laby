using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Getting_USB_Devices
{
    public partial class Form1 : Form
    {
        private readonly DeviceParser _searcher = new DeviceParser();
        private List<DeviceInfo> _devices;
        private readonly DataTable _table = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _devices = new List<DeviceInfo>();
            _table.Columns.Add("Directory", typeof(string));
            _table.Columns.Add("Name", typeof(string));
            _table.Columns.Add("Total size", typeof(string));
            _table.Columns.Add("Free space", typeof(string));
            _table.Columns.Add("Occupied space", typeof(string));
            UpdateGrid();
            OutputGrid.DataSource = _table;
            RemoveButton.Enabled = false;
            Timer1.Enabled = true;
        }

        private void UpdateGrid()
        {
            int currentPosition = 0;
            if (OutputGrid.CurrentRow != null)
            {
                currentPosition = OutputGrid.CurrentRow.Index;
            }
            _table.Clear();
            _devices = _searcher.GetDevices();
            foreach (DeviceInfo device in _devices)
            {
                _table.Rows.Add(device.Directory,device.Name, device.TotalSpace, device.FreeSpace, device.OccupiedSpace);
            }
            if (OutputGrid.RowCount - 1 > currentPosition)
            {
                OutputGrid.Rows[currentPosition].Selected = true;
            }
        }

        private void OutputGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (OutputGrid.CurrentRow != null)
            {
                Message.Text = "";
                if (OutputGrid.CurrentRow.Index >= 0 && OutputGrid.CurrentRow.Index < _devices.Count)
                {
                    RemoveButton.Enabled = !_devices[OutputGrid.CurrentRow.Index].IsMtp;
                }
                else
                {
                    RemoveButton.Enabled = false;
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (OutputGrid.CurrentRow != null)
            {
                Message.Text = _devices[OutputGrid.CurrentRow.Index].Retrieval();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateGrid();
        }
    }
}
