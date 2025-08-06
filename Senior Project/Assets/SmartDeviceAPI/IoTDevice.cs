using System;
using System.Collections.Generic;
using UnityEngine;
public enum DeviceType
{
    Light,
    Speaker,
    RC_Car,
    Other
}

public class IoTDevice
{
    public string DeviceName { get; private set; }
    public string IPAddress { get; private set; }
    public DeviceType Type { get; private set; }

    public IoTDevice(string name, string ip, DeviceType type)
    {
        DeviceName = name;
        IPAddress = ip;
        Type = type;
    }

    // Generic command to send a request to the IoT device  
    public void SendCommand(string command)
    {
        Debug.Log($"Sending command '{command}' to {DeviceName} at {IPAddress}");
        // HTTP or MQTT logic to communicate with the device  
    }
}
