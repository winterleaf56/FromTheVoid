using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float value) {
        Debug.Log("Enemy got " + value + " damage!");
    }
}
