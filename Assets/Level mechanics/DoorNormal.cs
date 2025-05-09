using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorInteraction : MonoBehaviour
{
    public Transform doorTransform; // The door object itself
    public float openAngle = 90f; // The angle to open the door
    public float doorSpeed = 2f; // Speed at which the door opens
    public XRGrabInteractable grabInteractable; // The grab interactable object for the door
    private bool isOpen = false; // To check if the door is open
    private bool isInteracting = false; // To prevent multiple interactions

    private void Awake()
    {
        if (grabInteractable == null)
        {
            grabInteractable = GetComponent<XRGrabInteractable>();
        }

        // Subscribe to the interaction events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // When the door is grabbed, start opening or closing
        if (!isInteracting)
        {
            isInteracting = true;
            StartCoroutine(OpenOrCloseDoor());
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // When the door is released, stop interacting
        isInteracting = false;
    }

    private IEnumerator OpenOrCloseDoor()
    {
        float targetAngle = isOpen ? 0f : openAngle; // Set target angle based on whether the door is already open

        float currentAngle = doorTransform.localRotation.eulerAngles.y;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            float smoothAngle = Mathf.Lerp(currentAngle, targetAngle, timeElapsed * doorSpeed);
            doorTransform.localRotation = Quaternion.Euler(0f, smoothAngle, 0f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        doorTransform.localRotation = Quaternion.Euler(0f, targetAngle, 0f); // Ensure the door reaches the target angle exactly
        isOpen = !isOpen; // Toggle the door's state
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events to prevent memory leaks
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}
