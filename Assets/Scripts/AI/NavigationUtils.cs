using UnityEngine;
using UnityEngine.AI;

public static class NavigationUtils {
    /// <summary>
    /// Calculates the total distance of a path from the NavMeshAgent to a target position.
    /// </summary>
    public static float CalculatePathDistance(NavMeshAgent agent, Vector3 targetPosition) {
        if (agent == null) {
            Debug.LogError("NavMeshAgent is null.");
            return Mathf.Infinity;
        }

        // Create a new NavMeshPath to store the path
        NavMeshPath path = new NavMeshPath();

        // Calculate the path to the target position
        if (agent.CalculatePath(targetPosition, path)) {
            // Sum the distances between each corner of the path
            float totalDistance = 0f;
            for (int i = 1; i < path.corners.Length; i++) {
                totalDistance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
            return totalDistance;
        }

        // If the path is invalid, return infinity
        return Mathf.Infinity;
    }

    /// <summary>
    /// Finds the farthest position along a path within a specified range.
    /// </summary>
    public static Vector3 GetPositionWithinRange(NavMeshPath path, float maxDistance) {
        float currentDistance = 0;
        for (int i = 1; i < path.corners.Length; i++) {
            float segmentDistance = Vector3.Distance(path.corners[i - 1], path.corners[i]);
            if (currentDistance + segmentDistance > maxDistance) {
                float remainingDistance = maxDistance - currentDistance;
                return Vector3.Lerp(path.corners[i - 1], path.corners[i], remainingDistance / segmentDistance);
            }
            currentDistance += segmentDistance;
        }
        return path.corners[path.corners.Length - 1]; // Return the last position if fully traversable
    }

}