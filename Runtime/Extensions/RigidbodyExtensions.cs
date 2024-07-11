using System.Collections.Generic;
using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Extensions
{
    public static class RigidbodyExtensions
    {
        public static void PopulateWithAllAttachedColliders(this Rigidbody body, List<Collider> attachedColliders)
        {
            if (attachedColliders == null)
            {
                return;
            }
            attachedColliders.Clear();
            body.GetComponentsInChildren<Collider>(true, attachedColliders);
            for (int i = attachedColliders.Count - 1; i >= 0; i--)
            {
                if (attachedColliders[i].attachedRigidbody != body)
                {
                    attachedColliders.RemoveAt(i);
                }
            }
        }
    }
}