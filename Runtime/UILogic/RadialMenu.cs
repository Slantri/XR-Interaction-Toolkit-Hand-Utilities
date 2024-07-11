using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;


namespace XR.Interaction.Toolkit.Hand.Utilities.UILogic
{
    public class RadialMenu : MonoBehaviour
    {
        [SerializeField]
        private Transform head = null;
        [SerializeField]
        private Transform hand = null;
        [SerializeField]
        private RectTransform menu = null;
        [SerializeField]
        private RectTransform reticle = null;
        [SerializeField]
        private float diameter = 0.3f;
        [SerializeField]
        private float followDiameter = 0.5f;
        [SerializeField]
        private bool isRight = false;
        [SerializeField]
        private ContinuousTurnProvider continuousTurnProvider = null;
        [SerializeField]
        private SnapTurnProvider snapTurnProvider = null;
        [SerializeField]
        private ContinuousMoveProvider continuousMoveProvider = null;

        private Vector3 localOrigin = Vector3.zero;
        private Vector2 inputDirection = Vector2.zero;
        private bool updateProviders = false;


        public Vector2 InputDirection { get => inputDirection; set => inputDirection = value; }
        public Transform Head { get => head; set => head = value; }
        public Transform Hand { get => hand; set => hand = value; }
        public RectTransform Menu { get => menu; set => menu = value; }
        public RectTransform Reticle { get => reticle; set => reticle = value; }
        public float Diameter { get => diameter; set => diameter = value; }
        public float FollowDiameter { get => followDiameter; set => followDiameter = value; }
        public bool IsRight { get => isRight; set => isRight = value; }
        public ContinuousTurnProvider ContinuousTurnProvider { get => continuousTurnProvider; set => continuousTurnProvider = value; }
        public SnapTurnProvider SnapTurnProvider { get => snapTurnProvider; set => snapTurnProvider = value; }
        public ContinuousMoveProvider ContinuousMoveProvider { get => continuousMoveProvider; set => continuousMoveProvider = value; }


        protected void OnEnable()
        {
            ForceRefresh();
        }

        protected void Update()
        {
            var localToWorld = CalculatBaseMatrix();
            var worldToLocal = localToWorld.inverse;
            var halfDiameter = diameter * 0.5f;

            menu.position = localToWorld.MultiplyPoint(localOrigin);

            var localhandDir = Vector3.ProjectOnPlane(menu.InverseTransformPoint(hand.position), Vector3.forward);

            var followDistSqr = followDiameter * 0.5f;
            followDistSqr *= followDistSqr;
            var diameterSqr = halfDiameter * halfDiameter;
            var localHandDirSqrMag = localhandDir.sqrMagnitude;
            if (localHandDirSqrMag > followDistSqr)
            {
                var handPos = menu.TransformPoint(localhandDir);
                var handDir = menu.TransformDirection(localhandDir.normalized);
                menu.position = handPos - (0.5f * followDiameter * handDir);
            }
            if (localHandDirSqrMag > diameterSqr)
            {
                localhandDir = localhandDir.normalized * diameter * 0.5f;
            }
            reticle.localPosition = localhandDir;

            inputDirection = new Vector2(localhandDir.x / halfDiameter, localhandDir.y / halfDiameter);
            if (inputDirection.sqrMagnitude > 1)
            {
                inputDirection.Normalize();
            }
            if (updateProviders)
            {
                if (isRight)
                {
                    if (continuousTurnProvider)
                    {
                        continuousTurnProvider.rightHandTurnInput.manualValue = inputDirection;
                    }
                    if (continuousMoveProvider)
                    {
                        continuousMoveProvider.rightHandMoveInput.manualValue = inputDirection;
                    }
                    if (snapTurnProvider)
                    {
                        snapTurnProvider.rightHandTurnInput.manualValue = inputDirection;
                    }
                }
                else
                {
                    if (continuousTurnProvider)
                    {
                        continuousTurnProvider.leftHandTurnInput.manualValue = inputDirection;
                    }
                    if (continuousMoveProvider)
                    {
                        continuousMoveProvider.leftHandMoveInput.manualValue = inputDirection;
                    }
                    if (snapTurnProvider)
                    {
                        snapTurnProvider.leftHandTurnInput.manualValue = inputDirection;
                    }
                }
            }
        }


        public void ForceRefresh()
        {
            var position = hand.position;
            var worldToLocal = CalculatBaseMatrix().inverse;
            localOrigin = worldToLocal.MultiplyPoint(hand.position);
            var rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(hand.forward, Vector3.up).normalized, Vector3.up) * Quaternion.Euler(90, 0, 0);
            menu.SetPositionAndRotation(position, rotation);
            reticle.localPosition = Vector3.zero;
        }

        public void ApplyInputValue()
        {
            updateProviders = true;
        }

        public void ClearInput()
        {
            updateProviders = false;
            if (isRight)
            {
                if (continuousTurnProvider)
                {
                    continuousTurnProvider.rightHandTurnInput.manualValue = Vector2.zero;
                }
                if (continuousMoveProvider)
                {
                    continuousMoveProvider.rightHandMoveInput.manualValue = Vector2.zero;
                }
                if (snapTurnProvider)
                {
                    snapTurnProvider.rightHandTurnInput.manualValue = Vector2.zero;
                }
            }
            else
            {
                if (continuousTurnProvider)
                {
                    continuousTurnProvider.leftHandTurnInput.manualValue = Vector2.zero;
                }
                if (continuousMoveProvider)
                {
                    continuousMoveProvider.leftHandMoveInput.manualValue = Vector2.zero;
                }
                if (snapTurnProvider)
                {
                    snapTurnProvider.leftHandTurnInput.manualValue = Vector2.zero;
                }
            }
        }


        private Matrix4x4 CalculatBaseMatrix()
        {
            return Matrix4x4.TRS(head.position, head.parent.rotation, head.lossyScale);
        }
    }
}