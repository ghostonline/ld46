using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public Sprite m_emptyBowl;
    public Sprite m_fullBowl;
    public SpriteRenderer m_renderer;
    public Food m_foodState;

    public void Awake()
    {
        SetFull(false);
    }

    public bool IsFull()
    {
        return m_foodState.m_edible;
    }

    public void Update()
    {
        if (!m_foodState.m_edible)
        {
            SetFull(false);
        }
    }

    public void Put(Carryable carryable)
    {
        GameObject.Destroy(carryable.gameObject);
        SetFull(true);
    }

    private void SetFull(bool value)
    {
        if (value)
        {
            m_renderer.sprite = m_fullBowl;
            m_foodState.m_edible = true;
        }
        else
        {
            m_renderer.sprite = m_emptyBowl;
            m_foodState.m_edible = false;
        }
    }
}
