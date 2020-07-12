using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float m_FlySpeed = 10.0f;
    public int m_Damage = 35;
    public Transform m_Particle;

    private void OnCollisionEnter(Collision other) 
    {
        Keyboard.Enemy enemy;
        other.collider.TryGetComponent<Keyboard.Enemy>(out enemy);

        if(enemy)
        {
            enemy.TakeDamage(m_Damage);
        }
        Destroy(gameObject);
    }

    private float m_Distance = 1000.0f;

    private void Update() 
    {
        Vector3 delta = transform.forward * m_FlySpeed * Time.deltaTime;
        transform.position += delta;

        m_Particle.position = transform.position;
    }

    private void Start() 
    {
        float time = m_Distance / m_FlySpeed;
        m_Particle.parent = null;
        Destroy(gameObject, time + 1.0f);
    }

    private void OnDestroy() 
    {
        var ps = m_Particle.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
        
        Destroy(m_Particle.gameObject, 2.0f);
    }
}
