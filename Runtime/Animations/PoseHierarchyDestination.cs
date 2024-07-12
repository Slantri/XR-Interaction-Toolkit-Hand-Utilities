using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseHierarchyDestination : MonoBehaviour
    {
        [SerializeField]
        private Transform hierarchyRoot = null;

        public Transform HierarchyRoot { get => hierarchyRoot; set => hierarchyRoot = value; }

        public virtual void Apply(PoseHierarchy hierarchy)
        {
            hierarchy.Apply(hierarchyRoot);
        }
    }
}