using System.Collections;
using UnityEngine;

public class EnemyTurn : MonoBehaviour {


    public IEnumerator StartEnemyTurn(GameObject[] enemies)
    {
        Debug.Log("Enemy Turn: Starting Enemy Turn");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log($"Enemy Turn: Enemy {enemy.name} is attacking");
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        Debug.Log("Enemy Turn: Ending Enemy Turn");
    }
}
