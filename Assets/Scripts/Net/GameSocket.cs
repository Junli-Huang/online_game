using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class GameSocket : MonoBehaviour
{
    Socket clientSocket;
    bool connected;

    public string IP = "192.168.2.234";
    public int port = 12224;

    private Dictionary<int, MessageQueue> messageQueue = new Dictionary<int, MessageQueue>();

    public void AddMessageHandle(MessageQueue handle)
    {
        messageQueue[handle.type] = handle;
    }

    public class MessageID
    {
        public const int GameSystem                 =   10000;
        public const int GameSystem_ShakeHand       =   10001;
        public const int GameSystem_CreatePlayer    =   10002;

        public const int Movement                   =   20000;
        public const int Movement_Translate         =   20001;
        public const int Movement_Rotate            =   20002;

        public const int Shoot                      =   30000;
    }

    public void Connect()
    {
        InitSocket(); 
        StartReceiveThread();
        Handshake();
    }

    private void Handshake()
    {
        SendData(MessageID.GameSystem + ":" + MessageID.GameSystem_ShakeHand+":");
    }

    private void InitSocket()
    {
        IPAddress ip = IPAddress.Parse(IP);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(ip, port);
        clientSocket.Connect(endPoint);

  

        // Using the RemoteEndPoint property.
        Debug.Log("I am connected to " + IPAddress.Parse(((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString()) + "on port number " + ((IPEndPoint)clientSocket.RemoteEndPoint).Port.ToString());

        // Using the LocalEndPoint property.
        Debug.Log("My local IpAddress is :" + IPAddress.Parse(((IPEndPoint)clientSocket.LocalEndPoint).Address.ToString()) + "I am connected on port number " + ((IPEndPoint)clientSocket.LocalEndPoint).Port.ToString());
    }

    void StartReceiveThread()
    {
        connected = true;

        Thread thread = new Thread(() => {
            while (connected)
            {

                byte[] receive = new byte[1024];
                int length = clientSocket.Receive(receive);  // length 接收字节数组长度
                string data = Encoding.ASCII.GetString(receive);

                ReadData(data);

            }
            clientSocket.Close();
            clientSocket = null;
            Debug.Log("close");
        });
        thread.Start();
    }

    private void ReadData(string data)
    {
        //Debug.Log(data);
        int sizeIndex = data.IndexOf(":");

        while (sizeIndex != -1)
        {
            int size = int.Parse(data.Substring(0, sizeIndex));
            string str = data.Substring(0, size);

            int messageIndex = str.IndexOf(":");
            string message = data.Substring(messageIndex + 1, size - messageIndex - 1);
            ProcessMessage(message);

            data = data.Substring(size);

            sizeIndex = data.IndexOf(":");
        }
    }

    public void ProcessMessage(string data)
    {
        //Debug.Log("接收消息为：" + data);
        int idx = data.IndexOf(":");

        int id = int.Parse(data.Substring(0, idx));
        string content = data.Substring(idx + 1);
        if (messageQueue[id] != null)
        {
            messageQueue[id].MsgEnqueue(content);
        }

    }


    public void OnCloseClick()
    {
        SendData("close");
        connected = false;
        
    }

    public void SendData(string data)
    {

        int length = data.Length;
        length = length + (length + "").Length + 1;

        data = length + ":" + data;

        if (clientSocket!=null)
        {
            byte[] buff = new byte[length];
            buff = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(buff);
        }
    }
}
