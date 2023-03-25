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
    [SerializeField] private Vector3 offset;
    [SerializeField] private float sqrDistance;
    [SerializeField] private float speed;

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
        float sqrMagnitude = (followerTransform.position - targetTransform.position).sqrMagnitude;
        if (sqrMagnitude < 0.02f)
        {
            IsFollowing = false;
        }
        else
        {
            IsFollowing = true;
        }

        if (IsFollowing)
        {
            followerTransform.position = Vector3.Lerp(followerTransform.position, targetTransform.position, Time.deltaTime * speed);
        }


    }
    
}
