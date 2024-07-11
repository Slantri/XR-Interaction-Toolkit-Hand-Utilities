using System.Collections.Generic;
using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.PhysicsUtils
{
    public class PhysicsUtilities
    {
        public static void IgnoreCollisions(List<Collider> collidersA, List<Collider> collidersB, bool ignore)
        {
            for (int i = 0, n = collidersA.Count; i < n; i++)
            {
                for (int iB = 0, nB = collidersB.Count; iB < nB; iB++)
                {
                    Physics.IgnoreCollision(collidersA[i], collidersB[iB], ignore);
                }
            }
        }
    }
}