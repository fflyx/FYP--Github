using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwingDoor : MonoBehaviour
{
    public Transform doorTransform;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private bool isOpening = false;

    private void Awake()
    {
        var interactable = GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(_ => OpenDoor());
    }

    private void OpenDoor()
    {
        if (!isOpening)
            StartCoroutine(SwingOpen());
    }

    private IEnumerator SwingOpen()
    {
        isOpening = true;
        Quaternion startRot = doorTransform.localRotation;
        Quaternion endRot = Quaternion.Euler(0, openAngle, 0);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * openSpeed;
            doorTransform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        isOpening = false;
    }
}
