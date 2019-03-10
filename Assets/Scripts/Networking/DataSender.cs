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
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
    public static void SendPlayerPosition(Transform transform)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackets.CPlayerPosition);
        buffer.WriteString(transform.ToString());
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
}
