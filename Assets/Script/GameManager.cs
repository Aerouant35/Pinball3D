using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;
    
    public readonly float Gravity = 9.81f;
    
    public Collision Ball { get; private set; }
    private int BallRemaining;

    // [SerializeField] private Text _startText;
    // [SerializeField] private Text _spaceText;
    
    [Header("Ball")]
    [SerializeField] private int ballMax;

    // [SerializeField] private Text _ballText;
    
    [SerializeField] private GameObject ballObject;
    [SerializeField] private Transform ballSpawner;
    
    [Header("Flippers")]
    [SerializeField] private Transform flipperLeft;
    [SerializeField] private Transform flipperRight;

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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && ReferenceEquals(Ball, null))
        {
            InitGame();
        }
        
        // flipperLeft.eulerAngles = new Vector3(flipperLeft.eulerAngles.x, flipperLeft.eulerAngles.y, Input.GetKey(KeyCode.Q) || 
        //     Input.GetKey(KeyCode.LeftArrow) ? flippersAngleMax[0] : flippersAngleOrigin[0]);
        //
        // flipperRight.eulerAngles = new Vector3(flipperRight.eulerAngles.x, flipperRight.eulerAngles.y, Input.GetKey(KeyCode.D) || 
        //     Input.GetKey(KeyCode.RightArrow) ? flippersAngleMax[1] : flippersAngleOrigin[1]);

        if (ReferenceEquals(Ball, null) && BallRemaining <= 0 ) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (_spaceText.enabled) _spaceText.enabled = false;
            //Ball.BallStart(_ballSpeedY);
        }
    }
    
    void InitGame()
    {
        BallRemaining = ballMax;
        //ScoreManager.Instance.ScoreDisplayer.text = "0";
        
        SpawnBall();
    }

    void SpawnBall()
    {
        Ball = Instantiate(ballObject, ballSpawner.position, Quaternion.identity).GetComponent<Collision>();
    }

    void EndGame()
    {
        Ball = null;
        //_ballText.text = "0";
        //ScoreManager.Instance.SaveHighScore();
    }

    public void MinusBall(int ball)
    {
        BallRemaining -= ball;
        //_ballText.text = ballRemaining.ToString();

        DestroyImmediate(Ball.gameObject);

        if (BallRemaining <= 0) EndGame();
        else SpawnBall();
    }
}
