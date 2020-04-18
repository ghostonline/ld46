using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChompAIState
{
    Sleeping,
    Active,
}

public class ChompAI : MonoBehaviour
{
    public CharacterController2D m_controller;
    public float m_speed = 1f;
    public float m_hopForce = 50f;
    public float m_jumpForce = 250f;
    public Transform m_anchor;
    public float m_chainLength = 3f;
    public ChompAIState m_initialState = ChompAIState.Active;
    public bool m_fixedAnchor = false;

    float m_movement = 0f;
    Rigidbody2D m_rigidbody;
    ChompAIState m_state;
    Rigidbody2D m_anchorRigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_state = m_initialState;
    }

    private void Start()
    {
        m_anchorRigidbody = m_anchor.GetComponent<Rigidbody2D>();
        if (m_fixedAnchor)
        {
            m_anchorRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (m_state == ChompAIState.Active)
        {
            var target = FindNearestEdible();
            if (target)
            {
                ChaseTarget(target);
            }
            else
            {
                Idle();
            }
        }
    }

    Transform FindNearestEdible()
    {
        Food nearest = null;
        float nearestDistance = float.PositiveInfinity;
        foreach(var food in FindObjectsOfType<Food>())
        {
            float distance = Vector3.Distance(food.transform.position, transform.position);
            if (food.m_edible && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = food;
            }
        }

        return nearest ? nearest.transform : null;
    }

    void ChaseTarget(Transform target)
    {
        Vector3 targetOffset = target.position - transform.position;

        m_movement = Mathf.Clamp(targetOffset.x, -m_speed, m_speed);

        if (targetOffset.y > 2f && Mathf.Abs(targetOffset.x) < 4f)
        {
            m_controller.JumpForce = m_jumpForce;
        }
        else
        {
            m_controller.JumpForce = m_hopForce;
        }
    }

    void Idle()
    {
        m_movement = 0f;
        m_controller.JumpForce = m_hopForce;
    }

    private void FixedUpdate()
    {
        if (m_state == ChompAIState.Active)
        {
            m_controller.Move(m_movement, false, true);
        }

        if (m_anchor)
        {
            Vector2 anchorOffset = transform.position - m_anchor.position;
            var currentChainLength = anchorOffset.magnitude;
            if (currentChainLength > m_chainLength)
            {
                if (m_fixedAnchor)
                {
                    MoveChomp(-anchorOffset);
                }
                else
                {
                    MoveAnchor(anchorOffset);
                }
                m_state = ChompAIState.Active;
            }
        }
    }

    void MoveChomp(Vector2 offset)
    {
        var normal = -offset.normalized;
        if (m_rigidbody.velocity.sqrMagnitude != 0)
        {
            float dot = Vector2.Dot(m_rigidbody.velocity, normal);
            if (dot >= 0f)
            {
                m_rigidbody.AddForce(offset, ForceMode2D.Impulse);
            }
        }
        else
        {
            m_rigidbody.MovePosition(m_anchor.position + (Vector3)(normal * m_chainLength));
        }
    }

    void MoveAnchor(Vector2 offset)
    {
        var rigidbody = m_anchor.GetComponent<Rigidbody2D>();
        var normal = -offset.normalized;
        if (rigidbody.velocity.sqrMagnitude != 0)
        {
            float dot = Vector2.Dot(rigidbody.velocity, normal);
            if (dot >= 0f)
            {
                rigidbody.AddForce(offset, ForceMode2D.Impulse);
                rigidbody.AddTorque((Random.value - 0.5f) * 10f);
            }
        }
        else
        {
            rigidbody.AddForce(offset, ForceMode2D.Impulse);
        }
    }

    public void WakeUp()
    {
        if (m_state == ChompAIState.Sleeping)
        {
            m_state = ChompAIState.Active;
        }
    }
}
