using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace MyBleApp
{
    public class BluetoothManager
    {
        private BluetoothManager() { }

        private static BluetoothManager _instance;

        public static BluetoothManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BluetoothManager();
            }
            return _instance;
        }

        private BluetoothLEDevice bluetoothLeDevice = null;
        private GattDeviceService selectedService;
        private GattCharacteristic selectedCharacteristic;

        // Only one registered characteristic at a time.
        private GattCharacteristic registeredCharacteristic;
        private GattPresentationFormat presentationFormat;

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            Log.Green("Value Changed.");
        }

        public async void ConnectToDeviceMacAsync(ulong btMacAddress)
        {
            Log.Std($"Connecting to {btMacAddress}...");

            bluetoothLeDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(btMacAddress).AsTask();

            if (bluetoothLeDevice != null)
            {
                Log.Green("Connected.");

                // Get ST features service
                var gServiceResult = await bluetoothLeDevice.GetGattServicesForUuidAsync(Guid.Parse("00000000-0001-11e1-9ab4-0002a5d5c51b")).AsTask();

                if (gServiceResult.Status == GattCommunicationStatus.Success)
                {
                    selectedService = gServiceResult.Services[0];

                    // Get Acc, Gyro and Mag Characteristic
                    //var gCharResult = await selectedService.GetCharacteristicsForUuidAsync(Guid.Parse("00000000-0001-11e1-ac36-0002a5d5c51b")).AsTask();
                    //var gCharResult = await selectedService.GetCharacteristicsForUuidAsync(Guid.Parse("00e00000-0001-11e1-ac36-0002a5d5c51b")).AsTask();
                    var gCharResult = await selectedService.GetCharacteristicsForUuidAsync(Guid.Parse("00020000-0001-11e1-ac36-0002a5d5c51b")).AsTask();

                    if (gCharResult.Status == GattCommunicationStatus.Success)
                    {
                        selectedCharacteristic = gCharResult.Characteristics[0];

                        var properties = selectedCharacteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            Log.Cyan("Read");
                        }
                        if (properties.HasFlag(GattCharacteristicProperties.Write))
                        {
                            Log.Cyan("Write");
                        }
                        if (properties.HasFlag(GattCharacteristicProperties.Notify))
                        {
                            Log.Cyan("Notify");
                        }

                        // Check descriptors
                        //var descriptors = await accGyrMagChar.GetDescriptorsAsync().AsTask();
                        //Log.Cyan($"Total of descriptors: {descriptors.Descriptors.Count}");

                        //for (int x = 0; x < 100; x++)
                        //{
                        //    GattReadResult result = await selectedCharacteristic.ReadValueAsync().AsTask();
                        //    if (result.Status == GattCommunicationStatus.Success)
                        //    {
                        //        var reader = DataReader.FromBuffer(result.Value);
                        //        byte[] input = new byte[reader.UnconsumedBufferLength];
                        //        reader.ReadBytes(input);
                        //        // Utilize the data as needed
                        //        Log.Green($"Data: {BitConverter.ToString(input)}");
                        //    }
                        //}

                        // Subscribe for notifications 
                        Log.Std("Setting notifications...");

                        var setNotifyOff = await selectedCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.None).AsTask();
                        if (setNotifyOff == GattCommunicationStatus.Success)
                        {
                            Log.Green("UnSubscribed.");
                        }
                        else
                        {
                            Log.Red("Failed to unsubscribe.");
                        }

                        var status = await selectedCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Notify).AsTask();
                        if (status == GattCommunicationStatus.Success)
                        {
                            Log.Green("Subscribed.");
                            selectedCharacteristic.ValueChanged += Characteristic_ValueChanged;
                        }
                        else
                        {
                            Log.Red("Failed to subscribe.");
                        }
                    }
                    else
                    {
                        Log.Red("Failed to find characteristic");
                    }

                }
                else
                {
                    Log.Red("Failed to find service.");
                }
            }
        }

        //public async Task<bool> ShowPairedBluetoothDevicesAsync()
        //{
        //    var collection = await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelectorFromPairingState(true));

        //    foreach (var dev in collection)
        //    {
        //        if (dev.Name.ToLower().Contains("am1v"))
        //        {
        //            Console.WriteLine($"Device Name: {dev.Name}");
        //            Console.WriteLine($"Device Id: {dev.Id}");

        //            var bleDev = await BluetoothLEDevice.FromIdAsync(dev.Id);
        //            Console.WriteLine($"Device Address: {bleDev.BluetoothAddress}");

        //            bleDev.Dispose();
        //        }
        //    }

        //    return true;
        //}
    }
}
