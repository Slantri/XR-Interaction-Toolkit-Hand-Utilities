using System;
using System.Collections.Generic;
using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class PoseHierarchyReceiver : MonoBehaviour
    {
        private class PoseHierarchySubscription : IDisposable
        {
            private PoseHierarchyReceiver receiver = null;
            private PoseHierarchy poseHierarchy = null;
            private float priority = 0;


            public float Priority { get => priority; set => priority = value; }
            public PoseHierarchyReceiver Receiver { get => receiver; set => receiver = value; }
            public PoseHierarchy PoseHierarchy { get => poseHierarchy; set => poseHierarchy = value; }


            public PoseHierarchySubscription(PoseHierarchyReceiver receiver, PoseHierarchy poseHierarchy, float priority)
            {
                this.receiver = receiver;
                this.poseHierarchy = poseHierarchy;
                this.priority = priority;
                receiver.Add(this);
            }


            public void Dispose()
            {
                if (receiver == null)
                {
                    return;
                }
                receiver.Remove(this);
                receiver = null;
                poseHierarchy = null;
            }
        }


        [SerializeField]
        private Transform root = null;


        private PoseHierarchySubscription prioritySub = null;
        private List<PoseHierarchySubscription> poseHierarchySubs = new List<PoseHierarchySubscription>();


        public Transform Root { get => root; set => root = value; }


#if UNITY_EDITOR
        protected void Reset()
        {
            if (root == null)
            {
                root = transform;
            }
        }
#endif

        protected void Update()
        {
            if (prioritySub == null)
            {
                return;
            }

            prioritySub.PoseHierarchy.Apply(root);
        }


        public IDisposable Apply(PoseHierarchy poseHierarchy, float priority)
        {
            return new PoseHierarchySubscription(this, poseHierarchy, priority);
        }


        private void Add(PoseHierarchySubscription subscription)
        {
            poseHierarchySubs.Add(subscription);
            Refresh();
        }

        private void Remove(PoseHierarchySubscription subscription)
        {
            poseHierarchySubs.Remove(subscription);
            Refresh();
        }

        private void Refresh()
        {
            prioritySub = null;
            foreach (var sub in poseHierarchySubs)
            {
                if (prioritySub == null || sub.Priority > prioritySub.Priority)
                {
                    prioritySub = sub;
                }
            }
        }
    }
}