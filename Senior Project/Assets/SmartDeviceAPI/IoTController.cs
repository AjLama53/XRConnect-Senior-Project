using System;
using System.Collections.Generic;
using UnityEngine;


public class IoTController
{
    public void TurnOnDevice(IoTDevice device)
    {
        device.SendCommand("turn_on");
    }

    public void TurnOffDevice(IoTDevice device)
    {
        device.SendCommand("turn_off");
    }

    public void SetBrightness(IoTDevice device, int level)
    {
        if (device.Type == DeviceType.Light)
        {
            device.SendCommand($"brightness:{level}");
        }
    }
}
