using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace XR.Interaction.Toolkit.Hand.Utilities.InputSystemUtilities
{
    public class XRRayInteractorInputSetter : MonoBehaviour
    {
        private const float floatToBoolThreshold = 0.65f;
        [SerializeField]
        private XRRayInteractor interactor = null;


        public XRRayInteractor Interactor { get => interactor; set => interactor = value; }


        public void SetSelectInputValue(float value)
        {
            interactor.selectInput.manualPerformed = value > floatToBoolThreshold ? true : false;
            interactor.selectInput.manualValue = value;
        }

        public void SetActivateInputValue(float value)
        {
            interactor.activateInput.manualPerformed = value > floatToBoolThreshold ? true : false;
            interactor.activateInput.manualValue = value;
        }
    }
}