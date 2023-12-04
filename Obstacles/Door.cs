using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, ILock
{
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOpen;
    [SerializeField] SpriteRenderer doorVisual;
    [SerializeField] BoxCollider2D doorBoxCollider;
    [SerializeField] private float moveDistance;

    public void Interact(GameObject player)
    {
        if (isLocked) return;
        if (!isOpen)
        {
            doorBoxCollider.enabled = false;
            doorVisual.enabled = false;
            isOpen = true;
        }
        else
        {
            doorBoxCollider.enabled = true;
            doorVisual.enabled = true;
            isOpen = false;
        }
    }


    public void Unlock()
    {
        isLocked = false;
    }

    public void Lock()
    {
        isLocked = true;
    }
}
