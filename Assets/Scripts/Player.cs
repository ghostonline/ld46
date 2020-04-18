using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D m_controller;
    public Transform m_carrySlot;

    private float m_horizontal = 0.0f;
    private bool m_jump = false;
    private Carryable m_carriedObject = null;

    void Update()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }
    }

    void FixedUpdate()
    {
        m_controller.Move(m_horizontal, false, m_jump);
        m_jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var carryable = collision.gameObject.GetComponent<Carryable>();
        if (carryable != null && !IsCarryingObject())
        {
            Carry(carryable);
        }

        var bowl = collision.gameObject.GetComponent<Bowl>();
        if (bowl != null && IsCarryingObject())
        {
            Drop(bowl);
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

    private void Drop(Bowl recepticle)
    {
        Debug.Assert(IsCarryingObject(), "Cannot drop anything if not carrying");
        recepticle.Put(m_carriedObject);
        m_carriedObject = null;
    }
}
