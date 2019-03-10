using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClientPackets
{
    CHelloServer = 1,
    CPlayerPosition = 2,
}
public static class DataSender
{
    public static void SendHelloServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackets.CHelloServer);
        buffer.WriteString("Connected!");
        Debug.Log("Send connected");
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
    public static void SendPlayerPosition(Vector3 position)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackets.CPlayerPosition);
        buffer.WriteString(position.ToString());
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
}
