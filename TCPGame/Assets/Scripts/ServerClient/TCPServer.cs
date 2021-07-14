using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    #region private members 	
    // TCPListener to listen for incomming TCP connection requests. 	
    private TcpListener tcpListener;
    // Background thread for TcpServer workload. 	
    private Thread tcpListenerThread;
    // Create handle to connected tcp client. 	
    private TcpClient connectedTcpClient;

    private string ServerIpAddress;
    private int ServerPort;
    #endregion

    public void InitiateThreadServer()
    {
        InformationHolder Info = FindObjectOfType<InformationHolder>();

        ServerIpAddress = Info.GetIpAddress();
        ServerPort = Info.GetPort();

        // Start TcpServer background thread 
        tcpListenerThread = new Thread(new ThreadStart(ServerListener));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    private void ServerListener()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Any, ServerPort);
            //tcpListener = new TcpListener(IPAddress.Parse(ServerIpAddress), ServerPort);
            tcpListener.Start(1);

            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<UIManager>().ServerUICommunication(ServerMessageType.SERVERON, "Servidor Iniciado.");
            });

            Debug.Log("Servidor iniciado.");

            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // CLIENT CONNECTED
                    ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                        FindObjectOfType<UIManager>().ClientConnectedUpdateLoadingServerUI();
                    });

                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);

                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            Debug.Log("client message received as: " + clientMessage);

                            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                                FindObjectOfType<MessageInterpreter>().ParseMessageReceived(clientMessage);
                            });
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            ExecuteOnMainThread.RunOnMainThread.Enqueue(() => {
                FindObjectOfType<ErrorReport>().ShowSocketError(socketException.ToString());
            });
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

    public void MessageToSend(string message)
    {
        byte[] messageToSendAsByteArray = Encoding.ASCII.GetBytes(message);
        SendMessage(messageToSendAsByteArray);
    }

    public void CloseServer()
    {
        // Send msg to client
        if (tcpListener != null)
            tcpListener.Stop();
    }

    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    private void SendMessage(byte[] messageToSend)
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.               
                stream.Write(messageToSend, 0, messageToSend.Length);
                Debug.Log("Server sent his message - should be received by client");
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
}
