using UnityEngine;

public partial class Movement : MonoBehaviour {
    public abstract class MovementState {
        protected Movement _movement;

        public MovementState (Movement movement) {
            _movement = movement;
        }

        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }
}