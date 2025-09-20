using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float moveSpeed = 10f;
    public Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    }
}
