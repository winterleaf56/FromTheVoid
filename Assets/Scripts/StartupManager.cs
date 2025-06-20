using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour {

    void Start() {
        SceneManager.LoadScene("Managers", LoadSceneMode.Single);

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

}
