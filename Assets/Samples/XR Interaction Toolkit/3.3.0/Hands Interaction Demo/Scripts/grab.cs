using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PaperGrabAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        animator = GetComponent<Animator>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        animator.SetBool("isGrabbed", true);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        animator.SetBool("isGrabbed", false);
    }
}
