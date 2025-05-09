using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    public AudioClip footstepClip;
    public float stepRate = 0.5f; 

    private AudioSource audioSource;
    private CharacterController controller;
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller != null && controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(footstepClip);
                stepTimer = stepRate;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}
