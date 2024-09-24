#XR Interaction Toolkit Hand Utilities
This is a utilities package to add extra functionality to XR Hands and XR Interaction Toolkit.

##Functionality
- Hand Tracking and Controller Support by Default
- Interactables:
    - XRGrabInteractableHandPose: Supports a left and right hand grab hand pose for when grabbing an interactable
- Hand Tracking:
    - Auto Mapping to grip, trigger, menu, device position, device rotation
    - Hand Tracking pose passed through to rendered hand
    - Palm Up three finger curl to start teleportation with pinch activate
    - Plam Up three finger curl for smooth locomotion with pinch activate
    - Plam To Center Three finger curl rotation with pinch to activate

##Setup
- Install XR Interaction Toolkit v3.0.4
    - Samples to Import:
        - Starter Assets
        - XR Device Simulator
        - Spatial Keyboard
- Install XR Hands v1.4.1
    - Samples to Import:
        - Gestures
        - HandVisualizers
- Open Project Settings/XR Plug-in management
    - Set OpenXR as Android and Desktop Default
    - Interaction Profile:
        - Desired Controller or Oculus Touch Controller Profile
    - OpenXR Feature Groups For Desktop and Android:
        - Hand Tracking Subsystem
        - Meta Hand Tracking Aim
        - Meta Quest Support (Android and if targeting Oculus)

##Usage
- Setup scene as you would for a normal Unity XR Interaction Toolkit project
- Replace the Player Rig with Samples\XR Interaction Toolkit Hand Utilities\1.0.0-pre.1\Player Rig\Prefabs\XR Origin (XR Rig) Variant.prefab
- Click Play

##Creating A Hand Pose
- Enter Play Mode
- Position hand on Interactable in desired pose
- Pause Editor, Click on the Interactable in Scene View, Shift+Click on the Hand in scene View
- Menu XRITUtil/PoseHierarchy/GenerateHandPose
- Save Pose
- In Edit Mode change the interactable to use XRGrabInteractableHandPose
- Assign your created hand pose to either Left Hand or Right Hand