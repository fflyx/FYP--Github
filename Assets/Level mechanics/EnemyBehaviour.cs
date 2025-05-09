using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyBehaviour : MonoBehaviour
{
    public float jumpscareDelay = 3f;
    public float jumpscareDistance = 13f;
    public AudioClip jumpscareSound;
    private AudioSource audioSource;
    private bool hasScared = false;

    public enum EnemyMode { PassiveDisappear, ChaseWhenUnseen }
    private NavMeshAgent navMeshAgent;

    [Header("General Settings")]
    public EnemyMode mode = EnemyMode.PassiveDisappear;
    public Transform player;
    public float detectionDistance = 30f;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;

    [Header("Disappear Settings")]
    public float visibleDuration = 3f;

    private bool isSeen = false;
    private Coroutine disappearCoroutine;

    void Start()
    {
        if (player == null)
            player = Camera.main.transform;

        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null && Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        bool visible = IsVisibleToPlayer();
        Debug.Log("Enemy Visible: " + visible);

        switch (mode)
        {
            case EnemyMode.PassiveDisappear:
                if (visible && !isSeen)
                {
                    isSeen = true;
                    disappearCoroutine = StartCoroutine(DisappearAfterSeen());
                }
                break;

            case EnemyMode.ChaseWhenUnseen:
                if (!visible)
                {
                    // When the enemy is not visible, it moves towards the player
                    ChasePlayer();
                }
                else
                {
                    // If the enemy sees the player, stop chasing
                    StopChase();
                    if (Vector3.Distance(transform.position, player.position) <= jumpscareDistance && !hasScared)
                    {
                        Jumpscare();
                    }
                }
                break;
        }
    }

    IEnumerator DisappearAfterSeen()
    {
        yield return new WaitForSeconds(visibleDuration);
        gameObject.SetActive(false);
    }

    void ChasePlayer()
    {
        if (navMeshAgent != null && !IsVisibleToPlayer())
        {
            // If the enemy is not visible, it chases the player
            float distance = Vector3.Distance(transform.position, player.position);
            Debug.Log("Distance to Player: " + distance);

            // If the distance is within the jumpscare range and the enemy hasn't scared the player
            if (distance <= jumpscareDistance && !hasScared)
            {
                Jumpscare();
            }

            navMeshAgent.SetDestination(player.position);  // Chase the player
            navMeshAgent.speed = chaseSpeed;  // Optional: Adjust chase speed
        }
        else
        {
            // If the enemy is visible to the player, stop chasing
            StopChase();
        }
    }

    IEnumerator Jumpscare()
    {
        hasScared = true;

        if (audioSource != null && jumpscareSound != null)
        {
            audioSource.PlayOneShot(jumpscareSound);
        }

        Debug.Log("Jumpscare!");

        navMeshAgent.ResetPath();  // Stop the enemy's movement
        yield return new WaitForSeconds(jumpscareDelay);

        // Trigger the game over screen after the jumpscare delay
        GameOver.Instance.TriggerGameOver();
        TriggerGameOver();
        StartCoroutine(DisableAfterDelay(2f));  // Delay before deactivating the enemy
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    bool IsVisibleToPlayer()
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("Camera.main is null!");
            return false;
        }

        // Calculate the vector from the player to the enemy
        Vector3 toEnemy = transform.position - Camera.main.transform.position;
        float angle = Vector3.Angle(Camera.main.transform.forward, toEnemy);

        // If the enemy is outside the camera's field of view, it's not visible
        if (angle > 85f) return false;

        // Cast a ray from the camera to the enemy
        Ray ray = new Ray(Camera.main.transform.position, toEnemy.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, toEnemy.magnitude))
        {
            return hit.transform == transform;  // The ray should hit the enemy itself
        }

        return false;  // No obstacles between the player and the enemy
    }

    void TriggerGameOver()
    {
        GameOver.Instance.TriggerGameOver();
    }

    void StopChase()
    {
        // Stop the enemy from chasing the player if it is within sight
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }
    }
}
