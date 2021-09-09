using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public int type { set; get; }
    private Queue<string> msgs = new Queue<string>();


    internal void MsgEnqueue(string content)
    {
        msgs.Enqueue(content);
    }

    internal string MsgDequeue()
    {
        return msgs.Dequeue();
    }
    internal int MsgCount()
    {
        return msgs.Count;
    }


}
