using UnityEngine;

public class Enemy : Lifeforms {

    public override void Attack() {
        throw new System.NotImplementedException();
    }

    public override void Die() {
        throw new System.NotImplementedException();
    }

    public override void Damage(float value) {
        throw new System.NotImplementedException();
    }

    /*public override void OnMouseDown() {
        Debug.Log("This is an enemy unit!");

        // If current state is the enemy's turn, display the enemy's stats
        if (BattleManager.Instance.currentTurn == BattleManager.GameState.EnemyTurn) {
            Debug.Log("Displaying Enemy Stats");
        } else if (BattleManager.Instance.currentTurn == BattleManager.GameState.PlayerTurn) {
            // If the current state is the player's turn, invoke an action on the enemy (check if an action is selected)
            Debug.Log("Player invoking action on enemy");
        }
    }*/

}
