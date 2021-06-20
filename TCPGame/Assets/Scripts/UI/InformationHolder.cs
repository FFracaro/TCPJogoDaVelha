using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationHolder : MonoBehaviour
{
    private string IpAddress = "";
    private int Port = -1;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void AddIpAddressAndPort(string ip, int port)
    {
        IpAddress = ip;
        Port = port;
    }

    public string GetIpAddress()
    {
        return IpAddress;
    }

    public int GetPort()
    {
        return Port;
    }

    public void DestroyInformationHolder()
    {
        Destroy(this);
    }
}
