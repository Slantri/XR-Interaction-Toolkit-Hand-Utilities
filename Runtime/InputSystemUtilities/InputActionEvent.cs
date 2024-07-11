using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class InputActionEvent : MonoBehaviour
    {
        [SerializeField]
        private InputActionProperty input;
        [SerializeField]
        private UnityEvent onStartedEvents = new UnityEvent();
        [SerializeField]
        private UnityEvent onPerformedEvents = new UnityEvent();
        [SerializeField]
        private UnityEvent onStoppedEvents = new UnityEvent();


        protected void OnEnable()
        {
            var action = input.action;
            if (input.reference == null)
            {
                action.Enable();
            }
            action.started += OnStarted;
            action.canceled += OnStopped;
            action.performed += OnPerformed;
        }

        protected void OnDisable()
        {
            var action = input.action;
            action.started -= OnStarted;
            action.canceled -= OnStopped;
            action.performed -= OnPerformed;
            if (input.reference == null)
            {
                action.Disable();
            }
        }


        private void OnStarted(InputAction.CallbackContext context)
        {
            onStartedEvents?.Invoke();
        }

        private void OnStopped(InputAction.CallbackContext context)
        {
            onStoppedEvents?.Invoke();
        }

        private void OnPerformed(InputAction.CallbackContext context)
        {
            onPerformedEvents?.Invoke();
        }

    }
}