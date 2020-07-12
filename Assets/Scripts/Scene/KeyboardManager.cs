using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keyboard
{
    public class KeyboardManager : SceneSingleton<KeyboardManager>
    {

        private Dictionary<KeyCode, Key> mKeyList = new Dictionary<KeyCode, Key>();

        public void RegisterKey(KeyCode code, Key key)
        {
            mKeyList.Add(code, key);
        }

        private void Update() 
        {
            foreach(var pair in mKeyList)
            {
                if(Input.GetKeyUp(pair.Key))
                {
                    // Move toppest item in spawn to here
                    SpawnManager.Instance.m_ItemSpawner.KeyTransfer(pair.Value);
                    return;
                }
            }
        }

    }
}
