using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public class InputMod
    {
        public InputAction jump;
        public InputAction sprint;
        public InputAction moveForward;
        public InputAction moveLateral;
    }

    public float lookSensitivity;
    public float acceleration;
    public float decceleration;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float kneeForce;
    public float kneeSink;
    public float kneeDamp;
    public int coyoteFrames;
    public float aerialControl;
    public float maxSlope;
    public float feetLength;

    private bool isGrounded;
    private bool wasGrounded;
    private int coyoteFrame;

    private Camera playerCam;
    private float camTilt;

    private Rigidbody RB;
    private Vector3 groundNormal;
    private Vector3 groundContact;
    private Vector3 feetSpeed;

    private InputMod control;
    private Vector2 mouseDelta;

    private float portalRelPos;
    private Portal closestPortal;

    public Transform testOb;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<Camera>();
        groundNormal = Vector3.up;
        groundContact = Vector3.zero;
        Cursor.lockState = CursorLockMode.Locked;
        coyoteFrame = coyoteFrames;
        PlayerInput _inputs = GetComponent<PlayerInput>();
        InputActionMap actionMap = _inputs.actions.FindActionMap("Player");
        control = new InputMod()
        {
            jump = actionMap.FindAction("Jump"),
            sprint = actionMap.FindAction("Sprint"),
            moveForward = actionMap.FindAction("MoveForward"),
            moveLateral = actionMap.FindAction("MoveLateral")
        };
    }

    void Update ()
    {
        MouseInput();
        PlayerMovementSpontaneous();
        PortalTraversal();
    }

    void LateUpdate ()
    {

    }

    void FixedUpdate ()
    {
        CastFeet();
        SuspendPlayer();
        PlayerMovement();
    }

    private void PortalTraversal()
    {
        float bestDist = float.MaxValue;
        Portal best = null;
        foreach (Portal portal in MainCamera.portals.Keys)
        {
            float dist = (portal.transform.position - transform.position).magnitude;
            if (dist < bestDist) 
            {
                bestDist = dist;
                best = portal;
            }
        }
        if (best != null)
        {
            Vector3 toPortal = best.transform.position - playerCam.transform.position;
            float dotWithPortal = Vector3.Dot(toPortal, best.transform.forward);
            
            // will we go?
            if (best == closestPortal && Mathf.Sign(dotWithPortal) != Mathf.Sign(portalRelPos) && WithinPortalBounds(best, toPortal))
            {
                Teleport(best);
            }
            closestPortal = best;
            portalRelPos = dotWithPortal;
        }
    }

    private bool WithinPortalBounds(Portal portal, Vector3 toPortalWorld)
    {
        Vector3 toPortalLocal = portal.transform.InverseTransformVector(toPortalWorld);
        bool withinWidth = Mathf.Abs(toPortalLocal.x) < portal.transform.localScale.x / 2f;
        bool withinHeight = Mathf.Abs(toPortalLocal.y) < portal.transform.localScale.y / 2f;
        return withinWidth && withinHeight;
    }

    private void Teleport(Portal portal)
    {
        Matrix4x4 transMatrix = portal.linkedPortal.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix * transform.localToWorldMatrix;
        transform.SetPositionAndRotation(transMatrix.GetColumn(3), transMatrix.rotation);
        transMatrix = portal.linkedPortal.transform.localToWorldMatrix * portal.transform.worldToLocalMatrix;
        RB.velocity = transMatrix * RB.velocity;
        Physics.gravity = -transform.up * Physics.gravity.magnitude;
        // UnityEditor.EditorApplication.isPaused = true;
    }

    /// <summary>
    /// Make Player taller than collider to allow for steps
    /// </summary>
    private void SuspendPlayer()
    {
        if (!isGrounded) return;
        Vector3 feetMaxPoint = transform.position - transform.up * feetLength;
        float discrepancy = Vector3.Dot(transform.up, groundContact - feetMaxPoint) - kneeSink;
        RB.velocity += groundNormal * Mathf.Max(Vector3.Dot(RB.velocity, -groundNormal), 0f) * kneeDamp;
        transform.Translate(transform.up * Mathf.Max(0f, discrepancy * kneeForce), Space.World);
    }

    private void CastFeet()
    {
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, feetLength, ~1 >> 12);
        wasGrounded = isGrounded;
        if (!isGrounded && coyoteFrame > 0) coyoteFrame--;
        if (hit.collider != null)
        {
            groundNormal = hit.normal;
            groundContact = hit.point;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            groundNormal = transform.up;
        }
        if (isGrounded && !wasGrounded) coyoteFrame = coyoteFrames;
    }

    private void MouseInput()
    {
        Vector2 _delta = Mouse.current.delta.ReadValue() * lookSensitivity;
        Vector2 delta = _delta + mouseDelta;
        delta /= 2f;

        camTilt -= delta.y;
        camTilt = Mathf.Clamp(camTilt, -90f, 90f);

        transform.Rotate(new Vector3(0f, delta.x, 0f), Space.Self);
        playerCam.transform.localRotation = Quaternion.Euler(camTilt, 0f, 0f);

        mouseDelta = _delta;
    }

    private void PlayerMovementSpontaneous()
    {
        
        if (control.jump.triggered && (isGrounded || coyoteFrame > 0))
        {
            float currentUpSpeed = Vector3.Dot(transform.up, RB.velocity);
            RB.velocity += transform.up * Mathf.Max(0f, jumpForce - currentUpSpeed);
            coyoteFrame = 0;
        }
    }

    private void PlayerMovement()
    {
        float forwardInput = control.moveForward.ReadValue<float>();
        float lateralInput = control.moveLateral.ReadValue<float>();

        float inputScale = Mathf.Sqrt(forwardInput * forwardInput + lateralInput * lateralInput);
        if (inputScale > 1f) 
        {
            forwardInput /= inputScale;
            lateralInput /= inputScale;
        }

        Vector3 floorForward = -Vector3.Normalize(Vector3.Cross(groundNormal, transform.right));
        Vector3 inputVector = new Vector3(lateralInput, 0f, forwardInput);

        Quaternion feetOrientation = Quaternion.LookRotation(floorForward, groundNormal);
        testOb.rotation = feetOrientation;
        testOb.position = groundContact;

        Vector3 feetSpaceInputVector = feetOrientation * inputVector;
        
        float capSpeed = control.sprint.IsPressed() ? runSpeed : walkSpeed;
        feetSpeed = Vector3.ProjectOnPlane(RB.velocity, groundNormal);

        if (!isGrounded) 
        {
            capSpeed *= aerialControl;
        }

        Vector3 feetForce = feetSpaceInputVector * acceleration;
        if ((feetForce + feetSpeed).magnitude > capSpeed) feetForce *= 0f;

        Vector3 deccelerateForce = Vector3.ProjectOnPlane(-feetSpeed * decceleration, groundNormal);
        if (!isGrounded) 
        {
            feetForce *= aerialControl;
            deccelerateForce *= 0.09f;
        }
        deccelerateForce -= Vector3.Normalize(deccelerateForce) * Mathf.Min(deccelerateForce.magnitude, Mathf.Max(0f, Vector3.Dot(-deccelerateForce, feetForce)));

        RB.velocity += feetForce;
        RB.velocity += deccelerateForce;

        // static friction
        if (isGrounded && feetForce.magnitude < 0.01f && feetSpeed.magnitude < 0.3f) 
        {
            RB.velocity = Vector3.zero + transform.up * Vector3.Dot(RB.velocity, transform.up);
        }
        
        feetSpeed = Vector3.ProjectOnPlane(RB.velocity, groundNormal);
    }
}
