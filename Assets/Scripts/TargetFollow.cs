// 
// Copyright (c) 2023 Kim Hyun Deok
//
// TargetFollow.cs
//
// Follow the specific object.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [SerializeField] private Transform followerTransform; // cache the transform of this object. 
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private Quaternion angle = new Quaternion(0f, 0f, 0f, 0f);
    [SerializeField] private float speed = 10f;
    [SerializeField] private float threshold = 0.05f;

    private Transform targetTransform;
    private bool IsFollowing = true;

    private void Awake()
    {
        if (!followerTransform)
        {
            Debug.LogWarning("follower Transform is null!");
        }

        followerTransform.rotation = angle;
        SetTarget(objectToFollow);
    }

    public void SetTarget(GameObject go)
    {
        targetTransform = go.transform;
        objectToFollow = go;
    }

    public void SetTarget(Transform tr)
    {
        targetTransform = tr;
        objectToFollow = tr.gameObject;
    }

    private void LateUpdate()
    {
        if (!followerTransform)
        {
            return;
        }

        if (IsFollowing)
        {
            if (smoothFollow)
            {
                followerTransform.position = Vector3.Lerp(followerTransform.position, targetTransform.position, Time.deltaTime * speed);

                if ((followerTransform.position - targetTransform.position).sqrMagnitude < threshold)
                {
                    followerTransform.SetPositionAndRotation(targetTransform.position, angle);
                }
            }
            else
            {
                followerTransform.SetPositionAndRotation(targetTransform.position, angle);
            }
        }


    }
    
}
