using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "Database/UnitDatabase")]
public class UnitDatabase : ScriptableObject {
    public List<Friendly> unitPrefabs;

    public Friendly GetUnitPrefabByID(int id) {
        return unitPrefabs.Find(u => u.FriendlyUnitID == id);
    }

    public Friendly GetUnitPrefabByName(string name) {
        return unitPrefabs.Find(u => u.UnitStats.UnitName == name);
    }
}
