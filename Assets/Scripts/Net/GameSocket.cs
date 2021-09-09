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
    public int port = 12223;

    private Dictionary<int, MessageQueue> messageQueue = new Dictionary<int, MessageQueue>();

    public void AddMessageHandle(MessageQueue handle)
    {
        messageQueue[handle.type] = handle;
    }

    public class MessageID
    {
        public const int Movement           =   10000;
        public const int Movement_Translate =   10001;
        public const int Movement_Rotate    =   10002;

        public const int Shoot              =   20000;
    }

    public void Connect()
    {
        InitSocket(); 
        StartReceiveThread();
    }

    private void InitSocket()
    {
        IPAddress ip = IPAddress.Parse(IP);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(ip, port);
        clientSocket.Connect(endPoint);
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

                CtrlData(data);

            }
            clientSocket.Close();
            clientSocket = null;
            Debug.Log("close");
        });
        thread.Start();
    }

    private void CtrlData(string data)
    {
        int sizeIndex = data.IndexOf(":");

        while (sizeIndex != -1)
        {
            int size = int.Parse(data.Substring(0, sizeIndex));
            string str = data.Substring(0, size);

            int messageIndex = str.IndexOf(":");
            string message = data.Substring(messageIndex + 1);
            ProcessMessage(message);

            data = data.Substring(size);

            sizeIndex = data.IndexOf(":");
        }
    }

    public class Movement
    {
        public int type { get; }
        public float value { get; }

        public Movement(int type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public Queue<Movement> movements = new Queue<Movement>();
    public void ProcessMessage(string data)
    {
        Debug.Log("接收消息为：" + data);
        int idx = data.IndexOf(":");

        int id = int.Parse(data.Substring(0, idx));
        string content = data.Substring(idx + 1);
        if (messageQueue[id] != null)
        {
            messageQueue[id].Enqueue(content);
        }
        //movements.Enqueue(new Movement(type,value));

    }


    public void OnCloseClick()
    {
        SendData("close");
        connected = false;
        
    }

  

    public void SendData(string data)
    {


        int length = data.Length + 5;

        string strLen = length + "";
        switch (strLen.Length){
            case 1:
                strLen = "000" + strLen;
                break;
            case 2:
                strLen = "00" + strLen;
                break;
            case 3:
                strLen = "0" + strLen;
                break;
        }
        data = strLen + ":" + data;

        if (clientSocket!=null)
        {
            byte[] buff = new byte[length];
            buff = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(buff);
        }
    }
}
