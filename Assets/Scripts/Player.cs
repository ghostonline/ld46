﻿using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D m_controller;
    public Transform m_carrySlot;
    public Collider2D m_actionRadius;
    public Rigidbody2D m_rigidbody;
    public Rigidbody2D m_deadPlayerPrefab;

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

        if (Input.GetButtonDown("Fire1"))
        {
            DoAction();
        }
    }

    private void DoAction()
    {
        var colliders = new List<Collider2D>();
        m_actionRadius.GetContacts(colliders);
        Carryable newCarryable = null;
        Bowl newBowl = null;
        foreach (var collider in colliders)
        {
            var bowl = collider.gameObject.GetComponent<Bowl>();
            if (bowl != null && !bowl.IsFull())
            {
                newBowl = bowl;
            }

            var carryable = collider.gameObject.GetComponent<Carryable>();
            if (carryable != null && carryable != m_carriedObject)
            {
                newCarryable = carryable;
            }
        }

        if (IsCarryingObject())
        {
            if (newBowl != null && IsCarryingFood())
            {
                AudioPlayer.Play(Clip.FillBowl);
                FillBowl(newBowl);
            }
            else if (newCarryable != null)
            {
                AudioPlayer.Play(Clip.Pickup);
                DropObject();
                PickUp(newCarryable);
            }
            else
            {
                AudioPlayer.Play(Clip.Drop);
                DropObject();
            }
        }
        else if (newCarryable != null)
        {
            AudioPlayer.Play(Clip.Pickup);
            PickUp(newCarryable);
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
            damage.TriggerDamage();
            Die();
        }
    }

    private bool IsCarryingObject()
    {
        return m_carriedObject != null;
    }

    private bool IsCarryingFood()
    {
        return m_carriedObject.isFood;
    }

    private void PickUp(Carryable carryable)
    {
        m_carriedObject = carryable;
        m_carriedObject.StartCarry(m_carrySlot);
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

        var deadProp = GameObject.Instantiate(m_deadPlayerPrefab, transform.position, transform.rotation);
        var currentVelocity = m_rigidbody.velocity;
        currentVelocity.y = 10.0f;
        deadProp.velocity = currentVelocity;

        AudioPlayer.Play(Clip.Die);

        GameObject.Destroy(gameObject);
        GameController.Instance.OnDied();
    }

    private void DropObject()
    {
        Debug.Assert(IsCarryingObject(), "Cannot drop anything if not carrying");
        m_carriedObject.StopCarry();
        m_carriedObject = null;
    }
}
