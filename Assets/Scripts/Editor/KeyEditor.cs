using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Keyboard
{
    [CustomEditor(typeof(Key))]
    public class KeyEditor : Editor
    {
        private void OnSceneGUI() 
        {
            var key = target as Key;

            if(key && key.m_KeyLink.Count != 0)
            {
                foreach(var other in key.m_KeyLink)
                {
                    Vector3 start = key.transform.position;
                    Vector3 end = other.transform.position;

                    Handles.color = Color.yellow;
                    Handles.DrawLine(start, end);
                }
            }
        }
    }
}
