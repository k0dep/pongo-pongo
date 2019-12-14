using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadService : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
