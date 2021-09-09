using System;
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
    void Start()
    {
        type = GameSocket.MessageID.Shoot;
        inventory = GetComponent<Inventory>();
        socket.AddMessageHandle(this);
    }
    // Update is called once per frame
    void Update()
    {

        while (MsgCount() > 0)
        {
            MsgDequeue();

            Shoot();
        }
    }

    private void Shoot()
    {
        Rigidbody bulletInstance = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bulletInstance.AddForce(shootPoint.forward * bulletSpeed);
        inventory.stuff.bullets--;


        Destroy(bulletInstance.gameObject, 10f);
    }
}
