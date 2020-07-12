using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject m_TitlePrefab;
    private GameObject m_TitleInstance;

    public GameObject m_IngamePrefab;
    private GameObject m_IngameInstance;

    public GameObject m_EndPrefab;
    private GameObject m_EndInstance;

    private void Start() 
    {
        m_TitleInstance = Instantiate(m_TitlePrefab, Vector3.zero, Quaternion.identity);
        m_TitleInstance.transform.SetParent(transform, false);
    }

    public void StartGame()
    {
        DestroyImmediate(m_TitleInstance);

        m_IngameInstance = Instantiate(m_IngamePrefab, Vector3.zero, Quaternion.identity);
        m_IngameInstance.transform.SetParent(transform, false);
    }

    public void StopGame()
    {
        DestroyImmediate(m_IngameInstance);

        m_EndInstance = Instantiate(m_EndPrefab, Vector3.zero, Quaternion.identity);
        m_EndInstance.transform.SetParent(transform, false);
    }
}
