using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    int hp = 3;
    private void OnTriggerEnter2D(Collider2D other) {
        hp--;
        other.transform.parent.gameObject.GetComponent<EnemyController>().Remove();
        if (hp <= 0) {
            GameController.Instance.lossFlag = true;
        }
    }
}
