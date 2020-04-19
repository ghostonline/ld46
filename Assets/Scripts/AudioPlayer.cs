using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clip
{
    Die,
    Drop,
    FillBowl,
    Munch,
    Pickup,
}

public class AudioPlayer : MonoBehaviour
{
    private static AudioPlayer m_instance;

    public AudioSource m_source;
    public AudioClip m_die;
    public AudioClip m_drop;
    public AudioClip m_fillBowl;
    public AudioClip m_munch;
    public AudioClip m_pickup;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void Play(Clip clipId)
    {
        var clip = m_instance.GetClip(clipId);
        m_instance.m_source.PlayOneShot(clip);
    }

    private AudioClip GetClip(Clip clipId)
    {
        AudioClip[] clips =
        {
            m_die,
            m_drop,
            m_fillBowl,
            m_munch,
            m_pickup,
        };

        return clips[(int)clipId];
    }
}
