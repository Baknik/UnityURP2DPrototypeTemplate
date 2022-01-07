using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AntiGravity : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.up * 20f);
    }
}
