using UnityEngine;
using UnityEngine.EventSystems;

public class Friendly : Lifeforms {
    public float actionPoints { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //Health health = new Health(100, 100);
        Health health = gameObject.AddComponent<Health>();
        health.InitializeHealth(100, 100);
        damage = 25;
        actionPoints = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public override void OnMouseDown() {
        Debug.Log("This is a friendly unit!");
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

    public void PerformMove(BasicMove move) {
        move.Execute(this);
    }

    public void PerformMove(SpecialMove move) {
        move.Execute(this);
    }

    public void PerformMove(UltimateMove move) {
        move.Execute(this);
    }
}
