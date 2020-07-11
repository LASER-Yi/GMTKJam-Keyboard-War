using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float m_FlySpeed = 10.0f;
    public float m_Damage = 35.0f;
    private void OnCollisionEnter(Collision other) 
    {
        Destroy(gameObject);
    }

    private float m_Distance = 1000.0f;

    private void Update() 
    {
        Vector3 delta = transform.forward * m_FlySpeed * Time.deltaTime;
        transform.position += delta;
    }

    private void Start() 
    {
        float time = m_Distance / m_FlySpeed;
        Destroy(gameObject, time + 1.0f);
    }
}
