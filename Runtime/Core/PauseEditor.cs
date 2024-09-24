using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XR.Interaction.Toolkit.Hand.Utilities.Core
{
    public class PauseEditor : MonoBehaviour
    {
        public void Pause()
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = true;
#endif
        }

        public void TogglePause()
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = !EditorApplication.isPaused;
#endif
        }
    }
}