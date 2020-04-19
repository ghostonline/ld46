using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float m_duration;
    public float m_angle;
    public float m_angleVariation;
    public float m_speed;

    float m_lifetime = 0f;
    Vector2 m_movement;

    void Start()
    {
        var angleRad = Mathf.Deg2Rad * m_angle;
        var variantRad = Mathf.Deg2Rad * m_angleVariation;
        var variant = (Random.value - 0.5f) * variantRad;
        m_movement.x = Mathf.Cos(angleRad + variant);
        m_movement.y = Mathf.Sin(angleRad + variant);
    }

    void Update()
    {
        m_lifetime += Time.deltaTime;
        if (m_lifetime >= m_duration)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)m_movement * m_speed * Time.fixedDeltaTime;
    }
}
