using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public float rotSpeed = 4.5f;

    [SerializeField]
    Transform target;

    Vector3 _offset;
    float _rotY = 0;
    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        _rotY += Input.GetAxis("Mouse X") * rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);

        transform.position = target.position + rotation * _offset;
        transform.LookAt(target);
    }
}
