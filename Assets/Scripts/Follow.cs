using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        SetTarget(this.target);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        if (this.target != null)
        {
            this.offset = this.target.position - transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position - offset;
        }
    }
}
