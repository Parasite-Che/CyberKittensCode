using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

    public void SceneLoading(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
        if ((int)Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    
    public void DeactivatePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void HideCursor()
    {
        //Cursor.visible = false;
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
