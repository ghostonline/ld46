using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject m_wasd;
    public GameObject m_arrow;

    float m_timer;

    void Start()
    {
        Flip();
    }

    void Flip()
    {
        m_arrow.SetActive(!m_arrow.activeSelf);
        m_wasd.SetActive(!m_arrow.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > 2f)
        {
            Flip();
            m_timer = 0f;
        }
    }
}
