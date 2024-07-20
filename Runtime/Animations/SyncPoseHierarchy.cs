using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class SyncPoseHierarchy : MonoBehaviour
    {
        [SerializeField]
        private int posesToClear = 0;
        [SerializeField]
        private int posesToClearOffset = 0;
        [SerializeField]
        private Transform sourceRoot = null;
        [SerializeField]
        private Transform destinationRoot = null;


        private PoseHierarchy poseHierarchy = new PoseHierarchy();


        public Transform SourceRoot { get => sourceRoot; set => sourceRoot = value; }
        public Transform DestinationRoot { get => destinationRoot; set => destinationRoot = value; }
        public int PosesToClear { get => posesToClear; set => posesToClear = value; }
        public int PosesToClearOffset { get => posesToClearOffset; set => posesToClearOffset = value; }


        protected void Update()
        {
            poseHierarchy.Update(sourceRoot);
            for (int i = posesToClearOffset; i < posesToClearOffset + posesToClear; i++)
            {
                poseHierarchy.Poses[i] = Pose.identity;
            }
            poseHierarchy.Apply(destinationRoot);
        }
    }
}