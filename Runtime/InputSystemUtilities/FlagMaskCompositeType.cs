using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
#if UNITY_EDITOR
    [InitializeOnLoad] // Automatically register in editor.
#endif
    [DisplayStringFormat("{Input}+{Mask}")]
    public class FlagMaskCompositeType : InputBindingComposite<float>
    {
        [UnityEngine.InputSystem.Layouts.InputControl(layout = "Integer")]
        public int Input;
        public int Mask;

        public override float ReadValue(ref InputBindingCompositeContext context)
        {
            var val = context.ReadValue<int>(Input);

            return (val & Mask) == Mask ? 1 : 0;
        }

        public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
        {
            return ReadValue(ref context);
        }

        static FlagMaskCompositeType()
        {
            InputSystem.RegisterBindingComposite<FlagMaskCompositeType>();
        }

        [RuntimeInitializeOnLoadMethod]
        static void Init() { }
    }
}