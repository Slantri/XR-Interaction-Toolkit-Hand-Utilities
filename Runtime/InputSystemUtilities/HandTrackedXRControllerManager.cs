using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    [AddComponentMenu("XR/Hand Tracking Utilities/Hand Tracking XR Controller Manager", 11)]
    [DefaultExecutionOrder(XRInteractionUpdateOrder.k_DeviceSimulator)]
    public class HandTrackedXRControllerManager : MonoBehaviour
    {
        [Serializable]
        public enum HandTypes
        {
            LeftHand = 0x1,
            RightHand = 0x2,
        }


        [SerializeField]
        private HandTypes handType = HandTypes.LeftHand;
        [SerializeField]
        private XRBaseInteractor interactor = null;
        [SerializeField]
        private Transform root = null;
        [SerializeField]
        private float scalarGravity = 10.0f;


        private bool grip = false;
        private bool pinch = false;
        private bool closeIndexFinger = false;
        private bool palmToCenterOfBody = false;
        private bool palmUp = false;


        private HandTrackedXRControllerState inputData;
        private HandTrackedXRController hand = null;
        private bool ignoreTrigger = false;


        public bool Grip { get => grip; set => grip = value; }
        public bool Pinch { get => pinch; set => pinch = value; }
        public bool CloseIndexFinger { get => closeIndexFinger; set => closeIndexFinger = value; }
        public bool PalmToCenterOfBody { get => palmToCenterOfBody; set => palmToCenterOfBody = value; }
        public bool PalmUp { get => palmUp; set => palmUp = value; }
        public HandTypes HandType { get => handType; set => handType = value; }
        public XRBaseInteractor Interactor { get => interactor; set => interactor = value; }
        public Transform Root { get => root; set => root = value; }
        public float ScalarGravity { get => scalarGravity; set => scalarGravity = value; }

        protected void Awake()
        {
            inputData.Reset();
        }

        protected void OnEnable()
        {
            ignoreTrigger = false;
            if (hand == null)
            {
                bool isLeft = handType == HandTypes.LeftHand;
                var hand = isLeft ? UnityEngine.InputSystem.CommonUsages.LeftHand : UnityEngine.InputSystem.CommonUsages.RightHand;
                var handCharacteristics = isLeft ? XRInputTrackingAggregator.Characteristics.leftController : XRInputTrackingAggregator.Characteristics.rightController;

                var descLeftHand = new InputDeviceDescription
                {
                    product = nameof(HandTrackedXRController),
                    capabilities = new XRDeviceDescriptor
                    {
                        deviceName = $"{nameof(HandTrackedXRController)} - {hand}",
                        characteristics = handCharacteristics,
                    }.ToJson(),
                };

                this.hand = UnityEngine.InputSystem.InputSystem.AddDevice(descLeftHand) as HandTrackedXRController;
                if (this.hand != null)
                    UnityEngine.InputSystem.InputSystem.SetDeviceUsage(this.hand, hand);
            }
            else if (!hand.added)
            {
                InputSystem.AddDevice(hand);
            }
        }

        protected void OnDisable()
        {
            if (hand != null && hand.added)
            {
                InputSystem.RemoveDevice(hand);
            }
        }

        protected void Update()
        {
            if (hand == null || !hand.added)
            {
                return;
            }

            var prevInputData = inputData;
            inputData.Reset();

            var triggerOffset = -scalarGravity * Time.deltaTime;
            var gripOffset = -scalarGravity * Time.deltaTime;
            inputData.isTracked = true;
            inputData.trackingState = (int)(InputTrackingState.Position | InputTrackingState.Rotation);
            inputData.devicePosition = root.localPosition;
            inputData.deviceRotation = root.localRotation;

            if (ignoreTrigger && !pinch && !closeIndexFinger)
            {
                ignoreTrigger = false;
            }

            if (prevInputData.grip > 0.65f)
            {
                if (grip || pinch)
                {
                    gripOffset *= -1;
                }
                if (grip && !ignoreTrigger && (pinch || closeIndexFinger))
                {
                    triggerOffset *= -1;
                }
            }
            else if (prevInputData.HasButton(ControllerButton.PrimaryButton))
            {
                inputData.WithButton(ControllerButton.PrimaryButton, grip);
                triggerOffset = (!ignoreTrigger && pinch) ? triggerOffset * -1 : triggerOffset;
            }
            else if (prevInputData.HasButton(ControllerButton.SecondaryButton))
            {
                inputData.WithButton(ControllerButton.SecondaryButton, grip);
                triggerOffset = (!ignoreTrigger && pinch) ? triggerOffset * -1 : triggerOffset;
            }
            else
            {
                ignoreTrigger = true;
                bool interacting = interactor && (interactor.hasHover || interactor.hasSelection);
                if (!interacting && palmUp)
                {
                    inputData.WithButton(ControllerButton.PrimaryButton, true);
                }
                else if (!interacting && palmToCenterOfBody)
                {
                    inputData.WithButton(ControllerButton.SecondaryButton, true);
                }
                else if (grip || pinch)
                {
                    gripOffset *= -1;
                }
            }
            inputData.SetTriggerAndButton(prevInputData.trigger + triggerOffset);
            inputData.SetGripAndButton(prevInputData.grip + gripOffset);
            InputState.Change(hand, inputData);
        }
    }
}