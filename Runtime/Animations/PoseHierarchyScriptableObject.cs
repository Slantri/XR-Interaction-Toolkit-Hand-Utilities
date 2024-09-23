#if UNITY_EDITOR
using Unity.XR.CoreUtils;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace XR.Interaction.Toolkit.Hand.Utilities.Animations
{
    [CreateAssetMenu]
    public class PoseHierarchyScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("XRITUtil/PoseHierarchy/GenerateHandPose")]
        public static void GenerateHandPose()
        {
            XRBaseInteractable interactable = null;
            NearFarInteractor interactor = null;
            Transform handTransform = null;

            foreach (var selection in Selection.gameObjects)
            {
                var trackedPoseDriver = selection.transform.GetComponentInParent<TrackedPoseDriver>();
                var interactableSel = selection.GetComponentInParent<XRBaseInteractable>();
                if (trackedPoseDriver)
                {
                    interactor = trackedPoseDriver.GetComponentInChildren<NearFarInteractor>();
                    handTransform = trackedPoseDriver.GetComponentInChildren<PoseHierarchyReceiver>().Root;
                }
                else if (interactableSel)
                {
                    interactable = interactableSel;
                }
            }
            if (interactable && interactor)
            {
                var attachTransform = interactor.GetAttachTransform(interactable);
                var worldPose = attachTransform.GetWorldPose();
                var localPose = interactable.transform.InverseTransformPose(worldPose);
                var hierarchy = ScriptableObject.CreateInstance<PoseHierarchyScriptableObject>();
                hierarchy.poseHierarchy = new PoseHierarchy(handTransform);
                hierarchy.poseHierarchy.Poses[0] = localPose;
                var hand = interactor.handedness == InteractorHandedness.Left ? "left" : "right";
                var filename = string.Format("{0}_{1}", hand, interactable.name);
                var filepath = EditorUtility.SaveFilePanel("Save Hand Pose", Application.dataPath, filename, "asset");
                if (string.IsNullOrEmpty(filepath))
                {
                    return;
                }
                filepath = filepath.Replace(Application.dataPath, "Assets");
                AssetDatabase.CreateAsset(hierarchy, filepath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
#endif
        public PoseHierarchy poseHierarchy;
    }
}