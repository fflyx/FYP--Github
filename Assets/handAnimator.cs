using UnityEngine;
using UnityEngine.InputSystem;

public class handAnimator : MonoBehaviour
{
    [SerializeField] private InputActionProperty trigAction;
    [SerializeField] private InputActionProperty gripAction;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        float trigValue = trigAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();

        anim.SetFloat("Trigger", trigValue);
        anim.SetFloat("Grip", gripValue);
    }
}
