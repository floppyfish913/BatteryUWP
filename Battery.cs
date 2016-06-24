using System;
using System.Timers;
using System.IO;
namespace Battery
{
    public class BatteryHandler
    {
        private char BatteryStatus;
        private char BatteryPercentage;
        private Timer Refresher;
        private StreamReader BatteryStatusFile;

        public BatteryHandler()
        {
            BatteryStatusFile = new StreamReader("C:\\var\\Battery");
            Refresher = new Timer(6000); //perces timer
            Refresher.Elapsed += Refresher_Elapsed;
            Refresher.Start();

        }

        private void Refresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Battery file beolvasása;
            string Status, Percentage;
            Status = BatteryStatusFile.ReadLine();
            Percentage = BatteryStatusFile.ReadLine();
            return;
        }

        public int getBatteryPercentage()
        {

        }

        public void setBatteryPercentage(int Value)
        {
            BatteryPercentage = Value;
        }


        ////////////////////////////////////////////////////
        // 
        //  Alapvetően állapotokat ad vissza
        //    Charging
        //    DisCharging
        //    Fully Charged
        //
        ////////////////////////////////////////////////////


        public char getBatteryStatus()
        {

        }


    }

    public static class BatteryStatus
    {

        enum Status { Charging, Discharging, Fully_Charged };

    }

}