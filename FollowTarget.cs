using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    //follow the player
    public Transform target;

    Vector3 velocity = Vector3.zero;

    //camera dampner
    public float smoothTime = 15f;

     void FixedUpdate()
    {
        //target position
        Vector3 targetPos = target.position;

        //align the camera and the targets z position
        targetPos.z = transform.position.z;

        //dampners
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);


    }
}
