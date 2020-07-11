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
        protected float m_CurrentHeat = 0;
        public BoxCollider m_Collider;

        // Single Direction Link Node for Stack
        protected BaseItem m_UpperItems;

        // Mono
        private void Awake()
        {
            m_Collider = GetComponent<BoxCollider>();
        }

        // Stack Link

        // Pointer Event
        protected bool m_IsDragging = false;
        [Header("Basic Layer")]

        public LayerMask m_GroundLayer;

        protected void Update()
        {
            if (m_IsDragging)
            {
                if (m_DraggingDest != Vector3.zero)
                {
                    transform.position = Vector3.Lerp(transform.position, m_DraggingDest, 0.1f);
                }
            }
        }

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
                if (key != null)
                {
                    // Snap to key
                    // key.transform.position
                    if (!key.LinkToKey(this))
                    {
                        // Let this item return to its original position
                        transform.DOMove(m_DraggingOrigin, 0.3f, false);
                    }
                    else
                    {
                        StartCoroutine(ResetItem());
                    }
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
        protected virtual void Activate()
        {
            Debug.Log("BaseItem::Activate is no impl");
        }

        protected virtual void Deactivate()
        {
            Debug.Log("BaseItem::Deactivate is no impl");
        }

    }
}
