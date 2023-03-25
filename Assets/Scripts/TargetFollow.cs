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
    [SerializeField] private GameObject objectToFollow;
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private Vector3 offset = new Vector3(10f,0f,0f);
    [SerializeField] private Quaternion angle = new Quaternion(0f, 0f, 0f, 0f);
    [SerializeField] private float speed = 10f;

    private Transform targetTransform;
    private Transform followerTransform; // cache the transform of this object. 
    private bool IsFollowing = true;

    private void Awake()
    {
        followerTransform = transform;

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

    private void Update()
    {
        if (IsFollowing)
        {
            if (smoothFollow)
            {
                followerTransform.position = Vector3.Lerp(followerTransform.position, targetTransform.position + offset, Time.deltaTime * speed);
                followerTransform.rotation = Quaternion.Lerp(followerTransform.rotation, angle, Time.deltaTime * speed);

                if ((followerTransform.position - targetTransform.position).sqrMagnitude < 0.02f)
                {
                    followerTransform.position = targetTransform.position + offset;
                    followerTransform.rotation = angle;
                }
            }
            else
            {
                followerTransform.position = targetTransform.position + offset;
                followerTransform.rotation = angle;
            }
        }


    }
    
}
