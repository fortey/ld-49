using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] public bool follow;

    public void StartFollow()
    {
        follow = true;
    }

    
    void Update()
    {
        if(follow)
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }
}
