using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseableHandInteractablePose : MonoBehaviour
    {
        [Serializable]
        public enum ConstraintTypes
        {
            Ignore = 0x0,
            Lock = 0x1,
            TreatAsMaxOpen = 0x2,
            TreatAsMaxclose = 0x4,
        }


        [Serializable]
        public class FingerConstraint
        {
            [SerializeField]
            private PoseableHand.FingerTypes fingerType = PoseableHand.FingerTypes.None;
            [SerializeField]
            private ConstraintTypes constraintType = ConstraintTypes.Lock;
        }


        [SerializeField]
        private PoseHierarchyScriptableObject leftHand = null;
        [SerializeField]
        private PoseHierarchyScriptableObject rightHand = null;
        [SerializeField]
        private List<FingerConstraint> leftHandConstraints = new List<FingerConstraint>();
        [SerializeField]
        private List<FingerConstraint> rightHandConstraints = new List<FingerConstraint>();


        public PoseHierarchyScriptableObject LeftHand { get => leftHand; set => leftHand = value; }
        public PoseHierarchyScriptableObject RightHand { get => rightHand; set => rightHand = value; }
        public List<FingerConstraint> LeftHandConstraints { get => leftHandConstraints; set => leftHandConstraints = value; }
        public List<FingerConstraint> RightHandConstraints { get => rightHandConstraints; set => rightHandConstraints = value; }


        public void Apply(PoseHierarchy poseHierarchy, bool isLeft)
        {

        }
    }
}