using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "SpawnOrder", menuName = "TimeTowerDefense/SpawnOrder", order = 0)]
public class SpawnOrder : ScriptableObject {
    [SerializeField] private List<SpawnData> data;

    public List<SpawnData> GetSorted() {
        List<SpawnData> list = new List<SpawnData>();
        foreach (var dat in data) {
            if (list.Count == 0) {
                list.Add(dat);
            } else {
                int index = 0;
                while (index < list.Count && dat.spawnTime >= list[index].spawnTime)
                    index++;
                if (index >= list.Count)
                    list.Add(dat);
                else
                    list.Insert(index, dat);
            }
        }
        return list;
    }


}

[Serializable] public struct SpawnData {
    public EnemyType type;
    public float spawnTime;
    public CarryType carry;
    public int id;
    public string data;
    
}

public enum EnemyType {
    DEFAULT, BASIC, TIME
}
public enum CarryType {
    NONE, AMMO, PARTS, 
}
