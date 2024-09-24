using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class TrackingTypeEvents : MonoBehaviour
    {
        [Flags]
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


        private TrackingTypes haveTrackingDataFor = TrackingTypes.None;
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

            haveTrackingDataFor = TrackingTypes.None;
            SetHaveTrackingDataBit(isTrackedControllerInputAction.ReadValue<float>() > 0.65f, TrackingTypes.Controller);
            SetHaveTrackingDataBit(isTrackedHandInputAction.ReadValue<float>() > 0.65f, TrackingTypes.Hand);
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
            SetHaveTrackingDataBit(context.ReadValueAsButton(), TrackingTypes.Hand);
        }

        private void OnIsTrackedHandCanceled(InputAction.CallbackContext context)
        {
            SetHaveTrackingDataBit(false, TrackingTypes.Hand);
        }

        private void OnIsTrackedHandStarted(InputAction.CallbackContext context)
        {
            SetHaveTrackingDataBit(context.ReadValueAsButton(), TrackingTypes.Hand);
        }

        private void OnIsTrackedControllerPerformed(InputAction.CallbackContext context)
        {
            SetHaveTrackingDataBit(context.ReadValueAsButton(), TrackingTypes.Controller);
        }

        private void OnIsTrackedControllerCanceled(InputAction.CallbackContext context)
        {
            SetHaveTrackingDataBit(false, TrackingTypes.Controller);
        }

        private void OnIsTrackedControllerStarted(InputAction.CallbackContext context)
        {
            SetHaveTrackingDataBit(context.ReadValueAsButton(), TrackingTypes.Controller);
        }


        private void SetHaveTrackingDataBit(bool setOn, TrackingTypes value)
        {
            if (setOn)
            {
                haveTrackingDataFor |= value;
            }
            else
            {
                haveTrackingDataFor &= ~value;
            }
        }

        private void LateUpdate()
        {
            var newTrackingType = curTrackingType;
            if ((newTrackingType & haveTrackingDataFor) != newTrackingType || newTrackingType == TrackingTypes.None)
            {
                if ((haveTrackingDataFor & TrackingTypes.Controller) == TrackingTypes.Controller)
                {
                    newTrackingType = TrackingTypes.Controller;
                }
                else if ((haveTrackingDataFor & TrackingTypes.Hand) == TrackingTypes.Hand)
                {
                    newTrackingType = TrackingTypes.Hand;
                }
                else
                {
                    newTrackingType = TrackingTypes.None;
                }
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