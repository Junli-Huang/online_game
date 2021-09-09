using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Connect : MonoBehaviour
{
    Socket clientSocket;
    public string text;
    bool connected;
    public void OnConnetClick()
    {
        String IP = "192.168.2.234";
        int port = 12223;

        IPAddress ip = IPAddress.Parse(IP);  //将IP地址字符串转换成IPAddress实例
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//使用指定的地址簇协议、套接字类型和通信协议
        IPEndPoint endPoint = new IPEndPoint(ip, port); // 用指定的ip和端口号初始化IPEndPoint实例
        clientSocket.Connect(endPoint);  //与远程主机建立连接
        

        StartReceiveThread();

    }

    public void OnSendClick()
    {
        SendMes(text);
        SendMes(text);
    }

    public void OnCloseClick()
    {
        SendMes("close");
        connected = false;
        
    }

    void StartReceiveThread()
    {
        connected = true;

        Thread thread = new Thread(() => {
            while (connected)
            {

                byte[] receive = new byte[1024];
                int length = clientSocket.Receive(receive);  // length 接收字节数组长度
                Debug.Log("接收消息为：" + Encoding.ASCII.GetString(receive));
            }
            clientSocket.Close();
            clientSocket = null;
            Debug.Log("close");
        });
        thread.Start();
    }

    void SendMes(string ms)
    {
        if (clientSocket!=null)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(ms);
            clientSocket.Send(data);
        }
    }
}
