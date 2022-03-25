using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private GameObject fxSource;
    [SerializeField] private AmmoIndicationController ammo;
    [SerializeField] private GameObject beamPrefab;
    int shots = 3;
    int MAX_SHOTS = 3;
    float nextFire;
    [SerializeField] private float range = 5;
    public Vector3 GetSourcePos() {
        return fxSource.transform.position;
    }

    void FixedUpdate() {
        EnemyController target;
        if (GameController.Instance.tickDiff == 0 && Time.time > nextFire && shots > 0 && (target = NextTarget()) != null) {
            Fire(target);
            nextFire = Time.time + 3;
        }
    }
    public void Fire(EnemyController target) {
        target.Die();
        shots--;
        ammo.Set((float)shots / MAX_SHOTS);
        GameObject beam = Instantiate(beamPrefab);
        LineRenderer line = beam.GetComponent<LineRenderer>();
        line.SetPositions(new Vector3[] {this.transform.position, target.transform.position});
    }

    public void Reload() {
        if (!GameController.Instance.TrySpendAmmo(1) || shots == MAX_SHOTS)
            return;
        shots = MAX_SHOTS;
        ammo.Set((float)shots / MAX_SHOTS);
    }

    private EnemyController NextTarget() {
        EnemyController ctrl = null;
        foreach (var enemy in GameController.Instance.GetEnemies()) {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= range)
                ctrl = enemy;
        }
        return ctrl;
    }
}
