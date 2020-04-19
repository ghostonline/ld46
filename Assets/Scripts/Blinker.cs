using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public UnityEngine.UI.Text m_textRender;
    public float m_blinkSpeed;

    float m_timer = 0f;

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_blinkSpeed)
        {
            m_textRender.enabled = !m_textRender.enabled;
            m_timer -= m_blinkSpeed;
        }
    }
}
