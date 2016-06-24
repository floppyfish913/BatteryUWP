using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel;



namespace BatteryHnd
{
    public class BatteryHandler
    {
        private String Status;
        private int Percentage;

        private StorageFolder ConfigFolder;

        private StorageFile BatteryInfoFile;


        public BatteryHandler()
        {
            Status = "Loading";
            Percentage = 0;
            BatteryHandlerInit();

        }

        private async void BatteryHandlerInit()
        {
            try
            {
                ConfigFolder = ApplicationData.Current.LocalFolder;
                BatteryInfoFile = await ConfigFolder.GetFileAsync(@"var\Battery");
                syncBatteryData();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                System.Diagnostics.Debug.WriteLine("Battery Info File is Missing or Access Denied!");
                await ConfigFolder.CreateFolderAsync("var");
                await ConfigFolder.CreateFileAsync(@"var\Battery");
            }

        }

        public async void syncBatteryData()
        {
            try
            {
                string data = await FileIO.ReadTextAsync(BatteryInfoFile);
                string p_Status = data.Substring(0, data.IndexOf("\r"));
                string data_ = data.Substring(data.IndexOf("\n") + 1);
                this.Percentage = Int32.Parse(data_);

                if (p_Status.StartsWith("C") || p_Status.StartsWith("F"))
                {
                    Status = "Charging";
                    if (this.Percentage == 100)
                    {
                        Status = "Fully Charged";
                    }
                }
                else if (p_Status.StartsWith("D"))
                {
                    Status = "Discharging";
                    if (this.Percentage < 20)
                    {
                        Status = "Low Battery";
                    }


                }
                else
                {

                    Status = "INVALID";
                }


            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e);


            }
        }

        public string getConfigFolderPath()

        {
            return ConfigFolder.Path;
        }

        ////////////////////////////////////////////////////
        // 
        //  Alapvetően állapotokat ad vissza
        //    Charging
        //    DisCharging
        //    Fully Charged
        //
        ////////////////////////////////////////////////////


        public string getBatteryStatus()
        {
            return Status;
        }
        ////////////////////////////////////////////////////
        // 
        //     Egészként visszaadja az akku becsült százalékos értékét
        //
        ////////////////////////////////////////////////////
        public int getBatteryData()
        {
            return Percentage;
        }

        
    }

}