using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    [SerializeField] private UnityEvent action;

    public void OnInteract() {
        action.Invoke();
    }
}
