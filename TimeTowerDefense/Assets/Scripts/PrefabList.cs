using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PrefabList", menuName = "TimeTowerDefense/PrefabList", order = 0)]
public class PrefabList : ScriptableObject {
    [SerializeField] private ObjectStringPair[] prefabs;

    public GameObject Get(string name) {
        foreach (var pair in prefabs) {
            if (name.Equals(pair.name))
                return pair.obj;
        }
        return null;
    }

    public GameObject Get(int index) {
        if (index >= prefabs.Length)
            return null;
        return prefabs[index].obj;
    }
}

[Serializable] struct ObjectStringPair {
    public string name;
    public GameObject obj;
} 