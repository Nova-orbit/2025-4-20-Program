using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void level1()
    {
        SceneManager.LoadScene("level1");
    }
    public void level2()
    {
        SceneManager.LoadScene("level2");
    }
    public void level3()
    {
        SceneManager.LoadScene("level3");
    }
    public void level4()
    {
        SceneManager.LoadScene("level4");
    }
    public void level5()
    {
        SceneManager.LoadScene("level5");
    }
}
