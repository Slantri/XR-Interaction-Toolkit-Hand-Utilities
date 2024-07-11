using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace XR.Interaction.Toolkit.Hand.Utilities.PhysicsUtils
{
    public class PhysicsTrackedPoseDriver : MonoBehaviour
    {
        [Flags]
        public enum TrackingTypes
        {
            None = 0x0,
            Rotation = 0x1,
            Position = 0x2,
        }


        [SerializeField]
        private TrackingTypes trackingType = TrackingTypes.Position | TrackingTypes.Rotation;
        [SerializeField]
        private InputActionProperty positionInput;
        [SerializeField]
        private InputActionProperty rotationInput;
        [SerializeField]
        private bool ignoreTrackingState = false;
        [SerializeField]
        private InputActionProperty trackingStateInput;
        [SerializeField]
        private Rigidbody rigidbody = null;
        [SerializeField, Range(0f, 1f)]
        private float velocityDamping = 1.0f;
        [SerializeField]
        private float velocityScale = 1.0f;
        [SerializeField, Range(0f, 1f)]
        private float angVelocityDamping = 1.0f;
        [SerializeField]
        private float angVelocityScale = 1.0f;


        private Pose targetPose = Pose.identity;
        private TrackingTypes inputTrackingType = TrackingTypes.Position | TrackingTypes.Rotation;


        public TrackingTypes TrackingType { get => trackingType; set => trackingType = value; }
        public InputActionProperty PositionInput { get => positionInput; set => positionInput = value; }
        public InputActionProperty RotationInput { get => rotationInput; set => rotationInput = value; }
        public bool IgnoreTrackingState { get => ignoreTrackingState; set => ignoreTrackingState = value; }
        public InputActionProperty TrackingStateInput { get => trackingStateInput; set => trackingStateInput = value; }
        public Rigidbody Rigidbody { get => rigidbody; set => rigidbody = value; }
        public float VelocityDamping { get => velocityDamping; set => velocityDamping = value; }
        public float VelocityScale { get => velocityScale; set => velocityScale = value; }
        public float AngVelocityDamping { get => angVelocityDamping; set => angVelocityDamping = value; }
        public float AngVelocityScale { get => angVelocityScale; set => angVelocityScale = value; }


        protected void OnEnable()
        {
            var trackingStateAction = trackingStateInput.action;
            if (trackingStateInput.reference == null)
            {
                trackingStateAction.Enable();
            }
            trackingStateAction.started += OnTrackingStateUpdated;
            trackingStateAction.performed += OnTrackingStateUpdated;
            trackingStateAction.canceled += OnTrackingStateStopped;


            var positionInputAction = positionInput.action;
            if (positionInput.reference == null)
            {
                positionInputAction.Enable();
            }
            positionInputAction.started += OnPositionSet;
            positionInputAction.performed += OnPositionSet;
            positionInputAction.canceled += OnPositionStopped;


            var rotationInputAction = rotationInput.action;
            if (rotationInput.reference == null)
            {
                trackingStateAction.Enable();
            }
            rotationInputAction.started += OnRotationSet;
            rotationInputAction.performed += OnRotationSet;
            rotationInputAction.canceled += OnRotationStopped;

        }

        private void OnRotationStopped(InputAction.CallbackContext context)
        {
            targetPose.rotation = Quaternion.identity;
        }

        private void OnRotationSet(InputAction.CallbackContext context)
        {
            targetPose.rotation = context.ReadValue<Quaternion>();
        }

        private void OnPositionStopped(InputAction.CallbackContext context)
        {
            targetPose.position = Vector3.zero;
        }

        private void OnPositionSet(InputAction.CallbackContext context)
        {
            targetPose.position = context.ReadValue<Vector3>();
        }

        private void OnTrackingStateStopped(InputAction.CallbackContext context)
        {
            inputTrackingType = TrackingTypes.None;
        }

        private void OnTrackingStateUpdated(InputAction.CallbackContext context)
        {
            inputTrackingType = (TrackingTypes)context.ReadValue<int>();
        }

        protected void FixedUpdate()
        {
            var worldPose = targetPose;
            if (transform.parent)
            {
                worldPose = targetPose.GetTransformedBy(new Pose(transform.parent.position, transform.parent.rotation));
            }
            var deltaTime = Time.deltaTime;
            var positionValid = ignoreTrackingState || (inputTrackingType & TrackingTypes.Position) != 0;
            var rotationValid = ignoreTrackingState || (inputTrackingType & TrackingTypes.Rotation) != 0;


            if (positionValid && (trackingType & TrackingTypes.Position) != 0)
            {
                if (rigidbody.isKinematic)
                {
                    rigidbody.MovePosition(targetPose.position);
                }
                else
                {
#if UNITY_2023_3_OR_NEWER
                    m_Rigidbody.linearVelocity *= (1f - velocityDamping);
#else
                    rigidbody.velocity *= (1f - velocityDamping);
#endif
                    var positionDelta = worldPose.position - transform.position;
                    var velocity = positionDelta / deltaTime;
#if UNITY_2023_3_OR_NEWER
                    m_Rigidbody.linearVelocity += (velocity * m_VelocityScale);
#else
                    rigidbody.velocity += (velocity * velocityScale);
#endif
                }
            }
            if (rotationValid && (trackingType & TrackingTypes.Position) != 0)
            {
                if (rigidbody.isKinematic)
                {
                    rigidbody.MoveRotation(targetPose.rotation);
                }
                else
                {
                    rigidbody.angularVelocity *= (1f - angVelocityDamping);
                    var rotationDelta = worldPose.rotation * Quaternion.Inverse(transform.rotation);
                    rotationDelta.ToAngleAxis(out var angleInDegrees, out var rotationAxis);
                    if (angleInDegrees > 180f)
                        angleInDegrees -= 360f;

                    if (Mathf.Abs(angleInDegrees) > Mathf.Epsilon)
                    {
                        var angularVelocity = (rotationAxis * (angleInDegrees * Mathf.Deg2Rad)) / deltaTime;
                        rigidbody.angularVelocity += (angularVelocity * angVelocityScale);
                    }
                }
            }

        }
    }
}