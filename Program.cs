using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBleApp
{
    class Program
    {
        //211708537755725 //big cradle
        //211708370571568 //small cradle
        const ulong DevSmallCradle = 211708370571568;
        const ulong DevBigCradle = 211708537755725;

        static void Main(string[] args)
        {
            Log.Std("App started");

            while (true)
            {
                // Pause until we press enter
                var command = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrEmpty(command))
                {
                }
                else if (command == "connect")
                {
                    BluetoothManager.GetInstance().ConnectToDeviceMacAsync(DevBigCradle);
                }
                else if (command == "show")
                {
                    //MBluetoothHelper.ShowPairedBluetoothDevicesAsync();
                }
                else if (command == "inspect")
                {
                    //MBluetoothHelper.ShowDeviceServicesAndCharsAsync(DevBigCradle);
                }
            }
        }
    }
}
