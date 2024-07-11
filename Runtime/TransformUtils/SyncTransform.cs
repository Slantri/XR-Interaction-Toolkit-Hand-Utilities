using System.Collections.Generic;
using UnityEngine;

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class SyncTransform : MonoBehaviour
    {
        [SerializeField]
        private Transform source = null;
        [SerializeField]
        private Transform destination = null;
        [SerializeField]
        private List<MonoBehaviour> constraints = new List<MonoBehaviour>();
        [SerializeField]
        private bool localSpace = false;


        private List<IMovementConstraint> movementConstraints = new List<IMovementConstraint>();


        public Transform Source { get => source; set => source = value; }
        public Transform Destination { get => destination; set => destination = value; }
        public bool LocalSpace { get => localSpace; set => localSpace = value; }
        public List<MonoBehaviour> Constraints => constraints;

        protected void Awake()
        {
            foreach (var constraint in constraints)
            {
                var constraintMovement = constraint as IMovementConstraint;
                if (constraintMovement != null)
                {
                    movementConstraints.Add(constraintMovement);
                }
            }
        }

        protected void Update()
        {
            var position = localSpace ? source.localPosition : source.position;
            var rotation = localSpace ? source.localRotation : source.rotation;

            foreach (var movementConstraint in movementConstraints)
            {
                movementConstraint.ApplyConstraint(ref position, ref rotation);
            }

            if (localSpace)
            {
                destination.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                destination.SetPositionAndRotation(position, rotation);
            }
        }
    }
}