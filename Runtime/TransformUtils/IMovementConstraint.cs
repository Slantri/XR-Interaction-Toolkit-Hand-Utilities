using UnityEngine;


namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public interface IMovementConstraint
    {
        public void ApplyConstraint(ref Vector3 position, ref Quaternion rotation);
    }
}