using System.Collections.Generic;
using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Extensions
{
    public static class TransformExtensions
    {
        public static void FindAllIncludingSelf(this Transform transform, string text, List<Transform> children, bool recursive = true)
        {
            if (transform.name.Contains(text, System.StringComparison.InvariantCultureIgnoreCase))
            {
                children.Add(transform);
            }
            for (int i = 0, n = transform.childCount; i < n; i++)
            {
                FindAllIncludingSelf(transform.GetChild(i), text, children, recursive);
            }
        }

        public static List<Transform> PopulateWithAllChildTransformsDepthFirst(this Transform root, List<Transform> transforms)
        {
            transforms.Clear();
            PopulateWithAllChildTransformsDepthFirstNonExtension(root, transforms);
            return transforms;
        }

        private static void PopulateWithAllChildTransformsDepthFirstNonExtension(Transform transform, List<Transform> transforms)
        {
            transforms.Add(transform);
            for (int i = 0, n = transform.childCount; i < n; i++)
            {
                PopulateWithAllChildTransformsDepthFirst(transform.GetChild(i), transforms);
            }
        }
    }
}