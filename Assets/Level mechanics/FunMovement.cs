using UnityEngine;
using UnityEngine.XR;

public class FunMovement : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform headTransform; 
    public float moveSpeed = 1.5f;
    public float sensitivity = 1f;

    private Vector3 previousLeftPos;
    private Vector3 previousRightPos;

    void Start()
    {
        previousLeftPos = leftHand.position;
        previousRightPos = rightHand.position;
    }

    void Update()
    {
        Vector3 leftVelocity = (leftHand.position - previousLeftPos) / Time.deltaTime;
        Vector3 rightVelocity = (rightHand.position - previousRightPos) / Time.deltaTime;

        
        float combinedVelocity = (leftVelocity.z + rightVelocity.z) * 0.5f;

        if (Mathf.Abs(combinedVelocity) > 0.01f) 
        {
            Vector3 moveDirection = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
            transform.position += moveDirection * combinedVelocity * moveSpeed * Time.deltaTime * sensitivity;
        }

        previousLeftPos = leftHand.position;
        previousRightPos = rightHand.position;
    }
}
