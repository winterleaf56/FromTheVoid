using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType {
    Basic,
    Special,
    Ultimate
}

public enum SkillType {
    Main,
    Secondary,
    Awakened
}

[System.Serializable]
public class MoveEntry {
    public MoveType moveType;
    public ActionBase move;
}

[System.Serializable]
public class SkillEntry {
    public SkillType skillType;
    public ActionBase skill;
}

[CreateAssetMenu(fileName = "UnitMovesActionsDatabase", menuName = "Database/Unit Moves + Actions")]
public class UnitMovesActionsDatabase : ScriptableObject {
    [SerializeField]
    private List<UnitMovesActionsGroup> unitGroups = new();

    public List<UnitMovesActionsGroup> UnitGroups => unitGroups;
}

[System.Serializable]
public class UnitMovesActionsGroup {
    public string unitName;
    public List<MoveEntry> moves = new();
    public List<ActionBase> actions = new();
    public List<SkillEntry> skills = new();
}