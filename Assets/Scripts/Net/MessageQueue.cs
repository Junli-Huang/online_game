using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueue : MonoBehaviour
{
    public int type { set; get; }
    public Queue<string> msgs = new Queue<string>();


    internal void Enqueue(string content)
    {
        msgs.Enqueue(content);
    }

    internal string Dequeue()
    {
        return msgs.Dequeue();
    }
    internal int Count()
    {
        return msgs.Count;
    }


}
