using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Google.Protobuf;

public class GameSocket : MonoBehaviour
{
    Socket clientSocket;
    bool connected;

    public UnityEngine.UI.InputField inputField;

    public string IP = "192.168.110.234";
    public int port = 12000;

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
        //Handshake();
    }

    //private void Handshake()
    //{
    //    SendData(MessageID.GameSystem + ":" + MessageID.GameSystem_ShakeHand+":");
    //}

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

                int size = receive[0] * 256 + receive[1];
                byte[] dataByte = new byte[size];
                System.Array.Copy(receive, 2, dataByte, 0, size);

                ReceiveSocketData(dataByte);

            }
            clientSocket.Close();
            clientSocket = null;
            Debug.Log("close");
        });
        thread.Start();
    }

    private void ReceiveSocketData(byte[] dataByte)
    {
        int msgid = dataByte[0] * 256 + dataByte[1];
        byte[] msgByte = new byte[dataByte.Length - 2];
        System.Array.Copy(dataByte, 2, msgByte, 0, msgByte.Length);
        switch (msgid)
        {
            case 10001:
                Item.Buy ret = Item.Buy.Parser.ParseFrom(msgByte);
                Debug.Log(ret);
                break;
            default:
                Debug.Log("???");
                break;
        }
    }

    public void OnCloseClick()
    {
        SendSocketData_("close");
        connected = false;
    }

    public void TestSend()
    {
        SendMsg("Item.Use", new Item.Use
        {
            Id = 10,
            Num = int.Parse(inputField.text)
        });
    }

    void SendMsg(string msgName, IMessage msg)
    {
        Debug.Log(msg);

        byte[] msgByteArray = msg.ToByteArray();
        byte[] dataArray = new byte[msgByteArray.Length + 2];

        int protoID = MsgList.Protocol(msgName);

        dataArray[0] = (byte)((protoID >> 8) & 0xFF);
        dataArray[1] = (byte)(protoID & 0xFF);

        System.Array.Copy(msgByteArray, 0, dataArray, 2, msgByteArray.Length);
        SendSocketData(dataArray);
    }

    void SendSocketData(byte[] dataByte)
    {

        byte[] buff = new byte[dataByte.Length + 2];

        buff[0] = (byte)((dataByte.Length >> 8) & 0xFF);
        buff[1] = (byte)(dataByte.Length & 0xFF);

        System.Array.Copy(dataByte, 0, buff, 2, dataByte.Length);

        clientSocket.Send(buff);
    }

    public void SendSocketData_(string data)
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
