using System.Collections.Generic;
using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseHierarchy
    {
        private List<Pose> poses = new List<Pose>();


        public List<Pose> Poses => poses;


        public PoseHierarchy() { }

        public PoseHierarchy(Transform root) : this(GetAllTransforms(root))
        {

        }

        public PoseHierarchy(List<Transform> transforms)
        {
            Update(transforms);
        }


        public void Apply(Transform root)
        {
            Apply(GetAllTransforms(root));
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
            Update(GetAllTransforms(root));
        }

        public void Update(List<Transform> transforms)
        {
            poses.Clear();
            foreach (var transform in transforms)
            {
                poses.Add(new Pose(transform.localPosition, transform.localRotation));
            }
        }


        private static List<Transform> GetAllTransforms(Transform root)
        {
            var transforms = new List<Transform>();
            GetAllTransforms(root, transforms);
            return transforms;
        }

        private static void GetAllTransforms(Transform transform, List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0; i < transform.childCount; i++)
            {
                GetAllTransforms(transform.GetChild(i), transforms);
            }
        }
    }
}