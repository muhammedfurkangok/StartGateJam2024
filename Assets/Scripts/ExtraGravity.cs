﻿using UnityEngine;

public class ExtraGravity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rigidbody;

    [Header("Info")]
    [SerializeField] private float extraGravity;

    private void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * extraGravity);
    }
}