using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [HideInInspector]
    public PauseGame pausePanel;
    
    [HideInInspector]
    public PinballUI inGamePanel;

    private void Awake()
    {
        if (!ReferenceEquals(Instance, null))
            DestroyImmediate(Instance);

        Instance = this;
        
        pausePanel = FindObjectOfType<PauseGame>();
        inGamePanel = FindObjectOfType<PinballUI>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.gameObject.SetActive(false);
        inGamePanel.gameObject.SetActive(true);
    }
}
