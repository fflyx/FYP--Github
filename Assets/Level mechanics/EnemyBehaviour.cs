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
        Debug.Log("Enemy Update running - Mode: " + mode);

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
                    ChasePlayer();

                    float distance = Vector3.Distance(transform.position, player.position);
                    if (distance <= jumpscareDistance && !hasScared)
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
        if (IsVisibleToPlayer())
        {
            StopChase();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to Player: " + distance);  // Log distance to see if the enemy is getting close enough

        // If the distance is within the jumpscare range and the enemy is not yet seen
        if (distance <= jumpscareDistance && !hasScared)
        {
            Jumpscare();
        }

        if (navMeshAgent != null && Camera.main != null)
        {
            navMeshAgent.SetDestination(Camera.main.transform.position);
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

        navMeshAgent.ResetPath();
        yield return new WaitForSeconds(jumpscareDelay);

        GameOver.Instance.TriggerGameOver();
        TriggerGameOver();
        StartCoroutine(DisableAfterDelay(2f));
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
        Vector3 toEnemy = transform.position - Camera.main.transform.position;
        float angle = Vector3.Angle(Camera.main.transform.forward, toEnemy);

       
        if (angle > 85f) return false;

        Ray ray = new Ray(Camera.main.transform.position, toEnemy.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, toEnemy.magnitude))
        {
            return hit.transform == transform;
        }

        return true;

    }

    void TriggerGameOver()
    {
        GameOver.Instance.TriggerGameOver();
    }
    void StopChase()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
        }
    }
}