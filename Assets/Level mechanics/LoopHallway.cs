using UnityEngine;

public class LoopHallway : MonoBehaviour
{
    public Transform teleportPoint; // Where to teleport back to
    public GameObject environmentManager; // Handles changes per loop
    private int loopCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // disable to prevent teleport glitches
            }

            other.transform.position = teleportPoint.position;

            if (cc != null)
            {
                cc.enabled = true;
            }

            loopCount++;
            environmentManager.GetComponent<EnvironmentChanger>().OnNewLoop(loopCount);
        }
    }
}
