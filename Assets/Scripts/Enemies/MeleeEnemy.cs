using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float actionPoints { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()  {
        Health health = gameObject.AddComponent<Health>();
        health.InitializeHealth(100, 100);
        //damage = 10;
        actionPoints = 100;
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack() {
        Debug.Log("Melee enemy attacking");
    }
}
