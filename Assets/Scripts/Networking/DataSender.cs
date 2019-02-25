using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClientPackets
{
    CHelloServer = 1
}
static class DataSender
{
    public static void SendHelloServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackets.CHelloServer);
        buffer.WriteString("Connected!");
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
}
