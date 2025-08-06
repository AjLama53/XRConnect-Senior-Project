using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPListener : MonoBehaviour
{
    private UdpClient udpClient;
    private const int listenPort = 56700; // Listening on the correct LIFX port

    private void Start()
    {
        // Bind to any available IP address on port 56700
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
        udpClient = new UdpClient(localEndPoint);
        udpClient.BeginReceive(OnReceive, null);
        Debug.Log($"Listening for UDP traffic on port {listenPort}");
    }

    private void OnReceive(IAsyncResult result)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, listenPort);
        byte[] receivedData = udpClient.EndReceive(result, ref remoteEndPoint);

        if (receivedData.Length > 0)
        {
            Debug.Log($"Received packet from {remoteEndPoint.Address}");
            ParseLIFXResponse(receivedData);
        }

        // Continue listening for more packets
        udpClient.BeginReceive(OnReceive, null);
    }

    // Decode LIFX packet (basic version)
    private void ParseLIFXResponse(byte[] data)
    {
        if (data.Length >= 36)
        {
            ushort packetType = BitConverter.ToUInt16(data, 32);
            Debug.Log($"Received packet of type {packetType}");
        }
        else
        {
            Debug.LogWarning("Received an invalid LIFX packet.");
        }
    }
}
