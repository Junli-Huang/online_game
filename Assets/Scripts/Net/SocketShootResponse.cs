using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class SocketShootResponse : MessageQueue
{
    public Rigidbody bulletPrefab;
    private Inventory inventory;
    public Transform shootPoint;
    public float bulletSpeed = 10;

    public GameSocket socket;
    //private MessageQueue messageHandle = new MessageQueue(GameSocket.MessageID.Shoot);
    void Start()
    {
        type = GameSocket.MessageID.Shoot;
        socket.AddMessageHandle(this);
    }
    // Update is called once per frame
    void Update()
    {

        while (socket.movements.Count > 0)
        {
            GameSocket.Movement movement = socket.movements.Peek();

            if (movement.type == (int)GameSocket.MessageID.Shoot)
            {
                socket.movements.Dequeue();

                Rigidbody bulletInstance = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                bulletInstance.AddForce(shootPoint.forward * bulletSpeed);
                inventory.stuff.bullets--;


                Destroy(bulletInstance.gameObject, 10f);
            }
        }
    }
}
