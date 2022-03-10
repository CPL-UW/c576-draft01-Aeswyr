using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Grid levelGrid;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject levelObjectParent;
    [SerializeField] private GameObject levelGoal;
    [SerializeField] private GameObject clockCanvas;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private ModeHandler playerMode;
    [SerializeField] private ClockController clockController;
    [SerializeField] private TextMeshProUGUI gameStateText;
    public bool lossFlag = false;
    
    private Mode gamemode;
    public Mode Gamemode {
        get {return gamemode;}
        set {
            if (gamemode == Mode.TIME && value != Mode.TIME)
                DeactivateTime();
            else if (value == Mode.TIME && gamemode != Mode.TIME)
                ActivateTime();
            gamemode = value;
        }
    }

    private int ammo, parts;

    [SerializeField] private SpawnOrder spawnOrder;
    [SerializeField] private GameObject enemyPrefab;
    private List<SpawnData> toSpawn;
    private List<EnemyController> spawned = new List<EnemyController>();
    private float timeMod;

    public long tickDiff {
        get;
        private set;
    }
    int id = 1;
    void Start() {
        toSpawn = spawnOrder.GetSorted();
        AddParts(1);
    }

    void FixedUpdate() {
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

        if (toSpawn.Count == 0 && spawned.Count == 0 && !lossFlag) {
            gameStateText.gameObject.SetActive(true);
            gameStateText.text = "Win!!";
        } else if (lossFlag) {
            gameStateText.gameObject.SetActive(true);
            gameStateText.text = "Lose...";
        }
    }

    public Grid GetLevelGrid() {
        return levelGrid;
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

    public bool TrySpendAmmo(int count) {
        if (count > ammo)
            return false;
        ammo -= count;
        HUDController.Instance.DisplayAmmo(ammo);
        return true;
    }

    public bool TrySpendParts(int count) {
        if (count > parts)
            return false;
        parts -= count;
        HUDController.Instance.DisplayParts(parts);
        return true;
    }

    public void AddAmmo(int count) {
        ammo += count;
        HUDController.Instance.DisplayAmmo(ammo);
    }

    public void AddParts(int count) {
        parts += count;
        HUDController.Instance.DisplayParts(parts);
    }

    public void ActivateTime() {
        clockCanvas.SetActive(true);
        hudCanvas.SetActive(false);
        Time.timeScale = 0;
        clockController.OpenClock();
    }
    public void DeactivateTime() {
        clockCanvas.SetActive(false);
        hudCanvas.SetActive(true);
        Time.timeScale = 1;
    }

    public void SyncPlayerMode() {
        playerMode.SyncMode();
    }

    public void StartRollback(long newTick) {
        tickDiff = HUDController.Instance.ForceTickDiff(newTick);
        timeMod += tickDiff * Time.fixedDeltaTime * 2;
    }

}

public enum Mode {
    DEFAULT, PLACE, MOVE, TIME, 
}
