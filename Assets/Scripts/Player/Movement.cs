using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]

public partial class Movement : MonoBehaviour {

    public float maxVelocity = 12;
    public float acceleration = 100;
    public float friction = 8;

    public float fallMaxSpeedUp = 10f;
    public float airDrag = 0.1f;
    public float jumpHeight = 2f;
    public float fallSpeedMultiplier = 2f;

    public Vector3 velocity;

    public MovementState movementState;
    public Rigidbody rigidbody;
    public bool grounded = false;
    public Vector3 groundedNormal = Vector3.up;

    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;
    private Vector3 desiredMovement;

    void Awake() {
        characterController = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        capsuleCollider.radius += characterController.skinWidth;

        SetState(new GroundedState(this));
    }

    void Update () {
        if (movementState == null) SetState(new GroundedState(this));

        CheckGround();

        desiredMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        movementState.OnStateUpdate();

        characterController.Move(velocity * Time.deltaTime);

        // Prevent the input velocity from getting bigger than what the real velocity is.
        // This prevents the player from shooting off in a certain direction when losing contact.
        if (velocity.magnitude > characterController.velocity.magnitude) velocity = characterController.velocity;
    }

    public void CheckGround () {
        grounded = rigidbody.SweepTest(Vector3.down, out RaycastHit hit, 0.25f);

        if (grounded) {
            groundedNormal = hit.normal;
        }
    }

    public void SetState (MovementState state) {
        movementState?.OnStateExit();

        movementState = state;

        movementState.OnStateEnter();
    }

    public void DoAcceleration(Vector3 wishDirection, float maxAccel, float maxVel) {
        var velocityDelta = GetAcceleration(wishDirection, maxAccel, maxVel);

        velocity += velocityDelta;
    }

    private Vector3 GetAcceleration(Vector3 wishDirection, float maxAccel, float maxVel) {
        var dotVelocity = Vector3.Dot(velocity, wishDirection);
        var addSpeed = maxVel - dotVelocity;
        addSpeed = Mathf.Clamp(addSpeed, 0, maxAccel * Time.deltaTime);

        return wishDirection * addSpeed;
    }

    public void AddForce(Vector3 force) {
        velocity += force / rigidbody.mass;
    }

    public void Move (Vector3 move) {
        characterController.Move(move);
    }
}