using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.BaseTeleportationInteractable;

public class LoopHallway : MonoBehaviour
{
    public Transform teleportPoint;
    public bool isForwardTeleporter;
    public GameObject otherTeleporter; 

    private bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            other.transform.position = teleportPoint.position;

            if (cc != null) cc.enabled = true;

            Debug.Log($"{gameObject.name} teleported the player!");

            if (isForwardTeleporter && otherTeleporter != null)
            {
                LoopHallway otherfart = otherTeleporter.GetComponent<LoopHallway>();
                if (otherfart != null)
                {
                    otherfart.DisableTemporarily();
                }
            }
        }
    }

    public void DisableTemporarily()
    {
        isActive = false;
        Invoke(nameof(EnableAgain), 2f); // after 2 seconds
    }

    private void EnableAgain()
    {
        isActive = true;
    }
}