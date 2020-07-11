using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class TowerItem : BaseItem
    {
        [Header("Shoot")]
        public float m_ShootInterval = 0.6f;
        public int m_ShootHeat = 5;
        public GameObject m_BulletPrefab;

        public Transform m_StartAt;

        [Tooltip("Degree")]
        public float m_RotationSpeed = 90.0f;

        public Transform m_RotateRig;

        [Header("Search")]
        // Radius boost when placing in higher floor
        public float m_ScanRadius = 10.0f;
        public float m_ForceScanInterval = 1.0f;
        private float m_RescanTimer = 0;

        [Header("Enemy")]
        public LayerMask m_EmemyLayer;

        private Enemy m_CurrentEnemy;

        // Private
        private bool m_Activation = false;

        public override void Activate()
        {
            m_Activation = true;
        }

        public override void Deactivate()
        {
            m_Activation = false;
            m_CurrentEnemy = null;
        }

        protected new void Update()
        {
            base.Update();

            // Update scanner status
            bool forceScan = Time.time - m_RescanTimer > m_ForceScanInterval;
            if(m_Activation && (!m_CurrentEnemy || forceScan))
            {
                ScanForEnemy();
                m_RescanTimer = Time.time;
            }

            if(m_CurrentEnemy)
            {
                // found enemy
                PointAndShoot();
            }
        }

        private float m_LastShootTimestamp = 0;

        private bool m_IsCoolingDown
        {
            get
            {
                return Time.time - m_LastShootTimestamp < m_ShootInterval;
            }
        }

        private void PointAndShoot()
        {
            Vector3 direction = m_CurrentEnemy.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

            m_RotateRig.rotation = Quaternion.RotateTowards(m_RotateRig.rotation, rotation, m_RotationSpeed * Time.deltaTime);


            // Check if current rotation within line of sight

            float angle = Quaternion.Angle(m_RotateRig.rotation, rotation);
            if(angle <= 1.0f)
            {
                TryShoot();
            }
        }

        private void TryShoot()
        {
            if(m_IsCoolingDown) return;

            if(!m_BulletPrefab && m_BulletPrefab.TryGetComponent<Bullet>(out _)) return;

            var ray = new Ray(m_StartAt.position, m_RotateRig.forward);

            // Use Capsule maybe
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000.0f))
            {
                if(hit.collider.TryGetComponent<Enemy>(out _))
                {
                    m_LastShootTimestamp = Time.time;
                    ShootBullet();
                }
            }
        }

        private void ShootBullet()
        {
            Instantiate(m_BulletPrefab, m_StartAt.position, m_StartAt.rotation, null);
            this.Damage(m_ShootHeat);
        }

        private float GetScanRadius()
        {
            // Add Boost later
            float boost = 1.0f;
            if(m_LinkedKey)
            {
                // TODO: Adjust
                boost += 0.2f * Mathf.Max(m_LinkedKey.GetFloorIndex(this), 0);
            }
            return m_ScanRadius * boost;
        }

        private void ScanForEnemy()
        {
            var colliders = Physics.OverlapSphere(transform.position, GetScanRadius(), m_EmemyLayer);

            if(colliders.Length > 0)
            {
                foreach(var item in colliders)
                {
                    Enemy script;
                    colliders[0].TryGetComponent<Enemy>(out script);
                    if(script)
                    {
                        m_CurrentEnemy = script;
                    }
                }
            }
        }
    }
}
