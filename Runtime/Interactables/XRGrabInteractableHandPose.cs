using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using XR.Interaction.Toolkit.Hand.Utilities.Animations;

namespace XR.Interaction.Toolkit.Hand.Utilities.Interactables
{
    public class XRGrabInteractableHandPose : XRGrabInteractable
    {
        [SerializeField]
        private PoseHierarchyScriptableObject leftHand = null;
        [SerializeField]
        private PoseHierarchyScriptableObject rightHand = null;


        public PoseHierarchyScriptableObject LeftHand { get => leftHand; set => leftHand = value; }
        public PoseHierarchyScriptableObject RightHand { get => rightHand; set => rightHand = value; }


        private readonly Dictionary<NearFarInteractor, IDisposable> interactorToSub = new Dictionary<NearFarInteractor, IDisposable>();


        protected override void OnSelectEntering(SelectEnterEventArgs args)
        {
            if (leftHand == null || rightHand == null)
            {
                base.OnSelectEntering(args);
                return;
            }
            useDynamicAttach = false;
            if (!attachTransform)
            {
                attachTransform = CreateChildTransform("Attach Transform");
            }
            if (!secondaryAttachTransform)
            {
                secondaryAttachTransform = CreateChildTransform("Secondary Attach Transform");
            }
            var baseInteractor = args.interactorObject as NearFarInteractor;
            var interactorsAttachTransform = GetAttachTransform(baseInteractor);
            var hand = baseInteractor.handedness == InteractorHandedness.Right ? rightHand : leftHand;
            var poseHierarchy = new PoseHierarchy(hand.poseHierarchy);
            interactorsAttachTransform.SetLocalPose(poseHierarchy.Poses[0]);
            poseHierarchy.Poses[0] = Pose.identity;
            base.OnSelectEntering(args);
            var receiver = baseInteractor.GetComponentInParent<TrackedPoseDriver>().GetComponentInChildren<PoseHierarchyReceiver>();
            if (interactorToSub.Remove(baseInteractor, out var sub))
            {
                sub.Dispose();
            }
            interactorToSub[baseInteractor] = receiver.Apply(poseHierarchy, 15.0f, true);
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
            if (leftHand == null || rightHand == null)
            {
                base.OnSelectExiting(args);
                return;
            }
            base.OnSelectExiting(args);
            var baseInteractor = args.interactorObject as NearFarInteractor;
            if (interactorToSub.Remove(baseInteractor, out var sub))
            {
                sub.Dispose();
            }
        }


        private Transform CreateChildTransform(string name)
        {
            var childTransform = new GameObject(name).transform;
            childTransform.SetParent(transform);
            childTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            return childTransform;
        }
    }
}