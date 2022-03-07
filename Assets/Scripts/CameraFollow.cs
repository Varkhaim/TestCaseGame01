using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ObjectToFollow;
    private Vector3 followedPosition;

    private void Start()
    {
        followedPosition.z = transform.position.z;
    }

    private void Update()
    {
        FollowObject();
    }

    private void FollowObject()
    {
        followedPosition.x = ObjectToFollow.position.x;
        followedPosition.y = ObjectToFollow.position.y;

        transform.position = followedPosition;
    }
}
