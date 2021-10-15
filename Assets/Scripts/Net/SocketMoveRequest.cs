using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketMoveRequest : MonoBehaviour
{


    public GameSocket socket;

    private void Update()
    {
        float forwardMovement = Input.GetAxis("Vertical");
        if (forwardMovement != 0)
        {
            socket.SendSocketData_(GameSocket.MessageID.Movement + ":" + GameSocket.MessageID.Movement_Translate + ":" + forwardMovement);
        }

        float turnMovement = Input.GetAxis("Horizontal");
        if (turnMovement != 0)
        {
            socket.SendSocketData_(GameSocket.MessageID.Movement + ":" + GameSocket.MessageID.Movement_Rotate + ":" + turnMovement);
        }
    }

}

