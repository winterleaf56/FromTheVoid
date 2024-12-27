using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleSystemController : MonoBehaviour {
    private new ParticleSystem particleSystem;

    private void OnEnable() {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Clear(); // Stops the particle system
        particleSystem?.Play();
    }

    private void Start() {
         // Starts the particle system
    }
}
