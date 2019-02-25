using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

public class CustomNetworkManager : MonoBehaviour
{
    public static CustomNetworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Customnetworkmanager Start");
        DontDestroyOnLoad(this);
        MainThreadDispatcher.Initialize();
        ClientHandleData.InitializePackets();
        ClientTCP.InitializingNetworking();
    }

    void OnApplicationQuit()
    {
        ClientTCP.Disconnect();
    }
}
