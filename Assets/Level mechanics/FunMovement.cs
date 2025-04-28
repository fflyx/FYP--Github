using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class SmoothArmSwingMovement : MonoBehaviour
{
    public Transform headTransform;
    public float moveSpeed = 1.5f;
    public float sensitivity = 1f;
    public float movementThreshold = 0.01f;
    public float smoothingFactor = 0.1f; 

    private XRHandSubsystem handSubsystem;
    private Vector3 previousLeftPalm;
    private Vector3 previousRightPalm;
    private bool hasPreviousData = false;

    private float smoothedSwingIntensity = 0f;

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
        else
        {
            Debug.LogError("XRHandSubsystem not found.");
        }
    }

    void Update()
    {
        if (handSubsystem == null || !handSubsystem.running)
            return;

        XRHand leftHand = handSubsystem.leftHand;
        XRHand rightHand = handSubsystem.rightHand;

        if (!leftHand.isTracked || !rightHand.isTracked)
        {
            hasPreviousData = false;
            smoothedSwingIntensity = Mathf.Lerp(smoothedSwingIntensity, 0, Time.deltaTime * 5f); 
            return;
        }

        XRHandJoint leftPalmJoint = leftHand.GetJoint(XRHandJointID.Palm);
        XRHandJoint rightPalmJoint = rightHand.GetJoint(XRHandJointID.Palm);

        if (!leftPalmJoint.TryGetPose(out Pose leftPalmPose) || !rightPalmJoint.TryGetPose(out Pose rightPalmPose))
            return;

        Vector3 leftPalm = leftPalmPose.position;
        Vector3 rightPalm = rightPalmPose.position;

        if (!hasPreviousData)
        {
            previousLeftPalm = leftPalm;
            previousRightPalm = rightPalm;
            hasPreviousData = true;
            return;
        }

        Vector3 leftDelta = leftPalm - previousLeftPalm;
        Vector3 rightDelta = rightPalm - previousRightPalm;

        float rawSwingIntensity = (leftDelta.magnitude + rightDelta.magnitude) * 0.5f;

        // Smooth the swing
        smoothedSwingIntensity = Mathf.Lerp(smoothedSwingIntensity, rawSwingIntensity, smoothingFactor);

        if (smoothedSwingIntensity > movementThreshold)
        {
            Vector3 moveDirection = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
            transform.position += moveDirection * smoothedSwingIntensity * moveSpeed * sensitivity * Time.deltaTime;
        }

        previousLeftPalm = leftPalm;
        previousRightPalm = rightPalm;

        Debug.Log($"Smoothed Intensity: {smoothedSwingIntensity:F4}");
    }
}
