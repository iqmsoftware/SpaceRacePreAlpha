using Assets.Scripts.EntityComponents;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class InputSystem : ComponentSystem
    {
        public struct Filter
        {
            public Transform Transform;
            public MovementComponent MovementComponent;
            public MouseRotationComponent RotationComponent;
        }

        protected override void OnUpdate()
        {
            foreach (var entity in GetEntities<Filter>())
            {
                var captureInputs = true;
                if (captureInputs)
                {
                    SetKeyboardInputs(entity);
                    SetMouseInputs(entity);
                }
            }
        }

        protected void SetKeyboardInputs(Filter entity)
        {
            // Get the vertical and horizontal axis index.
            entity.MovementComponent.Horizontal = Input.GetAxis("Vertical");
            entity.MovementComponent.Vertical = Input.GetAxis("Horizontal");
            entity.MovementComponent.IsRunning = Input.GetKey(KeyCode.LeftShift);
            entity.MovementComponent.IsDucking = Input.GetKey(KeyCode.LeftControl) && !entity.MovementComponent.IsRunning;
        }

        protected void SetMouseInputs(Filter entity)
        {
            Vector3 mousePosition;
            Vector3 gameObjectWorldPos;

            // Get the mouse position
            mousePosition = Input.mousePosition;

            // Get the current object position
            gameObjectWorldPos = Camera.main.WorldToScreenPoint(entity.Transform.position);

            // calculate the mouse pos in relation to object positon.
            mousePosition.x = mousePosition.x - gameObjectWorldPos.x;
            mousePosition.y = mousePosition.y - gameObjectWorldPos.y;

            // Calculate the amount of rotation required to make the object face toward the mouse pointer.
            // compensate by -90 deg so that the x axis points toward the target.
            entity.RotationComponent.Angle = -(Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg -90);
        }
    }
}