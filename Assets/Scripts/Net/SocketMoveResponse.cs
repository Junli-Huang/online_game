using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketMoveResponse : MessageQueue
{

    public float moveSpeed;
    public float rotateSpeed;

    public GameSocket socket;


    private void Start()
    {
        type = GameSocket.MessageID.Movement;
        socket.AddMessageHandle(this);
    }

    private void Update()
    {
        while (msgs.Count > 0)
        {
            string data = msgs.Dequeue();
            int idx = data.IndexOf(":");
            int id = int.Parse(data.Substring(0, idx));
            float value = float.Parse( data.Substring(idx + 1) );


            switch (id)
            {
                case GameSocket.MessageID.Movement_Translate:
                    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * value);
 
                    break;
                case GameSocket.MessageID.Movement_Rotate:
                    transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * value);

                    break;
            }
        }

    }


            
}
