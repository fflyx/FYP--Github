using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
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
        if (navMeshAgent != null && player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
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

        // Test cone: 90° field of view
        if (angle > 90f) return false;

        Ray ray = new Ray(Camera.main.transform.position, toEnemy.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, toEnemy.magnitude))
        {
            return hit.transform == transform;
        }

        return true;

    }
}