using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Battery_Manager
{
    class Battery
    {
        public int PreviousTime { get; set; }
        public int CurrentTime { get; set; }
        public string PowerLineStatus { get; set; }
        public string BatteryLifePercent { get; set; }
        public string BatteryLifeRemaining { get; set; }

        Regex VideoIdleExpression = new Regex("VIDEOIDLE.*\\n.*\\n.*\\n.*\\n.*\\n.*\\n.*");

        public Battery()
        {
            PreviousTime = GetTime();
            CurrentTime = PreviousTime;
            UpdateInfo();
        }

        public int GetTime()
        {
            Process timeProcess = Process.Start(PrepareStartInfo("cmd.exe", "/c powercfg /q", String.Empty));
            string  videoIdle = VideoIdleExpression.Match(timeProcess.StandardOutput.ReadToEnd()).Value;
            string currentTime = videoIdle.Substring(videoIdle.Length - 11).TrimEnd();
            timeProcess.WaitForExit();
            timeProcess.Close();
            return Convert.ToInt32(currentTime, 16) / 60;
        }

        public void UpdateInfo(bool timeChanged = false)
        {
            if (PowerLineStatus != SystemInformation.PowerStatus.PowerLineStatus.ToString())
            {
                SetNewTime(PreviousTime);
                timeChanged = true;
            }
            if (timeChanged) CurrentTime = GetTime();
            PowerLineStatus = SystemInformation.PowerStatus.PowerLineStatus.ToString();
            BatteryLifePercent = SystemInformation.PowerStatus.BatteryLifePercent * 100 + "%";
            SetPowerLifeRemaining();
        }

        private void SetPowerLifeRemaining()
        {
            if (PowerLineStatus == "Offline")
            {
                int batteryLife = SystemInformation.PowerStatus.BatteryLifeRemaining;
                if (batteryLife != -1)
                {
                    BatteryLifeRemaining = new TimeSpan(0, 0, batteryLife).ToString("c");
                }
                else BatteryLifeRemaining = "In process";
            }
            else
            {
                BatteryLifeRemaining = "From AC adapter";
            }
        }

        private ProcessStartInfo PrepareStartInfo(string fileName, string arguments, string value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments + value;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            return startInfo;

        }

        public void SetNewTime(int time)
        {
            Process setTimeProcess = Process.Start(PrepareStartInfo("cmd.exe", "/c powercfg /x -monitor-timeout-dc ", time.ToString()));
            setTimeProcess.WaitForExit();
            setTimeProcess.Close();
        }
    }
}
