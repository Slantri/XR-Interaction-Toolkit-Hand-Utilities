using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class TrackingTypeEvents : MonoBehaviour
    {
        private enum TrackingTypes
        {
            None = 0,
            Controller = 1,
            Hand = 2,
        }
        [SerializeField]
        private InputActionProperty isTrackedControllerInput;
        [SerializeField]
        private InputActionProperty isTrackedHandInput;
        [SerializeField]
        private UnityEvent OnControllerTrackingSetToMain = new UnityEvent();
        [SerializeField]
        private UnityEvent OnHandTrackingSetToMain = new UnityEvent();


        private bool isControllerTracked = false;
        private bool isHandTracked = false;
        private TrackingTypes curTrackingType = TrackingTypes.None;


        protected void OnEnable()
        {
            var isTrackedControllerInputAction = isTrackedControllerInput.action;
            if (isTrackedControllerInput.reference == null)
            {
                isTrackedControllerInputAction.Enable();
            }
            isTrackedControllerInputAction.started += OnIsTrackedControllerStarted;
            isTrackedControllerInputAction.canceled += OnIsTrackedControllerCanceled;
            isTrackedControllerInputAction.performed += OnIsTrackedControllerPerformed;

            var isTrackedHandInputAction = isTrackedHandInput.action;
            if (isTrackedHandInput.reference == null)
            {
                isTrackedHandInputAction.Enable();
            }
            isTrackedHandInputAction.started += OnIsTrackedHandStarted;
            isTrackedHandInputAction.canceled += OnIsTrackedHandCanceled;
            isTrackedHandInputAction.performed += OnIsTrackedHandPerformed;

            isControllerTracked = isTrackedControllerInputAction.ReadValue<float>() > 0.65f;
            isHandTracked = isTrackedHandInputAction.ReadValue<float>() > 0.65f;
            RefreshTracking();
        }

        protected void OnDisable()
        {
            curTrackingType = TrackingTypes.None;

            var isTrackedControllerInputAction = isTrackedControllerInput.action;
            if (isTrackedControllerInput.reference == null)
            {
                isTrackedControllerInputAction.Disable();
            }
            isTrackedControllerInputAction.started -= OnIsTrackedControllerStarted;
            isTrackedControllerInputAction.canceled -= OnIsTrackedControllerCanceled;
            isTrackedControllerInputAction.performed -= OnIsTrackedControllerPerformed;

            var isTrackedHandInputAction = isTrackedHandInput.action;
            if (isTrackedHandInput.reference == null)
            {
                isTrackedHandInputAction.Disable();
            }
            isTrackedHandInputAction.started -= OnIsTrackedHandStarted;
            isTrackedHandInputAction.canceled -= OnIsTrackedHandCanceled;
            isTrackedHandInputAction.performed -= OnIsTrackedHandPerformed;
        }

        private void OnIsTrackedHandPerformed(InputAction.CallbackContext context)
        {
            isHandTracked = context.ReadValue<float>() > 0.95f;
            RefreshTracking();
        }

        private void OnIsTrackedHandCanceled(InputAction.CallbackContext context)
        {
            //isHandTracked = false;
            RefreshTracking();
        }

        private void OnIsTrackedHandStarted(InputAction.CallbackContext context)
        {
            isHandTracked = context.ReadValue<float>() > 0.95f;
            RefreshTracking();
        }

        private void OnIsTrackedControllerPerformed(InputAction.CallbackContext context)
        {
            isControllerTracked = context.ReadValue<float>() > 0.95f;
            RefreshTracking();
        }

        private void OnIsTrackedControllerCanceled(InputAction.CallbackContext context)
        {
            // isControllerTracked = false;
            RefreshTracking();
        }

        private void OnIsTrackedControllerStarted(InputAction.CallbackContext context)
        {
            isControllerTracked = context.ReadValue<float>() > 0.95f;
            RefreshTracking();
        }

        private void RefreshTracking()
        {
            var newTrackingType = curTrackingType;
            if (newTrackingType == TrackingTypes.Hand && !isHandTracked)
            {
                newTrackingType = TrackingTypes.None;
            }
            else if (newTrackingType == TrackingTypes.Controller && !isControllerTracked)
            {
                newTrackingType = TrackingTypes.None;
            }
            if (newTrackingType == TrackingTypes.None)
            {
                newTrackingType = isControllerTracked ? TrackingTypes.Controller :
                (isHandTracked ? TrackingTypes.Hand : TrackingTypes.None);
            }

            if (newTrackingType != curTrackingType && newTrackingType != TrackingTypes.None)
            {
                curTrackingType = newTrackingType;
                switch (curTrackingType)
                {
                    case TrackingTypes.Hand:
                        OnHandTrackingSetToMain?.Invoke();
                        break;
                    case TrackingTypes.Controller:
                        OnControllerTrackingSetToMain?.Invoke();
                        break;
                }
            }
        }
    }
}