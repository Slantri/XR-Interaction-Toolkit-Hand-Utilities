using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class SyncPoseHierarchy : MonoBehaviour
    {
        [SerializeField]
        private bool clearRootPose = false;
        [SerializeField]
        private Transform sourceRoot = null;
        [SerializeField]
        private Transform destinationRoot = null;


        private PoseHierarchy poseHierarchy = new PoseHierarchy();


        public bool ClearRootPose { get => clearRootPose; set => clearRootPose = value; }
        public Transform SourceRoot { get => sourceRoot; set => sourceRoot = value; }
        public Transform DestinationRoot { get => destinationRoot; set => destinationRoot = value; }


        protected void Update()
        {
            poseHierarchy.Update(sourceRoot);
            if (clearRootPose && poseHierarchy.Poses.Count > 0)
            {
                poseHierarchy.Poses[0] = Pose.identity;
            }
            poseHierarchy.Apply(destinationRoot);
        }
    }
}