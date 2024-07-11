using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.Scripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [Preserve]
    public static class HandTrackedXRControllerLoader
    {
        [Preserve]
        static HandTrackedXRControllerLoader()
        {
            RegisterInputLayouts();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad), Preserve]
        public static void Initialize()
        {
            // Will execute the static constructor as a side effect.
        }

        static void RegisterInputLayouts()
        {
            UnityEngine.InputSystem.InputSystem.RegisterLayout<HandTrackedXRController>(
                matches: new InputDeviceMatcher()
                    .WithProduct(nameof(HandTrackedXRController)));
        }
    }
}
