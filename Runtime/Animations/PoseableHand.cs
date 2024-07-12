using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using XR.Interaction.Toolkit.Hand.Utilities.Extensions;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseableHand : PoseHierarchyDestination
    {
        [Flags]
        public enum FingerTypes
        {
            None = 0x0,
            Thumb = 0x1,
            Index = 0x2,
            Middle = 0x4,
            Ring = 0x8,
            Pinky = 0x10,
        }


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


            public FingerJoint() { }

            public FingerJoint(Transform joint)
            {
                this.joint = joint;
            }


#if UNITY_EDITOR
            public void DebugDraw(float radius, FingerJoint next)
            {
                radius = radius * radiusScale;
                Gizmos.DrawSphere(joint.position, radius);
                if (next != null)
                {
                    var p0 = joint.position;
                    var p1 = next.joint.position;
                    Gizmos.DrawSphere(Vector3.Lerp(p0, p1, 0.25f), radius);
                    Gizmos.DrawSphere(Vector3.Lerp(p0, p1, 0.5f), radius);
                    Gizmos.DrawSphere(Vector3.Lerp(p0, p1, 0.75f), radius);
                }
            }
#endif
        }


        [Serializable]
        public class Finger
        {
            [SerializeField]
            private FingerTypes fingerType = FingerTypes.None;
            [SerializeField]
            private float radius = 0.008f;
            [SerializeField]
            private List<FingerJoint> joints = new List<FingerJoint>();


            public float Radius { get => radius; set => radius = value; }
            public List<FingerJoint> Joints { get => joints; set => joints = value; }


            public Finger() { }

            public Finger(List<Transform> fingerJointTransforms, FingerTypes fingerType)
            {
                this.fingerType = fingerType;
                foreach (var joint in fingerJointTransforms)
                {
                    joints.Add(new FingerJoint(joint));
                }
            }


            public void PreventCollision(List<Transform> transforms, PoseHierarchy currentPose, PoseHierarchy desiredPose, PoseHierarchy openPose)
            {
                PoseHierarchy curHierarchy = new PoseHierarchy();
                PoseHierachy targetHierarcy = new PoseHierachy();
                PoseHierarchy
            }


#if UNITY_EDITOR
            public void DebugDraw()
            {
                for (int i = 0, n = joints.Count; i < n; i++)
                {
                    joints[i].DebugDraw(radius, i + 1 < n ? joints[i + 1] : null);
                }
            }
#endif
        }


        [SerializeField]
        private bool isLeft = false;
        [SerializeField]
        private List<Finger> fingers = new List<Finger>();
        [SerializeField]
        private XRBaseInteractor interactor = null;
        [SerializeField]
        private PoseHierarchyScriptableObject openHand = null;
        [SerializeField]
        private PoseHierarchyScriptableObject closedhand = null;


        private PoseHierarchy poseHierarchy = null;
        private PoseableHandInteractablePose targetInteractablePose = null;
        private readonly List<Transform> transforms = new List<Transform>();


        public PoseHierarchyScriptableObject OpenHand { get => openHand; set => openHand = value; }
        public PoseHierarchyScriptableObject Closedhand { get => closedhand; set => closedhand = value; }
        public List<Finger> Fingers { get => fingers; set => fingers = value; }
        public XRBaseInteractor Interactor { get => interactor; set => interactor = value; }


#if UNITY_EDITOR
        [ContextMenu("AutoPose")]
        public void AutoPose()
        {
            HierarchyRoot.PopulateWithAllChildTransformsDepthFirst(transforms);
            Apply(Closedhand.poseHierarchy);
        }

        protected void Reset()
        {
            if (HierarchyRoot == null)
            {
                HierarchyRoot = transform;
                isLeft = transform.name.StartsWith("L", StringComparison.InvariantCultureIgnoreCase);
            }
            if (fingers.Count == 0 && HierarchyRoot != null)
            {
                List<Transform> fingerJoints = new List<Transform>();
                HierarchyRoot.FindAllIncludingSelf("thumb", fingerJoints);
                fingers.Add(new Finger(fingerJoints, FingerTypes.Thumb));

                fingerJoints.Clear();
                HierarchyRoot.FindAllIncludingSelf("index", fingerJoints);
                fingers.Add(new Finger(fingerJoints, FingerTypes.Index));

                fingerJoints.Clear();
                HierarchyRoot.FindAllIncludingSelf("middle", fingerJoints);
                fingers.Add(new Finger(fingerJoints, FingerTypes.Middle));


                fingerJoints.Clear();
                HierarchyRoot.FindAllIncludingSelf("ring", fingerJoints);
                fingers.Add(new Finger(fingerJoints, FingerTypes.Ring));


                fingerJoints.Clear();
                HierarchyRoot.FindAllIncludingSelf("little", fingerJoints);
                fingers.Add(new Finger(fingerJoints, FingerTypes.Pinky));
            }
        }

        protected void OnDrawGizmosSelected()
        {
            foreach (var finger in fingers)
            {
                finger.DebugDraw();
            }
        }
#endif


        protected void OnEnable()
        {
            HierarchyRoot.PopulateWithAllChildTransformsDepthFirst(transforms);
            if (interactor)
            {
                interactor.selectEntered.AddListener(OnSelectEntered);
                interactor.selectExited.AddListener(OnSelectExited);
            }
        }

        protected void OnDisable()
        {
            if (interactor)
            {
                interactor.selectEntered.RemoveListener(OnSelectEntered);
                interactor.selectExited.RemoveListener(OnSelectExited);
            }
        }


        public override void Apply(PoseHierarchy hierarchy)
        {
            poseHierarchy.Clone(hierarchy);
            if (openHand)
            {
                foreach (var finger in fingers)
                {
                    finger.PreventCollision(transforms, poseHierarchy, openHand.poseHierarchy);
                }
            }
            if (targetInteractablePose)
            {
                targetInteractablePose.Apply(poseHierarchy, isLeft);
            }
            if (interactor && interactor.hasSelection)
            {
                /*
                add code to force hand to interactable so they do not separate
                */
            }
            hierarchy.Apply(HierarchyRoot);
        }


        private void OnSelectEntered(SelectEnterEventArgs arg0)
        {
            var interactable = arg0.interactableObject;
            var baseInteractable = interactable as XRBaseInteractable;
            if (baseInteractable)
            {
                baseInteractable.TryGetComponent<PoseableHandInteractablePose>(out targetInteractablePose);
            }
        }

        private void OnSelectExited(SelectExitEventArgs arg0)
        {
            targetInteractablePose = null;
        }
    }
}