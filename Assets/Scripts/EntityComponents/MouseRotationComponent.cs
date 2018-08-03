using UnityEngine;

namespace Assets.Scripts.EntityComponents
{
    public class MouseRotationComponent : MonoBehaviour
    {
        public float Angle;
        public bool DisableRotation;
        public float MouseHorizontal;
        public float RotationSpeed = 0.3f;
        public bool FollowMouse = true;
    }
}