using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public GameObject target;

    public Vector3 offset;

    private void Update()
    {
        if (target != null) transform.position = target.transform.position + offset;
    }
}
