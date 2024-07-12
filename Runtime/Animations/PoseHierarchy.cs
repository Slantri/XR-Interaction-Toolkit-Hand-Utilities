using System;
using System.Collections.Generic;
using UnityEngine;
using XR.Interaction.Toolkit.Hand.Utilities.Extensions;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    [Serializable]
    public class PoseHierarchy
    {
        private static readonly List<Transform> hierarchyTransformsTemp = new List<Transform>();


        [SerializeField]
        private List<Pose> poses = new List<Pose>();


        public List<Pose> Poses => poses;


        public PoseHierarchy() { }

        public PoseHierarchy(Transform root) : this(root.PopulateWithAllChildTransformsDepthFirst(hierarchyTransformsTemp))
        {

        }

        public PoseHierarchy(List<Transform> transforms)
        {
            Update(transforms);
        }


        public void Apply(Transform root)
        {
            Apply(root.PopulateWithAllChildTransformsDepthFirst(hierarchyTransformsTemp));
        }

        public void Apply(List<Transform> transforms)
        {
            if (transforms.Count != poses.Count)
            {
                return;
            }

            for (int i = 0; i < transforms.Count; i++)
            {
                var pose = poses[i];
                transforms[i].SetLocalPositionAndRotation(pose.position, pose.rotation);
            }
        }

        public void Update(Transform root)
        {
            Update(root.PopulateWithAllChildTransformsDepthFirst(hierarchyTransformsTemp));
        }

        public void Update(List<Transform> transforms)
        {
            poses.Clear();
            foreach (var transform in transforms)
            {
                poses.Add(new Pose(transform.localPosition, transform.localRotation));
            }
        }

        public void Clone(PoseHierarchy source)
        {
            poses.Clear();
            foreach (var pose in source.Poses)
            {
                poses.Add(pose);
            }
        }
    }
}