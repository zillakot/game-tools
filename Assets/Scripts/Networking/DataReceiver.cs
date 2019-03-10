using UnityEngine;

public enum ServerPackets
{
    SWelcomeMessage = 1,
    SPlayerPosition = 2,
}

static class DataReceiver
{
    public static void HandleWelcomeMessage(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetID = buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();

        Debug.Log(msg);
        DataSender.SendHelloServer();
    }

    public static void HandlePlayerPosition(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetID = buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();
        Debug.Log("serverpos: " + msg);
    }
}