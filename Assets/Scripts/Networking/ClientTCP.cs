using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using UniRx;

static class ClientTCP
{
    private static TcpClient clientSocket;
    private static NetworkStream myStream;
    private static byte[] recBuffer;
    public static void InitializingNetworking()
    {
        Debug.Log("init networking");
        clientSocket = new TcpClient();
        clientSocket.ReceiveBufferSize = 4096;
        clientSocket.SendBufferSize = 4096;
        recBuffer = new byte[4096 * 2];
        clientSocket.BeginConnect("127.0.0.1", 5557, new AsyncCallback(ClientConnectCallback), clientSocket);
    }

    private static void ClientConnectCallback(IAsyncResult result)
    {
        clientSocket.EndConnect(result);
        if(clientSocket.Connected == false)
        {
            return;
        }
        else
        {
            clientSocket.NoDelay = true;
            myStream = clientSocket.GetStream();
            myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);
        }
    }

    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int length = myStream.EndRead(result);
            if(length < 0)
            {
                return;
            }
            byte[] newBytes = new byte[length];
            Array.Copy(recBuffer, newBytes,length);
            Scheduler.MainThreadFixedUpdate.Schedule(() => ClientHandleData.HandleData(newBytes));          
            myStream.BeginRead(recBuffer, 0, 4092 * 2, ReceiveCallback, null);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public static void SendData(byte[] data){
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.WriteBytes(data);
        myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        buffer.Dispose();
    }

    public static void Disconnect()
    {
        clientSocket.Close();
    }
}
