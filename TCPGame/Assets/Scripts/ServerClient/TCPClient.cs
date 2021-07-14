using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour
{
    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;

    private string ServerIpAddress;
    private int ServerPort;
    #endregion

    public void ConnectToTcpServer()
    {
        InformationHolder Info = FindObjectOfType<InformationHolder>();

        ServerIpAddress = Info.GetIpAddress();
        ServerPort = Info.GetPort();

        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();

            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<UIManager>().ClientUICommunication(ServerMessageType.CLIENTON, "Cliente iniciado.");
            });

            Debug.Log("Cliente iniciado.");
        }
        catch (Exception e)
        {
            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<ErrorReport>().ClientConnectionError(e.ToString());
            });

            Debug.Log("On client connect exception " + e);
        }
    }

    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(ServerIpAddress, ServerPort);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var serverData = new byte[length];
                        Array.Copy(bytes, 0, serverData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(serverData);
                        Debug.Log("server message received as: " + serverMessage);

                        ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                            FindObjectOfType<MessageInterpreter>().ParseMessageReceived(serverMessage);
                        });
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<ErrorReport>().ShowSocketError(socketException.ToString());
            });

            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void MessageToSend(string message)
    {
        byte[] messageToSendAsByteArray = Encoding.ASCII.GetBytes(message);
        SendMessage(messageToSendAsByteArray);
    }

    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    private void SendMessage(byte[] messageToSend)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {               
                stream.Write(messageToSend, 0, messageToSend.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<ErrorReport>().ShowSocketError(socketException.ToString());
            });

            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void CloseClient()
    {
        if(socketConnection != null)
        {
            if (socketConnection.Connected)
            {
                socketConnection.Close();
            }
        }
    }
}
