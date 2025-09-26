using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Friendly : Lifeforms {
    public enum FriendlyUnitType {
        Assault,
        Sniper,
        Medic
    }

    [SerializeField] private FriendlyUnitType unitType;
    public FriendlyUnitType UnitType => unitType;

    //public float actionPoints { get; private set; }

    /*    [Header("Moves")]
        protected BasicMove basicMove;
        protected SpecialMove specialMove;
        protected UltimateMove ultimateMove;

        [Header("Actions")]
        protected RecoverAction recoverAction;
        protected RepositionAction repositionAction;*/

    [SerializeField] private int friendlyUnitID;
    public int FriendlyUnitID => friendlyUnitID;

    /*[SerializeField] protected ActionBase[] moves;
    [SerializeField] protected ActionBase[] actions;*/

    //[SerializeField] private bool turnFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start() {
        //Health health = new Health(100, 100);
        Health health = gameObject.AddComponent<Health>();
        health.InitializeHealth(100, 100);
        //damage = 25;
        actionPoints = 100;
    }*/

    [SerializeField] private Unit unitStats;

    public Unit UnitStats => unitStats;

    /*[SerializeField] private int duplicates;
    public int Duplicates => duplicates;*/

    private void Awake() {
        stats.InitializeStats(unitStats.UnitName, unitStats.ActionPoints, unitStats.ActionPointRecovery, unitStats.UltimatePoints, unitStats.MaxUltimatePoints, unitStats.MaxHealth, unitStats.MaxMoveDistance, unitStats.Defence, unitStats.Duplicates);

        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);

        SetMoves();
    }

    void Start() {
        //SetMoves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void SetMoves() {
        if (movesDatabase == null || unitStats == null) {
            Debug.LogError($"Moves database or unit stats not set for Friendly unit {unitStats?.UnitName ?? "NULL"}.");
            return;
        }

        Debug.Log($"Setting moves for unit: {unitStats.UnitName}");

        var group = movesDatabase.UnitGroups.Find(g => g.unitName == unitStats.UnitName);

        if (group == null) {
            Debug.LogError($"No move group found for unit: {unitStats.UnitName}");
            return;
        }

        // Assign moves
        foreach (var moveEntry in group.moves) {
            Debug.Log($"Found move: {moveEntry.move.moveName} of type {moveEntry.moveType}");
            switch (moveEntry.moveType) {
                case MoveType.Basic:
                    basicMove = moveEntry.move as BasicMove;
                    break;
                case MoveType.Special:
                    specialMove = moveEntry.move as SpecialMove;
                    break;
            }
        }

        // Assign actions
        recoverAction = group.actions.Find(a => a is RecoverAction) as RecoverAction;
        if (recoverAction == null)
            Debug.LogError($"RecoverAction is NULL for unit: {unitStats.UnitName}");
        else
            Debug.Log($"RecoverAction assigned for unit: {unitStats.UnitName}");

        repositionAction = group.actions.Find(a => a is RepositionAction) as RepositionAction;
        if (repositionAction == null)
            Debug.LogError($"RepositionAction is NULL for unit: {unitStats.UnitName}");
        else
            Debug.Log($"RepositionAction assigned for unit: {unitStats.UnitName}");

        if (basicMove == null) {
            Debug.LogError($"No BasicMove assigned for unit: {unitStats.UnitName}");
        }

        /*if (movesDatabase == null || unitStats == null) {
            Debug.LogError($"Moves database or unit stats not set for Friendly unit {unitStats.UnitName}.");
            return;
        }

        Debug.Log($"Setting moves for unit: {unitStats.UnitName}");

        var group = movesDatabase.UnitGroups.Find(g => g.unitName == unitStats.UnitName);

        if (group == null) {
            Debug.LogError($"No move group found for unit: {unitStats.UnitName}");
            return;
        }

        foreach (var moveEntry in group.moves) {
            Debug.Log($"Found move: {moveEntry.move.moveName} of type {moveEntry.moveType}");
            switch (moveEntry.moveType) {
                case MoveType.Basic:
                    basicMove = moveEntry.move as BasicMove;
                    break;
                case MoveType.Special:
                    specialMove = moveEntry.move as SpecialMove;
                    break;
                    // etc.
            }
        }

        if (basicMove == null) {
            Debug.LogError($"No BasicMove assigned for unit: {unitStats.UnitName}");
        }*/
    }

    /*public override void OnMouseDown() {
        Debug.Log("This is a friendly unit!");
    }*/

    /*protected virtual void SetMoves() {
        // Moves
        basicMove = moves[0] as BasicMove;
        specialMove = moves[1] as SpecialMove;
        ultimateMove = moves[2] as UltimateMove;

        // Actions
        recoverAction = actions[0] as RecoverAction;
        repositionAction = actions[1] as RepositionAction;
    }*/

    public void SetUnitStats(Unit unitStats) {
        this.unitStats = unitStats;
    }

    public void SetMovesDatabase(UnitMovesActionsDatabase movesDatabase) {
        this.movesDatabase = movesDatabase;
    }


    public override void Attack() {
        throw new System.NotImplementedException();
    }

    public override void Die() {
        throw new System.NotImplementedException();
    }

    public override void Damage(float value) {
        throw new System.NotImplementedException();
    }

    /*public void ObtainedDuplicate() {
        duplicates++;
    }*/
}
