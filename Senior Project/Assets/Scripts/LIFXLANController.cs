using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LIFXLANController : MonoBehaviour
{
    [Header("UI References")]
    public Slider hueSlider;
    public Slider saturationSlider;
    public Slider brightnessSlider;
    public Toggle powerToggle;

    [Header("Bulb Configuration")]
    public string bulbIpAddress = "192.168.1.113";  // Set this per panel in the Inspector

    private const int LIFX_PORT = 56700;
    private UdpClient udpClient;
    private IPEndPoint endPoint;
    private bool isOn = false;

    private void Start()
    {
        // Get correct IP address based on context
        if (gameObject.name.Contains("Light1"))
        {
            bulbIpAddress = AppSettingsManager.Instance.light1IP;
        }
        else if (gameObject.name.Contains("Light2"))
        {
            bulbIpAddress = AppSettingsManager.Instance.light2IP;
        }

        udpClient = new UdpClient();
        endPoint = new IPEndPoint(IPAddress.Parse(bulbIpAddress), LIFX_PORT);

        Debug.Log($"✅ LIFX Controller Initialized for {bulbIpAddress}");

        SendGetService();

        hueSlider.onValueChanged.AddListener(delegate { UpdateBulbColor(); });
        saturationSlider.onValueChanged.AddListener(delegate { UpdateBulbColor(); });
        brightnessSlider.onValueChanged.AddListener(delegate { UpdateBulbColor(); });
        powerToggle.onValueChanged.AddListener(delegate { ToggleBulbPower(powerToggle.isOn); });

        // Default slider values
        hueSlider.value = 0;
        saturationSlider.value = 65535;
        brightnessSlider.value = 32768;

        // Query initial state
        SendLIFXPacket(BuildHeader(101, 36, responseRequired: true)); // GetColor
        SendLIFXPacket(BuildHeader(20, 36, responseRequired: true));  // GetPower
        ReceiveInitialState();
    }



    private byte[] BuildHeader(ushort packetType, ushort size, bool ackRequired = false, bool responseRequired = false)
    {
        byte[] header = new byte[36];
        BitConverter.GetBytes(size).CopyTo(header, 0);
        header[3] = 0x34;
        BitConverter.GetBytes((uint)0).CopyTo(header, 6);
        for (int i = 12; i <= 23; i++) header[i] = 0x00;

        byte flags = 0;
        if (ackRequired) flags |= 0x01;
        if (responseRequired) flags |= 0x02;
        header[22] = flags;

        BitConverter.GetBytes(packetType).CopyTo(header, 32);
        header[34] = 0x00;
        header[35] = 0x00;

        return header;
    }

    public void SendGetService()
    {
        SendLIFXPacket(BuildHeader(2, 36, responseRequired: true));
        Debug.Log("✅ GetService packet sent.");
    }

    public void SendGetVersion()
    {
        SendLIFXPacket(BuildHeader(32, 36, responseRequired: true));
        Debug.Log("✅ GetVersion packet sent.");
    }

    public void SetPower(bool isOn)
    {
        byte[] packet = new byte[38];
        BuildHeader(117, 38, ackRequired: true).CopyTo(packet, 0);
        packet[36] = (byte)(isOn ? 0xFF : 0x00);
        packet[37] = 0x00;

        SendLIFXPacket(packet);
        Debug.Log($"✅ SetPower packet sent. Power: {(isOn ? "ON" : "OFF")}");
    }

    public void SetColor(ushort hue, ushort saturation, ushort brightness, ushort kelvin)
    {
        byte[] packet = new byte[49];
        BuildHeader(102, 49, ackRequired: true).CopyTo(packet, 0);
        packet[36] = 0x00;
        BitConverter.GetBytes(hue).CopyTo(packet, 37);
        BitConverter.GetBytes(saturation).CopyTo(packet, 39);
        BitConverter.GetBytes(brightness).CopyTo(packet, 41);
        BitConverter.GetBytes(kelvin).CopyTo(packet, 43);
        BitConverter.GetBytes(0).CopyTo(packet, 45);

        SendLIFXPacket(packet);
        Debug.Log("✅ SetColor packet sent.");
    }

    public void GetPower()
    {
        SendLIFXPacket(BuildHeader(20, 36, responseRequired: true));
        Debug.Log("✅ GetPower packet sent.");
    }

    private void SendLIFXPacket(byte[] packet)
    {
        if (udpClient == null || endPoint == null)
        {
            Debug.LogError("❗️ UDP Client or Endpoint not initialized.");
            return;
        }

        udpClient.Send(packet, packet.Length, endPoint);
        Debug.Log($"📡 Packet sent to {bulbIpAddress}:{LIFX_PORT}");
        Debug.Log($"📦 Packet Bytes (Hex): {BitConverter.ToString(packet)}");
    }

    public void UpdateBulbColor()
    {
        ushort hue = (ushort)hueSlider.value;
        ushort saturation = (ushort)saturationSlider.value;
        ushort brightness = (ushort)brightnessSlider.value;

        SetColor(hue, saturation, brightness, 3500);
        Debug.Log($"🎨 Updated Color - Hue: {hue}, Saturation: {saturation}, Brightness: {brightness}");
    }

    public void ToggleBulbPower(bool state)
    {
        isOn = state;
        SetPower(isOn);
        Debug.Log($"🔘 Bulb is now {(isOn ? "ON" : "OFF")}");
    }

    private async void ReceiveInitialState()
    {
        try
        {
            var result = await udpClient.ReceiveAsync(); // ✅ use the same client
            byte[] data = result.Buffer;

            if (data.Length >= 36)
            {
                ushort type = BitConverter.ToUInt16(data, 32);

                if (type == 22) // StatePower
                {
                    ushort powerValue = BitConverter.ToUInt16(data, 36);
                    bool isBulbOn = powerValue > 0;

                    isOn = isBulbOn;
                    powerToggle.SetIsOnWithoutNotify(isOn);
                    Debug.Log($"🟢 Power state: {(isBulbOn ? "ON" : "OFF")}");
                }
                else if (type == 107) // State (color info)
                {
                    ushort hue = BitConverter.ToUInt16(data, 37);
                    ushort saturation = BitConverter.ToUInt16(data, 39);
                    ushort brightness = BitConverter.ToUInt16(data, 41);
                    ushort kelvin = BitConverter.ToUInt16(data, 43);

                    hueSlider.SetValueWithoutNotify(hue);
                    saturationSlider.SetValueWithoutNotify(saturation);
                    brightnessSlider.SetValueWithoutNotify(brightness);

                    Debug.Log($"🎨 Retrieved Color - Hue: {hue}, Saturation: {saturation}, Brightness: {brightness}");
                }
            }
        }
        catch (SocketException)
        {
            Debug.LogWarning("⚠️ Timed out waiting for LIFX response.");
        }
    }


}
