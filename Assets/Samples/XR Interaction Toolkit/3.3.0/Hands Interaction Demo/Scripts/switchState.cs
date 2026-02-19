using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PaperStatesToggleOnGrab : MonoBehaviour
{
    [Header("Objets d�j� dans la sc�ne (Hierarchy)")]
    public GameObject[] states; // 0=normal, 1=froiss�1, 2=froiss�2, 3=froiss�3...

    [Header("Options")]
    public int startIndex = 0;
    public bool advanceOneStepPerGrab = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private int index;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab == null)
        {
            Debug.LogError("XRGrabInteractable manquant sur cet objet.");
            enabled = false;
            return;
        }

        index = Mathf.Clamp(startIndex, 0, states.Length - 1);
        ShowOnly(index);

        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDestroy()
    {
        if (grab != null)
            grab.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!advanceOneStepPerGrab) return;

        int next = Mathf.Min(index + 1, states.Length - 1);
        if (next != index)
        {
            index = next;
            ShowOnly(index);
        }
    }

    private void ShowOnly(int activeIndex)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] != null)
                states[i].SetActive(i == activeIndex);
        }
    }
}
