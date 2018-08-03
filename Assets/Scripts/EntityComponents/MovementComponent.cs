using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class MovementComponent : MonoBehaviour
    {
        public float Horizontal;
        public float Vertical;
        public float MaxSpeed = 3f;
        public float MaxRunningSpeed;
        public float CurrentSpeed;

        public bool DisableMovement;
        public bool RelativeMovement;

        public ForceMode CurrentForceMode;

        public bool IsMoving => Horizontal != 0f || Vertical != 0f && !DisableMovement;
        public bool IsDucking = false;
        public bool IsRunning = false;
    }
}