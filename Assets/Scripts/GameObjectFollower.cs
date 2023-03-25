// 
// Copyright (c) 2023 Kim Hyun Deok
//
// GameObjectFollower.cs
//
// Follow the specific object.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFollower : MonoBehaviour
{
    public bool smoothFollow = true;
    public Vector3 distance;

    private Transform targetTransform;
    [SerializeField] private GameObject objectToFollow;

    public void SetTarget(GameObject target)
    {
        targetTransform = target.transform;
        objectToFollow = target;
    }
    public void SetTarget(Transform target)
    {
        targetTransform = target;
        objectToFollow = target.gameObject;
    }

    private void Update()
    {
    }
}
