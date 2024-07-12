using System.Runtime.InteropServices;
using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseHierarchySource : MonoBehaviour
    {
        [SerializeField]
        private bool clearRootPose = false;
        [SerializeField]
        private Transform hierarchyRoot = null;
        [SerializeField]
        private PoseHierarchyDestination destination = null;


        private PoseHierarchy poseHierarchy = new PoseHierarchy();


        public bool ClearRootPose { get => clearRootPose; set => clearRootPose = value; }
        public Transform HierarchyRoot { get => hierarchyRoot; set => hierarchyRoot = value; }
        public PoseHierarchyDestination Destination { get => destination; set => destination = value; }


        protected void Update()
        {
            poseHierarchy.Update(hierarchyRoot);
            if (clearRootPose && poseHierarchy.Poses.Count > 0)
            {
                poseHierarchy.Poses[0] = Pose.identity;
            }
            destination.Apply(poseHierarchy);
        }
    }
}