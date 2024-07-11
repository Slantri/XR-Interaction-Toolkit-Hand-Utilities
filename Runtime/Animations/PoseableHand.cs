using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseableHand : MonoBehaviour
    {
        [Serializable]
        public class FingerJoint
        {
            [SerializeField]
            private Transform joint = null;
            [SerializeField]
            private Vector3 localOffset = Vector3.zero;
            [SerializeField]
            private float radiusScale = 1.0f;


            public Transform Joint { get => joint; set => joint = value; }
            public float RadiusScale { get => radiusScale; set => radiusScale = value; }
            public Vector3 LocalOffset { get => localOffset; set => localOffset = value; }
        }

        [Serializable]
        public class Finger
        {
            [SerializeField]
            private float radius = 0.008f;
            [SerializeField]
            private List<FingerJoint> joints = new List<FingerJoint>();


            public float Radius { get => radius; set => radius = value; }
            public List<FingerJoint> Joints { get => joints; set => joints = value; }
        }


        [SerializeField]
        private Transform poseHierarchyRoot = null;
        [SerializeField]
        private List<Finger> fingers = new List<Finger>();
        [SerializeField]
        private XRBaseInteractor interactor = null;
        [SerializeField]
        private PoseHierarchyScriptableObject openHand = null;
        [SerializeField]
        private PoseHierarchyScriptableObject closedhand = null;


        public Transform PoseHierarchyRoot { get => poseHierarchyRoot; set => poseHierarchyRoot = value; }
        public PoseHierarchyScriptableObject OpenHand { get => openHand; set => openHand = value; }
        public PoseHierarchyScriptableObject Closedhand { get => closedhand; set => closedhand = value; }
        public List<Finger> Fingers { get => fingers; set => fingers = value; }
        public XRBaseInteractor Interactor { get => interactor; set => interactor = value; }


#if UNITY_EDITOR
        [ContextMenu("AutoPose")]
        public void AutoPose()
        {

        }

        protected void Reset()
        {
            if (fingers.Count == 0)
            {

            }
        }
#endif



    }
}