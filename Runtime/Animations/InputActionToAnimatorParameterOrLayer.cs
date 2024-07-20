using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    public class InputActionToAnimatorParameterOrLayer : MonoBehaviour
    {
        [Serializable]
        public enum ParameterTypes
        {
            Float = 0x1,
            FloatInverted = 0x2,
            Bool = 0x4,
            BoolInverted = 0x8,
            LayerWeight = 0x10,
            LayerWeightIverted = 0x20,
        }


        [Serializable]
        public class Mapping
        {
            [SerializeField]
            private InputActionProperty input;
            [SerializeField]
            private string key = "";
            [SerializeField]
            private ParameterTypes parameterType = ParameterTypes.Float;


            private Animator animator = null;
            private int keyIndex = 0;


            public InputActionProperty Input { get => input; set => input = value; }
            public string Key { get => key; set => key = value; }
            public ParameterTypes ParameterType { get => parameterType; set => parameterType = value; }


            public void Setup(Animator animator)
            {
                keyIndex = parameterType == ParameterTypes.LayerWeight || parameterType == ParameterTypes.LayerWeightIverted ? animator.GetLayerIndex(key) : Animator.StringToHash(key);
                this.animator = animator;
                var inputAction = input.action;
                if (input.reference == null)
                {
                    inputAction.Enable();
                }
                inputAction.performed += OnPerformed;
            }

            public void Cleanup()
            {
                var inputAction = input.action;
                if (input.reference == null)
                {
                    inputAction.Disable();
                }
                inputAction.performed -= OnPerformed;
            }


            private void OnPerformed(InputAction.CallbackContext context)
            {
                if (animator)
                {
                    var value = context.action.ReadValue<float>();
                    switch (parameterType)
                    {
                        case ParameterTypes.Bool:
                            animator.SetBool(keyIndex, context.action.IsPressed());
                            break;
                        case ParameterTypes.BoolInverted:
                            animator.SetBool(keyIndex, !context.action.IsPressed());
                            break;
                        case ParameterTypes.Float:
                            animator.SetFloat(keyIndex, context.action.ReadValue<float>());
                            break;
                        case ParameterTypes.FloatInverted:
                            animator.SetFloat(keyIndex, 1.0f - context.action.ReadValue<float>());
                            break;
                        case ParameterTypes.LayerWeight:
                            animator.SetLayerWeight(keyIndex, context.action.ReadValue<float>());
                            break;
                        case ParameterTypes.LayerWeightIverted:
                            animator.SetLayerWeight(keyIndex, 1.0f - context.action.ReadValue<float>());
                            break;
                    }
                }
            }
        }


        [SerializeField]
        private Animator animator = null;
        [SerializeField]
        private List<Mapping> inputMapping = new List<Mapping>();


        public Animator Animator { get => animator; set => animator = value; }
        public List<Mapping> InputMapping { get => inputMapping; set => inputMapping = value; }


        protected void OnEnable()
        {
            foreach (var inputMap in inputMapping)
            {
                inputMap.Setup(animator);
            }
        }

        protected void OnDisable()
        {
            foreach (var inputMap in inputMapping)
            {
                inputMap.Cleanup();
            }
        }
    }
}