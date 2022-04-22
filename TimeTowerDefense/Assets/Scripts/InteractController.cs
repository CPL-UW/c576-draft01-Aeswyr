using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent enterAction;
    [SerializeField] private UnityEvent exitAction;
    [SerializeField] private UnityEvent stayAction;
    [SerializeField] private bool triggersHint;

    public void OnInteract() {
        if (action != null)
            action.Invoke();
    }

    public bool CanInteract() {
        return action != null;
    }

    public bool TriggersHint() {
        return triggersHint;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (enterAction != null)
            enterAction.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (stayAction != null)
            stayAction.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (exitAction != null)
            exitAction.Invoke();
    }
}
