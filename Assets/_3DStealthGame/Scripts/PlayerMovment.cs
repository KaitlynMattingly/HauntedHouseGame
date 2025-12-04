using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction MoveAction;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    public float sprintSpeed = 2.5f;
    public float maxSprintTime = 2f;
    private float sprintTimer;
    private bool canSprint = true;
    public float sprintRechargeTime = 3f;
    private float rechargeTimer = 0f;
    public bool CanSprint => canSprint;

    private bool isFrozen = false;
    private float freezeTimer = 0f;
    public float minFreezeInterval = 8f;
    public float maxFreezeInterval = 12f;
    public float freezeDuration = 3f;
    public bool IsFrozen => isFrozen;
    private IEnumerator FreezePlayer()
    {

        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;
    }

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    Animator m_Animator;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();

        sprintTimer = maxSprintTime;

        freezeTimer = Random.Range(minFreezeInterval, maxFreezeInterval);
    }

    void FixedUpdate()
    {

        if (!isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                isFrozen = true;
                StartCoroutine(FreezePlayer());
                freezeTimer = Random.Range(minFreezeInterval, maxFreezeInterval);
            }
        }

        if (isFrozen)
            return;

        var pos = MoveAction.ReadValue<Vector2>();
        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        float currentSpeed = walkSpeed;
        bool sprintHeld = Keyboard.current.leftShiftKey.isPressed;
        if (sprintHeld && canSprint && m_Movement.magnitude > 0f)
        {
            currentSpeed = sprintSpeed;
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0f)
            {
                canSprint = false;
                rechargeTimer = sprintRechargeTime;
            }
        }
        else
        {
            if (!canSprint)
            {
                rechargeTimer -= Time.deltaTime;
                if (rechargeTimer <= 0f)
                {
                    canSprint = true;
                    sprintTimer = maxSprintTime;
                }
            }
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * currentSpeed * Time.deltaTime);

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
    }
}