using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "Database/UnitDatabase")]
public class UnitDatabase : ScriptableObject {
    //public List<Friendly> unitPrefabs;
    public List<GameObject> unitPrefabs;

    public GameObject GetUnitPrefabByID(int id) {
        return unitPrefabs.Find(u => u.GetComponent<Friendly>().FriendlyUnitID == id);
    }

    public GameObject GetUnitPrefabByName(string name) {
        return unitPrefabs.Find(u => u.GetComponent<Friendly>().UnitStats.UnitName == name);
    }
}
