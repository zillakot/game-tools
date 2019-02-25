using UnityEngine;

public enum ServerPackets
{
    SWelcomeMessage = 1,
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
}