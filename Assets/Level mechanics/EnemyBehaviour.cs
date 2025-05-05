using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using Unity.AI.Navigation;
public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3f;
    public float detectionDistance = 30f;
    private bool isTemporary = false;
    private float visibleTimer = 3f;
    private bool hasBeenSeen = false;

    private EnvironmentChanger stinkJuice;
    private bool isChasing = false;

    void Start()
    {
        stinkJuice = Object.FindFirstObjectByType<EnvironmentChanger>();

        if (player == null)

        {
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        if (isTemporary && !hasBeenSeen && IsVisibleToPlayer())
        {
            hasBeenSeen = true;
            StartCoroutine(DisappearAfterDelay());
        }

        if (!isTemporary)
        {
            ChasePlayer();
        }
        if (stinkJuice == null) return;

        int currentLoop = stinkJuice.loopCount;

        if (currentLoop < 3)
        {
            IdleBehaviour();
        }
        else if (currentLoop >= 5)
        {
            ChasePlayer();
        }
    }

    void IdleBehaviour()
    {
        isChasing = false;
    }

    void ChasePlayer()
    {
        isTemporary = false;
        isChasing = true;

        float step = chaseSpeed * Time.deltaTime;
        transform.LookAt(player); 
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }
    public void SetupTemporary(bool temporary)
    {
        isTemporary = temporary;
        hasBeenSeen = false;
    }

    IEnumerator DisappearAfterDelay()
    {
        yield return new WaitForSeconds(visibleTimer);
        gameObject.SetActive(false);
    }
    bool IsVisibleToPlayer()
    {
        if (Camera.main == null) return false;

        Vector3 toEnemy = transform.position - Camera.main.transform.position;
        float angle = Vector3.Angle(Camera.main.transform.forward, toEnemy);

        // Field of view check (e.g. 60° cone)
        if (angle > 60f) return false;

        // Raycast to ensure there's no wall blocking line of sight
        Ray ray = new Ray(Camera.main.transform.position, toEnemy.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, toEnemy.magnitude))
        {
            return hit.transform == transform;
        }

        return true;
    }
}

