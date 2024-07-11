using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion GetRotationAbout(this Quaternion rotation, Vector3 axis)
        {
            var xyz = new Vector3(rotation.x, rotation.y, rotation.z);
            var proj = Vector3.Project(xyz, axis);
            var partialRotation = new Quaternion(proj.x, proj.y, proj.z, rotation.w);
            return partialRotation.normalized;
        }
    }
}