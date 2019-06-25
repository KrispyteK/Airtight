using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class Movement : MonoBehaviour {
    private class AirState : MovementState {
        public AirState(Movement movement) : base(movement) { }

        private float enterTime;

        public override void OnStateEnter() {
            enterTime = Time.timeSinceLevelLoad;
        }

        public override void OnStateUpdate() {
            if (_movement.grounded && (Time.timeSinceLevelLoad - enterTime) > 0.1f) _movement.SetState(new GroundedState(_movement));

            _movement.ApplyFriction(_movement.airDrag);

            // Faster fall velocity.
            if (_movement.velocity.y > -_movement.fallMaxSpeedUp) _movement.velocity += Vector3.down * -Physics.gravity.y * (_movement.fallSpeedMultiplier - 1) * Time.deltaTime;

            // Gravity.
            _movement.velocity += Vector3.down * -Physics.gravity.y * Time.deltaTime;

            _movement.DoAcceleration(_movement.transform.TransformDirection(_movement.desiredMovement), _movement.airAcceleration, _movement.maxVelocity);
        }

        public override void OnStateExit() {
        }
    }
}