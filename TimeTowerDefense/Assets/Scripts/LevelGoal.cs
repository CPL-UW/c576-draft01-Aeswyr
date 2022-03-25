using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    bool started = false;
    int hp = 3;
    private void OnTriggerEnter2D(Collider2D other) {
        hp--;
        other.transform.parent.gameObject.GetComponent<EnemyController>().Remove();
        if (hp <= 0) {
            GameController.Instance.lossFlag = true;
        }
    }

    public void StartLevel() {
        if (started)
            return;
        GameController.Instance.StartLevel();
        started = true;
    }
}
