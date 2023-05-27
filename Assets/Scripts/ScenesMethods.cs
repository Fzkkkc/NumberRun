using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesMethods : MonoBehaviour
{
    private ScenesMethods _scenesMethods;
    
    public void Initialize(ScenesMethods controller)
    {
        _scenesMethods = controller;
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
}
