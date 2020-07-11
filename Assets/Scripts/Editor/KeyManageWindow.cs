using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Keyboard
{
    public class KeyManageWindow : EditorWindow
    {
        [MenuItem("Tools/Keyboard/Key Editor")]
        static public void Open()
        {
            GetWindow<KeyManageWindow>();
        }

        private int GetSelectedKey(out Key[] outKeys)
        {
            var selectedList = Selection.gameObjects;

            var keyList = new List<Key>();

            foreach (var selected in selectedList)
            {
                bool isKey = selected.TryGetComponent<Key>(out _);
                if (isKey)
                {
                    keyList.Add(selected.GetComponent<Key>());
                    continue;
                }
                else
                {
                    break;
                }
            }

            outKeys = keyList.ToArray();
            return outKeys.Length;
        }

        private void OnGUI()
        {
            var selectedCount = GetSelectedKey(out _) >= 1;

            if (selectedCount)
            {
                EditorGUILayout.HelpBox("Key Help", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Select two key objects to start", MessageType.Error);
            }
            EditorGUILayout.BeginVertical("box");

            EditorGUI.BeginDisabledGroup(!selectedCount);
            DrawKeyManager();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        private void DrawKeyManager()
        {
            // get first two Key of selecting object
            Key[] keyList;
            GetSelectedKey(out keyList);

            EditorGUI.BeginDisabledGroup(keyList.Length != 2);
            if (GUILayout.Button("Link Key"))
            {
                Undo.RecordObjects(keyList, "Link Key");
                Link(keyList[0], keyList[1]);
                PrefabUtility.RecordPrefabInstancePropertyModifications(keyList[0]);
                PrefabUtility.RecordPrefabInstancePropertyModifications(keyList[1]);
            }

            if(GUILayout.Button("Unlink Key"))
            {
                Undo.RecordObjects(keyList, "Unlink Key");
                Unlink(keyList[0], keyList[1]);
                PrefabUtility.RecordPrefabInstancePropertyModifications(keyList[0]);
                PrefabUtility.RecordPrefabInstancePropertyModifications(keyList[1]);
            }

            EditorGUI.EndDisabledGroup();

            
        }

        private void Link(Key lhs, Key rhs)
        {
            lhs.m_KeyLink.Add(rhs);
            rhs.m_KeyLink.Add(lhs);
        }

        private void Unlink(Key lhs, Key rhs)
        {
            lhs.m_KeyLink.Remove(rhs);
            rhs.m_KeyLink.Remove(lhs);
        }
    }
}
