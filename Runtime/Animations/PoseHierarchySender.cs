using System;
using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseHierarchySender : MonoBehaviour
    {
        [SerializeField]
        private int posesToClear = 0;
        [SerializeField]
        private int posesToClearOffset = 0;
        [SerializeField]
        private Transform root = null;
        [SerializeField]
        private PoseHierarchyReceiver poseHierarchyReceiver = null;
        [SerializeField]
        private float priority = 0.0f;


        private PoseHierarchy poseHierarchy = new PoseHierarchy();
        private IDisposable receiverSub = null;


        public int PosesToClear { get => posesToClear; set => posesToClear = value; }
        public int PosesToClearOffset { get => posesToClearOffset; set => posesToClearOffset = value; }
        public Transform Root { get => root; set => root = value; }
        public PoseHierarchyReceiver PoseHierarchyReceiver { get => poseHierarchyReceiver; set => poseHierarchyReceiver = value; }
        public float Priority { get => priority; set => priority = value; }


        protected void OnEnable()
        {
            receiverSub = poseHierarchyReceiver.Apply(poseHierarchy, priority, false);
        }

        protected void OnDisable()
        {
            if (receiverSub != null)
            {
                receiverSub.Dispose();
                receiverSub = null;
            }
        }


        protected void Update()
        {
            poseHierarchy.Update(root);
            for (int i = posesToClearOffset; i < posesToClearOffset + posesToClear; i++)
            {
                poseHierarchy.Poses[i] = Pose.identity;
            }
        }
    }
}