using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private bool levelCompleted = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "»õÎï" && !levelCompleted)
        {
            levelCompleted = true;
            Global.StartButtonOn =false;
            Invoke("FinishLevel", 0.1f);
        }
    }
    private void FinishLevel()
    {
        SceneManager.LoadScene("Chooselevel");
    }
}
