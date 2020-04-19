using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChompAIState
{
    Sleeping,
    Hungry,
    Fed,
}

public class ChompAI : MonoBehaviour
{
    public CharacterController2D m_controller;
    public float m_speed = 1f;
    public float m_hopForce = 50f;
    public float m_jumpForce = 250f;
    public Transform m_anchor;
    public float m_chainLength = 3f;
    public ChompAIState m_initialState = ChompAIState.Hungry;
    public bool m_fixedAnchor = false;
    public float m_eatRange = 1f;
    public float m_eatDuration = 2f;
    public Transform m_chainLinkPrefab;

    float m_eatTimer = 0f;
    float m_movement = 0f;
    Rigidbody2D m_rigidbody;
    ChompAIState m_state;
    Rigidbody2D m_anchorRigidbody;
    Transform[] m_chainLinks;

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
        CreateChain();
    }

    void CreateChain()
    {
        m_chainLinks = new Transform[5];
        for (int ii = 0; ii < m_chainLinks.Length; ++ii)
        {
            var link = GameObject.Instantiate(m_chainLinkPrefab);
            link.name = string.Format("ChainLink_{0}", ii);
            link.parent = transform;
            m_chainLinks[ii] = link;
        }
    }

    void Update()
    {
        if (m_state == ChompAIState.Hungry)
        {
            var target = FindNearestEdible();
            if (target)
            {
                ChaseFood(target);
            }
            else
            {
                Idle();
            }
        }
        else if (m_state == ChompAIState.Fed)
        {
            var target = FindNearestPlayer();
            if (target != null)
            {
                ChaseTarget(target.transform);
            }
        }
    }

    private void LateUpdate()
    {
        UpdateChain();
    }

    void UpdateChain()
    {
        var start = transform.position;
        var end = m_anchor.position;
        var offset = end - start;
        var step = offset / (m_chainLinks.Length + 1);
        for (int ii = 0; ii < m_chainLinks.Length; ++ii)
        {
            m_chainLinks[ii].position = start + step * (ii + 1);
        }
    }

    Food FindNearestEdible()
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

        return nearest ? nearest : null;
    }

    void ChaseFood(Food target)
    {
        var targetOffset = ChaseTarget(target.transform);

        if (targetOffset.magnitude < m_eatRange && target.m_satisfying)
        {
            m_eatTimer += Time.deltaTime;

            if (m_eatTimer >= m_eatDuration)
            {
                target.Consume();
                m_state = ChompAIState.Fed;
                GameController.Instance.OnChompFed();
            }
        }
        else
        {
            m_eatTimer = 0;
        }
    }

    Vector3 ChaseTarget(Transform target)
    {
        Vector3 targetOffset = target.position - transform.position;

        m_movement = Mathf.Clamp(targetOffset.x, -m_speed, m_speed);

        if (m_state == ChompAIState.Hungry && targetOffset.y > 2f && Mathf.Abs(targetOffset.x) < 4f)
        {
            m_controller.AirControl = false;
            m_controller.JumpForce = m_jumpForce;
        }
        else
        {
            m_controller.AirControl = true;
            m_controller.JumpForce = m_hopForce;
        }

        return targetOffset;
    }

    void Idle()
    {
        m_movement = 0f;
        m_controller.JumpForce = m_hopForce;
    }

    Player FindNearestPlayer()
    {
        Player nearest = null;
        float nearestDistance = float.PositiveInfinity;
        foreach (var player in FindObjectsOfType<Player>())
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = player;
            }
        }

        return nearest ? nearest : null;
    }

    private void FixedUpdate()
    {
        if (m_state != ChompAIState.Sleeping)
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
                m_state = ChompAIState.Hungry;
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
            m_state = ChompAIState.Hungry;
        }
    }
}
