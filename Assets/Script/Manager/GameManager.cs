using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;
    
    public const float Gravity = 9.81f;
    
    public Ball Ball { get; private set; }
    private int BallRemaining;
    
    private float TLeft;
    private float TRight;
    
    [HideInInspector]
    public float[] flipPower = new float[2];
    
    [Header("UI")]
    [SerializeField] private Text scoreDisplayer;
    public string ScoreDisplayer
    {
        set => scoreDisplayer.text = value;
    }

    [SerializeField] private Text ballText;

    [Header("Ball")]
    [SerializeField] private int ballMax;

    [SerializeField] private GameObject ballObject;
    [SerializeField] private Transform ballSpawner;
    
    [Header("Flippers")]
    [SerializeField] private Transform flipperLeft;
    [SerializeField] private Transform flipperRight;
    
    [SerializeField]
    private float speedRotation = 5;
    
    [SerializeField]
    private float angleFlippers = 35;

    // [SerializeField] private int[] flippersAngleOrigin = new int[2];
    // [SerializeField] private int[] flippersAngleMax = new int[2];
    #endregion

    private void Awake()
    {
        if (!ReferenceEquals(Instance, null))
            DestroyImmediate(Instance);
        else
            Instance = this;
        
    }

    private void Start()
    {
        if (ReferenceEquals(Ball, null))
            InitGame();        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #endif
                        Application.Quit();
        }
        
        FlipperInput();
        
        // flipperLeft.eulerAngles = new Vector3(flipperLeft.eulerAngles.x, flipperLeft.eulerAngles.y, Input.GetKey(KeyCode.Q) || 
        //     Input.GetKey(KeyCode.LeftArrow) ? flippersAngleMax[0] : flippersAngleOrigin[0]);
        //
        // flipperRight.eulerAngles = new Vector3(flipperRight.eulerAngles.x, flipperRight.eulerAngles.y, Input.GetKey(KeyCode.D) || 
        //     Input.GetKey(KeyCode.RightArrow) ? flippersAngleMax[1] : flippersAngleOrigin[1]);

        if (ReferenceEquals(Ball, null) && BallRemaining <= 0 ) return;
    }
    
    void InitGame()
    {
        BallRemaining = ballMax;
        ballText.text = BallRemaining.ToString();
        
        scoreDisplayer.text = "0";
        
        SpawnBall();
    }

    void SpawnBall()
    {
        Ball = Instantiate(ballObject, ballSpawner.position, Quaternion.identity).GetComponent<Ball>();
    }

    void EndGame()
    {
        Ball = null;
        ballText.text = "0";
        ScoreManager.Instance.SaveHighScore();
    }

    public void MinusBall(int ball)
    {
        BallRemaining -= ball;
        ballText.text = BallRemaining.ToString();

        DestroyImmediate(Ball.gameObject);

        if (BallRemaining <= 0) EndGame();
        else SpawnBall();
    }
    
    private void FlipperInput()
    {
        float tempTLeft;
        float tempTRight;

        if (Input.GetButton("FlipperLeft"))
        {
            tempTLeft = TLeft;
            TLeft += speedRotation * Time.deltaTime;
            TLeft = Mathf.Min(1, TLeft);
            flipPower[0] = TLeft - tempTLeft;
        }
        else
        {
            tempTLeft = TLeft;
            TLeft -= speedRotation * Time.deltaTime;
            TLeft = Mathf.Max(0, TLeft);
            flipPower[0] = TLeft - tempTLeft;
        }
        
        if (Input.GetButton("FlipperRight"))
        {
            tempTRight = TRight;
            TRight += speedRotation * Time.deltaTime;
            TRight = Mathf.Min(1, TRight);
            flipPower[1] = TRight - tempTRight;
        }
        else
        {
            tempTRight = TRight;
            TRight -= speedRotation * Time.deltaTime;
            TRight = Mathf.Max(0, TRight);
            flipPower[1] = TRight - tempTRight;
        }
        Debug.Log(flipPower[0]);
        Debug.Log(flipPower[1]);
        flipperLeft.localEulerAngles = new Vector3(flipperLeft.localEulerAngles.x, Mathf.Lerp(angleFlippers, -angleFlippers,TLeft), flipperLeft.localEulerAngles.z);
        flipperRight.localEulerAngles = new Vector3(flipperRight.localEulerAngles.x, Mathf.Lerp(-angleFlippers, angleFlippers, TRight), flipperRight.localEulerAngles.z);
    }
}
 