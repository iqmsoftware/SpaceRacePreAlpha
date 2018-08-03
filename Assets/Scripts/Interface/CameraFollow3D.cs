using Assets.Scripts;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public class CameraFollow3D : MonoBehaviour
    {
        public Transform myTarget;
        public Transform camTranform;
        public float SensivityX = 1.5f;
        public float SensivityY = 1.5f;
        public bool ClampCamY = true;
        public float Y_ANGLE_MIN = 45f;
        public float Y_ANGLE_MAX = 85f;
        public float ZOOM_MIN = 3f;
        public float ZOOM_MAX = 19f;
        public bool LinkedToTargetRot;
        public Vector3 CameraXYOffset = new Vector3(0, 0, 10f);
        public bool FixedCamera = true;

        private Camera cam;
        private float CurrentX = 0f;
        private float CurrentY = 0f;

        public void Start()
        {
            LinkedToTargetRot = false;
            camTranform = transform;
            cam = Camera.main;
        }

        public void Update()
        {
            //// LinkedToTargetRot = !Input.GetKey(KeyCode.LeftAlt);
            if (!FixedCamera)
            {
                CurrentX += Input.GetAxis("Mouse X")*SensivityX;
                CurrentY += Input.GetAxis("Mouse Y")*SensivityY;
            }

            if (ClampCamY)
            {
                CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
            }

            // Apply Mouse zoom.
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                // Adjust the orthographic size of the camera.
                var tempDist = CameraXYOffset.z - Input.GetAxis("Mouse ScrollWheel");

                // Make sure the camera can't zoom in or out too far.
                CameraXYOffset.z = Mathf.Clamp(tempDist, ZOOM_MIN, ZOOM_MAX);
            }
        }

        public void LateUpdate()
        {
            if (myTarget != null)
            {
                var direction = new Vector3(CameraXYOffset.x, CameraXYOffset.y, -CameraXYOffset.z);
                Quaternion rotation;
                if (LinkedToTargetRot)
                {
                    rotation = myTarget.rotation*Quaternion.Euler(CurrentY, 0, 0);
                }
                else
                {
                    rotation = Quaternion.Euler(CurrentY, CurrentX, 0);
                }

                camTranform.position = myTarget.position + rotation*direction;
                camTranform.LookAt(myTarget);
            }
        }
    }
}