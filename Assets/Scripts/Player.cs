using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D m_controller;
    public Transform m_carrySlot;
    public Collider2D m_actionRadius;

    private float m_horizontal = 0.0f;
    private bool m_jump = false;
    private Carryable m_carriedObject = null;
    private bool m_action = false;

    void Update()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            DoAction();
        }
    }

    private void DoAction()
    {
        var colliders = new List<Collider2D>();
        m_actionRadius.GetContacts(colliders);
        foreach (var collider in colliders)
        {
            var carryable = collider.gameObject.GetComponent<Carryable>();
            if (carryable != null && !IsCarryingObject())
            {
                Carry(carryable);
                break;
            }

            var bowl = collider.gameObject.GetComponent<Bowl>();
            if (bowl != null && IsCarryingObject() && !bowl.IsFull())
            {
                FillBowl(bowl);
                break;
            }
        }
    }

    void FixedUpdate()
    {
        m_controller.Move(m_horizontal, false, m_jump);
        m_jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damage = collision.gameObject.GetComponent<DamageDealer>();
        if (damage != null)
        {
            Die();
        }
    }

    private bool IsCarryingObject()
    {
        return m_carriedObject != null;
    }

    private void Carry(Carryable carryable)
    {
        m_carriedObject = carryable;
        m_carriedObject.transform.parent = m_carrySlot;
        m_carriedObject.transform.localPosition = Vector3.zero;
    }

    private void FillBowl(Bowl recepticle)
    {
        Debug.Assert(IsCarryingObject(), "Cannot drop anything if not carrying");
        recepticle.Put(m_carriedObject);
        m_carriedObject = null;
    }

    private void Die()
    {
        if (IsCarryingObject())
        {
            DropObject();
        }

        GameObject.Destroy(gameObject);
    }

    private void DropObject()
    {
        Debug.Assert(IsCarryingObject(), "Cannot drop anything if not carrying");
        m_carriedObject.transform.parent = transform.parent;
        m_carriedObject = null;
    }
}
