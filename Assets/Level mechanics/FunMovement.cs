using UnityEngine;
using UnityEngine.XR;

public class FunMovement : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform headTransform;

    public float moveSpeed = 1.5f;
    public float sensitivity = 1f;
    public float movementThreshold = 0.01f;

    private Vector3 previousLeftPos;
    private Vector3 previousRightPos;

    void Start()
    {
        if (leftHand == null || rightHand == null || headTransform == null)
        {
            Debug.LogError("Hand or Head transform not assigned!");
            enabled = false;
            return;
        }

        previousLeftPos = leftHand.position;
        previousRightPos = rightHand.position;
    }

    void Update()
    {
        Vector3 leftVelocity = (leftHand.position - previousLeftPos) / Time.deltaTime;
        Vector3 rightVelocity = (rightHand.position - previousRightPos) / Time.deltaTime;

        float averageVelocity = (leftVelocity.z + rightVelocity.z) * 0.5f;

        Debug.Log($"Left Z: {leftVelocity.z}, Right Z: {rightVelocity.z}, Avg: {averageVelocity}");

        if (Mathf.Abs(averageVelocity) > movementThreshold)
        {
            Vector3 moveDirection = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
            transform.position += moveDirection * averageVelocity * moveSpeed * sensitivity * Time.deltaTime;

            Debug.Log("Moving in direction: " + moveDirection);
        }

        previousLeftPos = leftHand.position;
        previousRightPos = rightHand.position;
    }
}
