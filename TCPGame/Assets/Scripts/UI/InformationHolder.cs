using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationHolder : MonoBehaviour
{
    private string IpAddress = "";
    private int Port = -1;
    private int PlayFirst = -1;
    private int PieceColor = 0;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // 0 = red pieces = -1
    // 1 = blue pieces = 1
    public void SetPieceColor(int colorChoice)
    {
        this.PieceColor = colorChoice;
    }

    public int GetPieceColor()
    {
        return this.PieceColor;
    }

    // false == play second = 1
    // true ==  play first = 0
    public void SetPlayFirst(int choice)
    {
        this.PlayFirst = choice;
    }

    public int GetPlayFirst()
    {
        return this.PlayFirst;
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
