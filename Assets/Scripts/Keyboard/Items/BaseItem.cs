using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Keyboard
{
    // Create Basic behaviour of item
    [RequireComponent(typeof(BoxCollider))]
    public class BaseItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        // Heat from 0 - 100
        protected float _heat = 0;

        public float m_CurrentHeat
        {
            get
            {
                return _heat;
            }
        }

        [Tooltip("Per Second")]
        public float m_HeatDissipation = 0.1f;

        public BoxCollider m_Collider;

        // Mono
        protected void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
        }

        protected void Update()
        {
            if (m_IsDragging)
            {
                if (m_DraggingDest != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(transform.position, m_DraggingDest, 0.1f);
                }
            }

            // Check heat, if larger then 100 explosion ourself
            if (m_CurrentHeat >= 100.0f)
            {
                Explosion();
                return;
            }

            // Then we dissipation heat
            _heat -= m_HeatDissipation * Time.deltaTime;
            _heat = Mathf.Max(0.0f, _heat);
        }

        // Behaviours

        void Explosion()
        {
            // Tell the key we are going to explosion

        }

        // Stack Link

        // Single Direction Link Node for Stack
        protected BaseItem m_UpperItems;

        protected Key m_LinkedKey;

        // Pointer Event
        protected bool m_IsDragging = false;
        [Header("Basic Layer")]

        public LayerMask m_GroundLayer;

        private Vector3 m_DraggingDest = Vector3.zero;
        private Vector3 m_DraggingOrigin = Vector3.zero;

        public void OnBeginDrag(PointerEventData data)
        {
            m_IsDragging = true;
            m_DraggingOrigin = transform.position;
        }
        public void OnDrag(PointerEventData data)
        {
            Vector3 start = data.pointerCurrentRaycast.worldPosition + Vector3.up * 1.0f;
            Vector3 dir = Vector3.down;

            // TODO: Max Distance
            RaycastHit hit;
            if (Physics.Raycast(start, dir, out hit, 100.0f, m_GroundLayer))
            {
                Vector3 dest = hit.point + Vector3.up * 1.0f;
                m_DraggingDest = dest;
            }
        }

        public void TryLink(Key key)
        {
            // Snap to key
            if (key != null && key != m_LinkedKey && key.LinkToKey(this))
            {
                // delink previous Key
                if(m_LinkedKey != null)
                {
                    m_LinkedKey.RemoveFromKey(this);
                }

                m_LinkedKey = key;

                StartCoroutine(ResetItem());                
            }
            else
            {
                // Let this item return to its original position
                transform.DOMove(m_DraggingOrigin, 0.3f, false);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            m_IsDragging = false;
            m_DraggingDest = Vector3.zero;

            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f, m_GroundLayer))
            {
                // TODO: Add Stack
                Key key;
                hit.transform.TryGetComponent<Key>(out key);
                BaseItem item;
                hit.transform.TryGetComponent<BaseItem>(out item);

                if (key != null)
                {
                    TryLink(key);
                }
                else if (item != null && item.m_LinkedKey != null)
                {
                    TryLink(item.m_LinkedKey);
                }
                else
                {
                    // Let this item return to its original position
                    transform.DOMove(m_DraggingOrigin, 0.3f, false);
                }
            }

            m_DraggingOrigin = Vector3.zero;
        }

        IEnumerator ResetItem()
        {
            this.Deactivate();
            // TODO: Timer animation
            yield return new WaitForSecondsRealtime(0.6f);
            this.Activate();
        }

        // Here's the behaviour for child class
        public virtual void Activate()
        {
            Debug.Log("BaseItem::Activate is no impl");
        }

        public virtual void Deactivate()
        {
            Debug.Log("BaseItem::Deactivate is no impl");
        }

    }
}
