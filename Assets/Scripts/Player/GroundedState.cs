using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class Movement : MonoBehaviour {
    private class GroundedState : MovementState {
        public GroundedState(Movement movement) : base(movement) {

        }

        public override void OnStateEnter() {
            print("Entered grounded state");

            _movement.velocity.y = 0;
        }

        public override void OnStateUpdate() {
            if (Input.GetButtonDown("Jump")) {
                _movement.velocity += Vector3.up * Mathf.Sqrt(_movement.jumpHeight * 2f * Physics.gravity.magnitude * (_movement.fallSpeedMultiplier));
                _movement.SetState(new AirState(_movement));
                return;
            }

            var rayCast = Physics.Raycast(_movement.transform.position, Vector3.down, out RaycastHit hit, _movement.characterController.height / 2 + _movement.characterController.skinWidth + 1f);

            if (!_movement.grounded) _movement.SetState(new AirState(_movement));

            _movement.characterController.Move(Vector3.down * hit.distance);

            var vel = _movement.velocity;

            if (vel.magnitude != 0) {
                var drop = vel.magnitude * _movement.friction * Time.deltaTime;
                _movement.velocity *= Mathf.Max(vel.magnitude - drop, 0) / vel.magnitude; // Scale the velocity based on friction.
            }

            var movementDir = _movement.transform.TransformDirection(_movement.desiredMovement);
            movementDir.y += Vector3.Dot(movementDir, -hit.normal);

            _movement.DoAcceleration(movementDir, _movement.acceleration, _movement.maxVelocity);
        }

        public override void OnStateExit() {
            print("Exited grounded state");
        }
    }
}