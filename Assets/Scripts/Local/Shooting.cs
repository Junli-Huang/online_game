using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Shooting : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    private Inventory inventory;
    public Transform shootPoint;
    public float bulletSpeed = 10;

    void Start()
    {
        inventory = GetComponent<Inventory>();    
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && inventory.stuff.bullets>0)
        {
            Rigidbody bulletInstance = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bulletInstance.AddForce(shootPoint.forward * bulletSpeed);
            inventory.stuff.bullets--;

 
            Destroy(bulletInstance.gameObject, 10f);
        }
    }
}
