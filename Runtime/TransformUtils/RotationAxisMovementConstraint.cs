using UnityEngine;
using XR.Interaction.Toolkit.Hand.Utilities.Extensions;

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class RotationAxisMovementConstraint : MonoBehaviour, IMovementConstraint
    {
        [SerializeField]
        private Vector3 rotationAxis = Vector3.up;


        public Vector3 RotationAxis { get => rotationAxis; set => rotationAxis = value; }


        public void ApplyConstraint(ref Vector3 position, ref Quaternion rotation)
        {
            rotation = rotation.GetRotationAbout(rotationAxis);
        }
    }
}