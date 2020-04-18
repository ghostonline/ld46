using UnityEngine;

public class Carryable : MonoBehaviour
{
    public Transform m_owner;
    public Food m_food;

    private Rigidbody2D m_rigidbody;
    private Transform m_originalParent;

    public void Start()
    {
        m_owner = m_owner != null ? m_owner : transform;
        m_originalParent = m_owner.parent;
        m_rigidbody = m_owner.GetComponent<Rigidbody2D>();
        m_food = m_food != null ? m_food : GetComponent<Food>();
    }

    public void StartCarry(Transform slot)
    {
        if (m_rigidbody)
        {
            m_rigidbody.simulated = false;
        }
        m_owner.parent = slot;
        m_owner.localPosition = Vector3.zero;
    }

    public void StopCarry()
    {
        m_owner.parent = m_originalParent;
        if (m_rigidbody)
        {
            m_rigidbody.simulated = true;
        }
    }

    public bool isFood
    {
        get
        {
            return m_food && m_food.m_edible;
        }
    }
}
