using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompAI : MonoBehaviour
{
    public CharacterController2D m_controller;
    public float m_speed = 1f;
    public float m_hopForce = 50f;
    public float m_jumpForce = 250f;

    float m_movement = 0f;

    void Update()
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

        return nearest.transform;
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
        m_controller.Move(m_movement, false, true);
    }
}
