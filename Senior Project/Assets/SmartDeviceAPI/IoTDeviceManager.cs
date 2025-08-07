using System;
using System.Collections.Generic;
using UnityEngine;

public class IoTDeviceManager
{
    // List of discovered devices  
    private List<IoTDevice> connectedDevices = new List<IoTDevice>();

    // Event triggered when a new device is discovered  
    public event Action<IoTDevice> OnDeviceDiscovered;

    // Initialize the IoT manager  
    public void Initialize()
    {
        // Start scanning for devices  
        DiscoverDevices();
    }

    // Discover IoT devices on the network  
    private void DiscoverDevices()
    {
        // Simulated discovery logic  
        IoTDevice newDevice = new IoTDevice("Smart Light", "192.168.1.10", DeviceType.Light);
        connectedDevices.Add(newDevice);

        // Trigger event  
        OnDeviceDiscovered?.Invoke(newDevice);
    }

    // Get a list of available devices  
    public List<IoTDevice> GetDevices() => connectedDevices;
}
