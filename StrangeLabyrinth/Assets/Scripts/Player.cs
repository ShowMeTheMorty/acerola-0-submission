using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public class InputMod
    {
        public InputAction Jump;
        public InputAction Sprint;
        public InputAction MoveForward;
        public InputAction MoveLateral;
    }

    public float LookSensitivity;
    public float Acceleration;
    public float Decceleration;
    public float WalkSpeed;
    public float RunSpeed;
    public float JumpForce;
    public float KneeForce;
    public float KneeSink;
    public float KneeDamp;
    public int CoyoteFrames;
    public float AerialControl;
    public float MaxSlope;
    public float FeetLength;

    private bool IsGrounded;
    private bool WasGrounded;
    private int CoyoteFrame;

    private Camera PlayerCam;
    private float CamTilt;

    private Rigidbody RB;
    private Vector3 GroundNormal;
    private Vector3 GroundContact;
    private Vector3 feetSpeed;

    private InputMod Control;
    private Vector2 MouseDelta;

    public Transform testOb;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        PlayerCam = GetComponentInChildren<Camera>();
        GroundNormal = Vector3.up;
        GroundContact = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
        CoyoteFrame = CoyoteFrames;
        PlayerInput _inputs = GetComponent<PlayerInput>();
        InputActionMap actionMap = _inputs.actions.FindActionMap("Player");
        Control = new InputMod()
        {
            Jump = actionMap.FindAction("Jump"),
            Sprint = actionMap.FindAction("Sprint"),
            MoveForward = actionMap.FindAction("MoveForward"),
            MoveLateral = actionMap.FindAction("MoveLateral")
        };
    }

    void Update ()
    {
        MouseInput();
        PlayerMovementSpontaneous();
    }

    void FixedUpdate ()
    {
        CastFeet();
        SuspendPlayer();
        PlayerMovement();
    }

    /// <summary>
    /// Make Player taller than collider to allow for steps
    /// </summary>
    private void SuspendPlayer()
    {
        if (!IsGrounded) return;
        Vector3 feetMaxPoint = transform.position - transform.up * FeetLength;
        float discrepancy = Vector3.Dot(transform.up, GroundContact - feetMaxPoint) - KneeSink;
        RB.velocity += GroundNormal * Mathf.Max(Vector3.Dot(RB.velocity, -GroundNormal), 0f) * KneeDamp;
        transform.Translate(transform.up * Mathf.Max(0f, discrepancy * KneeForce), Space.World);
    }

    private void CastFeet()
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, FeetLength, ~1 >> 12);
        WasGrounded = IsGrounded;
        if (!IsGrounded && CoyoteFrame > 0) CoyoteFrame--;
        if (hit.collider != null)
        {
            GroundNormal = hit.normal;
            GroundContact = hit.point;
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
            GroundNormal = transform.up;
        }
        if (IsGrounded && !WasGrounded) CoyoteFrame = CoyoteFrames;
    }

    private void MouseInput()
    {
        Vector2 _delta = Mouse.current.delta.ReadValue() * LookSensitivity;
        Vector2 delta = _delta + MouseDelta;
        delta /= 2f;

        CamTilt -= delta.y;
        CamTilt = Mathf.Clamp(CamTilt, -90f, 90f);

        transform.Rotate(new Vector3(0f, delta.x, 0f), Space.Self);
        PlayerCam.transform.localRotation = Quaternion.Euler(CamTilt, 0f, 0f);

        MouseDelta = _delta;
    }

    private void PlayerMovementSpontaneous()
    {
        
        if (Control.Jump.triggered && (IsGrounded || CoyoteFrame > 0))
        {
            float currentUpSpeed = Vector3.Dot(transform.up, RB.velocity);
            RB.velocity += transform.up * Mathf.Max(0f, JumpForce - currentUpSpeed);
            CoyoteFrame = 0;
        }
    }

    private void PlayerMovement()
    {
        float forwardInput = Control.MoveForward.ReadValue<float>();
        float lateralInput = Control.MoveLateral.ReadValue<float>();

        float inputScale = Mathf.Sqrt(forwardInput * forwardInput + lateralInput * lateralInput);
        if (inputScale > 1f) 
        {
            forwardInput /= inputScale;
            lateralInput /= inputScale;
        }

        Vector3 floorForward = -Vector3.Normalize(Vector3.Cross(GroundNormal, transform.right));
        Vector3 inputVector = new Vector3(lateralInput, 0f, forwardInput);

        Quaternion feetOrientation = Quaternion.LookRotation(floorForward, GroundNormal);
        testOb.rotation = feetOrientation;
        testOb.position = GroundContact;

        Vector3 feetSpaceInputVector = feetOrientation * inputVector;
        
        Vector3 feetForce = feetSpaceInputVector * Acceleration;
        
        float capSpeed = Control.Sprint.IsPressed() ? RunSpeed : WalkSpeed;
        feetSpeed = Vector3.ProjectOnPlane(RB.velocity, GroundNormal);
        Vector3 deccelerateForce = Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(-feetSpeed * Decceleration, Vector3.Normalize(feetForce)), GroundNormal);

        if (!IsGrounded) 
        {
            capSpeed *= AerialControl;
            feetForce *= AerialControl;
            deccelerateForce *= 0.09f;
        }

        if ((feetForce + feetSpeed).magnitude <= capSpeed) RB.velocity += feetForce;
        RB.velocity += deccelerateForce;
        // static friction
        if (IsGrounded && feetForce.magnitude < 0.01f && feetSpeed.magnitude < 0.3f) 
        {
            RB.velocity = Vector3.zero + transform.up * Vector3.Dot(RB.velocity, transform.up);
        }
        
        feetSpeed = Vector3.ProjectOnPlane(RB.velocity, GroundNormal);
    }
}
