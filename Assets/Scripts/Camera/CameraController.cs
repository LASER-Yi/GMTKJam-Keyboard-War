using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SceneSingleton<CameraController>
{
    private AudioSource m_CameraAudio;

    public AudioSource m_PlayerAudio;
    public AudioSource m_EnemyAudio;

    private void Awake() 
    {
        m_CameraAudio = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        m_CameraAudio.clip = clip;
        m_CameraAudio.Play();
    }

    public void PlayPlayerSound(AudioClip clip)
    {
        m_PlayerAudio.clip = clip;
        m_PlayerAudio.Play();
    }

    public void PlayEnemySound(AudioClip clip)
    {
        m_EnemyAudio.clip = clip;
        m_EnemyAudio.Play();
    }
}
