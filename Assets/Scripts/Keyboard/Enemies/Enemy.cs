﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Keyboard
{
    [RequireComponent(typeof(BoxCollider))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float m_Health = 100;
        public float m_JumpInterval = 3f;
        private float m_LastJumpTimestamp = 0f;
        private bool m_IsCoolingDown
        {
            get
            {
                return Time.time - m_LastJumpTimestamp < m_JumpInterval;
            }
        }

        public Material m_Material;

        public Color m_ExplosionColor;
        public float m_ExplosionDelay = 3f;
        public int m_ExplosionDamage = 100;
        public Key m_StandingKey;
        private BoxCollider m_Collider;
        private Coroutine m_ExplosionRoutine = null;

        private void Awake() 
        {
            m_Collider = GetComponent<BoxCollider>();
            m_Material = GetComponent<Renderer>().material;
        }

        private void Update()
        {
            if(m_ExplosionRoutine != null) return;

            if(m_Health <= 0)
            {
                ExplosionImmediate();
                return;
            }

            if(m_StandingKey && !m_IsCoolingDown)
            {
                JumpOrExplosion();
            }
        }

        public void Initialize(Key key)
        {
            m_StandingKey = key;
            m_LastJumpTimestamp = Time.time;
            transform.position = ComputeStandPosition(key);
        }

        public void TakeDamage(int damage)
        {
            m_Health -= damage;
        }

        private Key FindDestnationKey()
        {
            Vector3 direction = Vector3.left;

            foreach(var other in m_StandingKey.m_KeyLink)
            {
                Vector3 keyDirection = other.transform.position - m_StandingKey.transform.position;
                keyDirection.Normalize();

                // TODO: Adjust
                if(Vector3.Dot(direction, keyDirection) > 0.75f)
                {
                    // Get the left key
                    return other;
                }
            }

            return null;
        }

        private Vector3 ComputeStandPosition(Key key)
        {
            Vector3 position = key.transform.position;
            position += key.m_TopSurfaceOffset * Vector3.up;
            position += m_Collider.bounds.extents.y * Vector3.up;

            return position;
        }

        void JumpOrExplosion()
        {
            var key = FindDestnationKey();

            if(key)
            {
                if(key.m_ItemCount > 0)
                {
                    // Reach the wall, explosion
                    Explosion();
                }
                else
                {
                    Jump(key);
                }
            }
            else 
            {
                // found nothing, explosion now
                Explosion();
            }
        }

        private void Jump(Key to)
        {
            m_StandingKey = to;
            Vector3 position = ComputeStandPosition(to);
            transform.DOJump(position, 1.0f, 1, 0.4f);
            m_LastJumpTimestamp = Time.time;
        }

        private void Explosion()
        {
            m_ExplosionRoutine = StartCoroutine(ExplosionDelay());
        }

        private void ExplosionImmediate()
        {
            GameManager.Instance.EnemeyDidExplosion();
            if(m_StandingKey) m_StandingKey.EnemeyDidExplosion(this);
            DestroyImmediate(gameObject);
        }

        private IEnumerator ExplosionDelay()
        {
            m_Material.SetColor("_BaseColor", m_ExplosionColor);
            yield return new WaitForSeconds(m_ExplosionDelay);
            // Explosion
            ExplosionImmediate();
        }
    }   
}
