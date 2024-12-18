using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
    private NavMeshAgent agent;
    private bool isMoving = false;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 destination, System.Action onComplete = null) {
        if (agent == null) {
            Debug.LogError("No NavMeshAgent found on this object.");
            return;
        }

        agent.SetDestination(destination);
        isMoving = true;

        StartCoroutine(CheckMovementCompletion(onComplete));
    }

    private IEnumerator CheckMovementCompletion(Action onComplete) {
        while (isMoving) {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
                isMoving = false;
                onComplete?.Invoke();
            }
            yield return null;
        }
    }

    public void CancelMovement() {
        if (agent != null) {
            agent.ResetPath();
            isMoving = false;
        }
    }
}
