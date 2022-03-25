using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Grid levelGrid;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject levelObjectParent;
    [SerializeField] private GameObject levelGoal;
    [SerializeField] private SpawnOrder spawnOrder;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int startingParts = 1;
    [SerializeField] private int startingAmmo = 0;
    private List<SpawnData> toSpawn;
    private List<EnemyController> spawned = new List<EnemyController>();
    public bool lossFlag = false;
    bool endFlagReady = true;
    private float timeMod;
    private bool started = false;

    public long tickDiff {
        get;
        private set;
    }
    int id = 1;
    // Start is called before the first frame update
    public void StartLevel() {
        toSpawn = spawnOrder.GetSorted();
        timeMod = Time.time;
        started = true;
        GameController.Instance.unpaused = true;
    }

    void Start() {
        GameController.Instance.AddParts(startingParts);
        GameController.Instance.AddAmmo(startingAmmo);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!started)
            return;
        
        if (tickDiff == 0 && toSpawn.Count > 0 && Time.time > toSpawn[0].spawnTime + timeMod) {
            GameObject enemy = Instantiate(enemyPrefab, levelObjectParent.transform);
            enemy.transform.position = spawnPoint.transform.position;
            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.spawnData = toSpawn[0];
            if (controller.spawnData.id == 0) {
                controller.spawnData.id = id;
                id++;
            }
            spawned.Add(controller);
            toSpawn.RemoveAt(0);
        }

        if (tickDiff > 0)
            tickDiff--;

        if (endFlagReady && toSpawn.Count == 0 && spawned.Count == 0 && !lossFlag) {
            GameController.Instance.SetVictoryState(true);
            endFlagReady = false;
        } else if (endFlagReady && lossFlag) {
            GameController.Instance.SetVictoryState(false);
            endFlagReady = false;
        }
    }

    public Grid GetLevelGrid() {
        return levelGrid;
    }

    public GameObject GetLevelObjectParent() {
        return levelObjectParent;
    }

    public Vector3 GetGoal() {
        return levelGoal.transform.position;
    }

    public List<EnemyController> GetEnemies() {
        return spawned;
    }

    public void RemoveEnemy(EnemyController ctrl) {
        spawned.Remove(ctrl);
    }

    public void UnspawnEnemy(EnemyController ctrl) {
        RemoveEnemy(ctrl);
        if (toSpawn.Count == 0) {
            toSpawn.Add(ctrl.spawnData);
        } else {
            int index = 0;
            while (index < toSpawn.Count && ctrl.spawnData.spawnTime >= toSpawn[index].spawnTime)
                index++;
            if (index >= toSpawn.Count)
                toSpawn.Add(ctrl.spawnData);
            else
                toSpawn.Insert(index, ctrl.spawnData);
        }
    }
    public void StartRollback(long newTick) {
        tickDiff = HUDController.Instance.ForceTickDiff(newTick);
        timeMod += tickDiff * Time.fixedDeltaTime * 2;
    }
}
