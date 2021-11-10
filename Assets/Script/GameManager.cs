using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;
    
    public readonly float Gravity = 9.81f;
    
    public Ball Ball { get; private set; }
    private int BallRemaining;
    
    private float TLeft;
    private float TRight;
    
    [HideInInspector]
    public float[] flipPower = new float[2];

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
        
        FlipperInput();
        
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
        Ball = Instantiate(ballObject, ballSpawner.position, Quaternion.identity).GetComponent<Ball>();
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
 