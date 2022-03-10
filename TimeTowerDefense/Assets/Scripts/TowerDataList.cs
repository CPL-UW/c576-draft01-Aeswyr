using System;
using UnityEngine;

public class TowerDataList : MonoBehaviour
{
    [SerializeField] private TowerData[] data;

    public TowerData Get(string name) {
        foreach (var tower in data) {
            if (tower.name == name)
                return tower;
        }
        return default;
    }
}

[Serializable] public struct TowerData {
    public string name;
}
