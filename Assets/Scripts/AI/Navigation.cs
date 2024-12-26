using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*public class Navigation : MonoBehaviour {
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
}*/


public class Navigation : MonoBehaviour {
    private NavMeshAgent agent;
    private bool isMoving = false;

    // Separation settings
    public float separationDistance = 2f; // Minimum distance to maintain
    public float separationForce = 2f;   // Force applied for separation
    public List<NavMeshAgent> nearbyAgents = new List<NavMeshAgent>();

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (isMoving) {
            // Apply separation steering to prevent overlapping
            ApplySeparation();

            // Adjust radius dynamically
            agent.radius = Mathf.Lerp(agent.radius, 0.3f, Time.deltaTime * 5f); // Shrink while moving
        } else {
            agent.radius = Mathf.Lerp(agent.radius, 1f, Time.deltaTime * 5f); // Expand when stationary
        }
    }

    public void MoveTo(Vector3 destination, Action onComplete = null) {
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

    private void ApplySeparation() {
        if (nearbyAgents.Count == 0) return;

        Vector3 separationVector = Vector3.zero;

        foreach (var other in nearbyAgents) {
            if (other == null || other == agent) continue;

            float distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance < separationDistance) {
                separationVector += transform.position - other.transform.position;
            }
        }

        if (separationVector != Vector3.zero) {
            agent.velocity += separationVector.normalized * separationForce * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        var otherAgent = other.GetComponent<NavMeshAgent>();
        if (otherAgent != null && otherAgent != agent && !nearbyAgents.Contains(otherAgent)) {
            nearbyAgents.Add(otherAgent);
        }
    }

    private void OnTriggerExit(Collider other) {
        var otherAgent = other.GetComponent<NavMeshAgent>();
        if (otherAgent != null && nearbyAgents.Contains(otherAgent)) {
            nearbyAgents.Remove(otherAgent);
        }
    }
}


