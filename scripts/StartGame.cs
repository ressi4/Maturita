using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneManager.LoadScene("GameScene"); 
    }
}
