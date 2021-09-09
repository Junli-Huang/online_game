using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class SocketShootRequest : MessageQueue
{

    private Inventory inventory;

    public GameSocket socket;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && inventory.stuff.bullets>0)
        {
            socket.SendData(GameSocket.MessageID.Shoot+":0");
        }
    }
}
