using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DoorHandleGrabRotate : MonoBehaviour
{
    public Transform door; 
    public Transform rotationPivot; 
    private XRBaseInteractor interactor;
    private Quaternion initialDoorRotation;
    private Quaternion initialHandRotation;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
        initialDoorRotation = door.rotation;
        initialHandRotation = interactor.transform.rotation;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        interactor = null;
    }

    void Update()
    {
        if (interactor != null)
        {
            Quaternion currentHandRotation = interactor.transform.rotation;
            Quaternion rotationDelta = currentHandRotation * Quaternion.Inverse(initialHandRotation);
            door.rotation = rotationDelta * initialDoorRotation;
        }
    }
}