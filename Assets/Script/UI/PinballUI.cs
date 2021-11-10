using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinballUI : MonoBehaviour
{
    [SerializeField] private Text scoreDisplayer;
    [SerializeField] private Text ballText;
    
    public string ScoreDisplayer
    {
        set => scoreDisplayer.text = value;
    }

    public string BallText
    {
        set => ballText.text = value;
    }
}
