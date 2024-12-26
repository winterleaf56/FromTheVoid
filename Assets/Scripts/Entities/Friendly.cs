using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Friendly : Lifeforms {
    //public float actionPoints { get; private set; }

/*    [Header("Moves")]
    protected BasicMove basicMove;
    protected SpecialMove specialMove;
    protected UltimateMove ultimateMove;

    [Header("Actions")]
    protected RecoverAction recoverAction;
    protected RepositionAction repositionAction;*/

    [SerializeField] private int friendlyUnitID;

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

    private void Awake() {
        stats.InitializeStats(unitStats.UnitName, unitStats.ActionPoints, unitStats.ActionPointRecovery, unitStats.UltimatePoints, unitStats.MaxUltimatePoints, unitStats.MaxHealth, unitStats.MaxMoveDistance, unitStats.Defence);

        health = gameObject.AddComponent<Health>();
        health.InitializeHealth(stats.MaxHealth, stats.MaxHealth);

        SetMoves();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public override void Attack() {
        throw new System.NotImplementedException();
    }

    public override void Die() {
        throw new System.NotImplementedException();
    }

    public override void Damage(float value) {
        throw new System.NotImplementedException();
    }
}
