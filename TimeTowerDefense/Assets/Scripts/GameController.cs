using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameObject clockCanvas;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private ModeHandler playerMode;
    [SerializeField] private ClockController clockController;
    [SerializeField] private TextMeshProUGUI gameStateText;
    private LevelController currentLevel;
    [SerializeField] private PrefabList levelList;
    [SerializeField] private GameObject player;
    int currLvl;
    public bool unpaused = false;
    public long tickDiff {
        get {return currentLevel.tickDiff;}
    }

    public bool lossFlag {
        get {return currentLevel.lossFlag;}
        set {currentLevel.lossFlag = value;}
    }
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

    private void Start() {
        LoadNextLevel();
    }

    public Grid GetLevelGrid() {
        return currentLevel.GetLevelGrid();
    }

    public Vector3 GetGoal() {
        return currentLevel.GetGoal();
    }

    public List<EnemyController> GetEnemies() {
        return currentLevel.GetEnemies();
    }

    public void StartLevel() {
        currentLevel.StartLevel();
    }

    public void RemoveEnemy(EnemyController ctrl) {
        currentLevel.RemoveEnemy(ctrl);
    }

    public void UnspawnEnemy(EnemyController ctrl) {
        currentLevel.UnspawnEnemy(ctrl);
    }

    public void SetVictoryState(bool state) {
        gameStateText.gameObject.SetActive(true);
        if (state) {
            gameStateText.text = "Win!!";
            PlayerPrefs.SetInt("FurthestLevel", currLvl + 1);
            if (levelList.Get(currLvl + 1) != null)
                StartCoroutine(DoLevelTransition());
        }
        else
            gameStateText.text = "Lose...";
    }

    public GameObject GetLevelObjectParent() {
        return currentLevel.GetLevelObjectParent();
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
        currentLevel.StartRollback(newTick);
    }

    public void LoadNextLevel() {
        gameStateText.gameObject.SetActive(false);
        HUDController.Instance.ForceTick(0);
        currLvl = PlayerPrefs.GetInt("FurthestLevel");
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        currentLevel = Instantiate(levelList.Get(currLvl)).GetComponent<LevelController>();
        player.transform.position = GetGoal();
        unpaused = false;
    }

    public IEnumerator DoLevelTransition() {
        yield return new WaitForSeconds(3f);

        LoadNextLevel();
    }

}

public enum Mode {
    DEFAULT, PLACE, MOVE, TIME, 
}
