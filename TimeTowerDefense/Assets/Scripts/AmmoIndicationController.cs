using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoIndicationController : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    public void Set(float ratio) {
        mask.transform.localPosition = new Vector3(2 * ratio - 2, 0, 0);
    }
}
