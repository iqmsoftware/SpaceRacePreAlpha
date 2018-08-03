using Assets.Scripts.EntityComponents;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public struct Filter
    {
        public Transform Transform;
        public Rigidbody Rigidbody;
        public MovementComponent MovementComponent;
    }

    public class MovementSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            foreach (var entity in GetEntities<Filter>())
            {
                // Are we tranforming the position.
                var rb = entity.Rigidbody;
                var verticalMovementIndex = entity.MovementComponent.Vertical;
                var horizontalMovementIndex = entity.MovementComponent.Horizontal;

                if (rb != null)
                {
                    var currentSpeed = GetCurrentSpeed(entity);

                    // Remove all non gavitaional velocity.
                    var currentVel = rb.velocity;
                    currentVel = new Vector3(0, currentVel.y, 0);
                    rb.velocity = currentVel;

                    if (verticalMovementIndex != 0f || horizontalMovementIndex != 0f)
                    {
                        // Adjust the velocity of the transform/rigid body if there is one.
                        var movementVector = new Vector3(verticalMovementIndex, 0, horizontalMovementIndex);
                        if (entity.MovementComponent.RelativeMovement)
                        {
                            Debug.LogWarning("Add Relative Force");
                            rb.AddRelativeForce(movementVector * currentSpeed, entity.MovementComponent.CurrentForceMode);
                        }
                        else
                        {
                            Debug.LogWarning("Add Force");
                            rb.AddForce(movementVector * currentSpeed, entity.MovementComponent.CurrentForceMode);
                        }
                    }
                    else
                    {
                        // If there are current forces but no input detected.
                        if (rb.velocity != Vector3.zero && rb.angularVelocity != Vector3.zero)
                        {
                            // Stop all force.
                            Debug.LogWarning("Stop All Force");
                            rb.velocity = Vector3.zero;
                            rb.angularVelocity = Vector3.zero;
                        }
                    }
                }
            }
        }

        protected virtual float GetCurrentSpeed(Filter entity)
        {
            float returnSpeed;

            // Get the local direction of movement.
            var directionOfMovement = entity.Transform.InverseTransformDirection(entity.Rigidbody.velocity);
            var localForwardVelocity = Vector3.Dot(entity.Rigidbody.velocity, entity.Transform.forward);

            if (localForwardVelocity < 1)
            {
                returnSpeed = entity.MovementComponent.MaxSpeed;
            }
            else
            {
                // Compare the direction of movement and the facing direction to dermine if we are allowed to run.
                if (directionOfMovement.normalized.z > 0.8f)
                {
                    // Allow runnung (as a percentage of your current stamina
                    returnSpeed = entity.MovementComponent.IsRunning
                        ? entity.MovementComponent.MaxSpeed + entity.MovementComponent.MaxRunningSpeed
                        : entity.MovementComponent.MaxSpeed;
                }
                else
                {
                    // Else return max running speed.
                    returnSpeed = entity.MovementComponent.MaxSpeed;
                }
            }

            return returnSpeed;
        }
    }
}