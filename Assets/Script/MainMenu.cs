using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private GameObject menuCanvas;
    
    [SerializeField] 
    private GameObject scoreCanvas;

    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.SetActive(true);
        scoreCanvas.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Score()
    {
        menuCanvas.SetActive(false);
        scoreCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

    public void BackToMainMenu()
    {
        menuCanvas.SetActive(true);
        scoreCanvas.SetActive(false);
    }
}
