using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using XR.Interaction.Toolkit.Hand.Utilities.Extensions;
using XR.Interaction.Toolkit.Hand.Utilities.PhysicsUtils;

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class XRInteractorToInteractablePhysicsCollisionDisabler : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody interactorRigidbody = null;
        [SerializeField]
        private XRBaseInteractor interactor = null;


        private readonly List<Collider> interactorColliders = new List<Collider>();
        private readonly List<Collider> interactableColliders = new List<Collider>();


        public Rigidbody InteractorRigidbody { get => interactorRigidbody; set => interactorRigidbody = value; }
        public XRBaseInteractor Interactor { get => interactor; set => interactor = value; }


        protected void OnEnable()
        {
            interactorRigidbody.PopulateWithAllAttachedColliders(interactorColliders);
            interactor.selectEntered.AddListener(OnSelectEntered);
            interactor.selectExited.AddListener(OnSelectExited);
        }

        protected void OnDisable()
        {
            interactor.selectEntered.RemoveListener(OnSelectEntered);
            interactor.selectExited.RemoveListener(OnSelectExited);
        }


        private void OnSelectExited(SelectExitEventArgs eventArgs)
        {
            IgnoreCollision(eventArgs.interactableObject, false);
        }

        private void OnSelectEntered(SelectEnterEventArgs eventArgs)
        {
            IgnoreCollision(eventArgs.interactableObject, true);
        }

        private void IgnoreCollision(IXRSelectInteractable iInteractable, bool ignoreCollision)
        {
            Debug.Log("HH");
            var interactable = iInteractable as MonoBehaviour;
            if (!interactable)
            {
                return;
            }
            Debug.Log("HH1");
            if (interactable.TryGetComponent<Rigidbody>(out var interactableRigidbody))
            {
                interactableRigidbody.PopulateWithAllAttachedColliders(interactableColliders);
            }
            Debug.Log("HH3");
            PhysicsUtilities.IgnoreCollisions(interactorColliders, interactableColliders, ignoreCollision);
            Debug.Log("HH4");
        }
    }
}