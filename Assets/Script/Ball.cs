using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball : MonoBehaviour
{
    public static Ball Instance;
    
    private Transform BallTransform;
    
    private float BallRadius;
    [SerializeField]
    private float ForceDelta = 0.5f;
    
    private int FramesCut = 1000;
    private int Width = 5;
    
    [Header("Entities")]
    [SerializeField]
    private List<Transform> Planes;
    
    [SerializeField]
    private List<Transform> Cylinders;
    
    [SerializeField]
    private List<Transform> Walls;
    
    [SerializeField]
    private Transform[] FlippersCol;
    
    [HideInInspector]
    public Vector3 VectorSpeed;

    private void Awake()
    {
        if (!ReferenceEquals(Instance, null)){
            Destroy(Instance);
        }
        
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BallTransform = transform;
        BallRadius = BallTransform.localScale.x / 2;
        
        VectorSpeed = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        VectorSpeed.y += -GameManager.Instance.Gravity * Time.deltaTime;
        
        PlaneCollision(BallTransform);
        BallTransform.position = WallCollision(BallTransform);
        BallTransform.position = BumperCollision(BallTransform);
        BallTransform.position = FlipperCollision(BallTransform);

        transform.position = BallTransform.position;
        if (VectorSpeed.magnitude*Time.deltaTime > 2*BallRadius)
        {
            //VectorSpeed *=2 * BallRadius / VectorSpeed.magnitude*Time.deltaTime ;
        }
        transform.position += VectorSpeed * Time.deltaTime;
    }

    private void PlaneCollision(Transform ballTransform)
    {
        ForceDelta = 0.5f;
        
        foreach (var plane in Planes)
        {
            Transform planeTrans = plane.transform;
            
            Vector3 normalPlane = planeTrans.up;
            Vector3 vectorPlaneBall = ballTransform.position - planeTrans.position;

            if (!(Mathf.Abs(Vector3.Dot(vectorPlaneBall, normalPlane)) < BallRadius) ||
                !(Mathf.Abs(Vector3.Dot(vectorPlaneBall, planeTrans.right)) < planeTrans.localScale.x * Width) ||
                !(Mathf.Abs(Vector3.Dot(vectorPlaneBall, planeTrans.forward)) < planeTrans.localScale.z * Width)) continue;
            
            Vector3 perpendicular = normalPlane * Vector3.Dot(VectorSpeed, normalPlane);
            Vector3 parallel = VectorSpeed - perpendicular;
                
            VectorSpeed = parallel - perpendicular * ForceDelta;

        }
    }

    private Vector3 WallCollision(Transform ballTransform)
    {
        ForceDelta = 0.9f;
        
        foreach (var wall in Walls)
        {
            Transform wallTrans = wall.transform;

            Vector3 tempBallPos = ballTransform.position;
            Vector3 normalWall = wallTrans.up;
            Vector3 vectorWallBall = ballTransform.position - wallTrans.position;

            if (!(Mathf.Abs(Vector3.Dot(vectorWallBall, normalWall)) < BallRadius) ||
                !(Mathf.Abs(Vector3.Dot(vectorWallBall, wallTrans.right)) < wallTrans.localScale.x * Width) ||
                !(Mathf.Abs(Vector3.Dot(vectorWallBall, wallTrans.forward)) < wallTrans.localScale.z * Width)) continue;
            
            tempBallPos -= VectorSpeed * Time.deltaTime;

            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += i / FramesCut * VectorSpeed * Time.deltaTime;
                vectorWallBall = tempBallPos - wallTrans.position;
                    
                if (Mathf.Abs(Vector3.Dot(vectorWallBall, normalWall)) < BallRadius)
                {
                    break;
                }
            }

            gameObject.transform.position += Mathf.Abs(Vector3.Dot(vectorWallBall, normalWall)) * normalWall;
            Vector3 perpendicular= normalWall * Vector3.Dot(VectorSpeed, normalWall);
            Vector3 parallel = VectorSpeed - perpendicular;
            
            VectorSpeed = parallel - perpendicular * ForceDelta;

            return tempBallPos;
        }

        return ballTransform.position;
    }

    private Vector3 BumperCollision(Transform ballTransform)
    {
        ForceDelta = 1f;
        
        foreach (var bumper in Cylinders)
        {
            Vector3 tempBallPos = ballTransform.position;

            Transform bumperTransform = bumper.transform;
            
            float bumperRadius = bumperTransform.localScale.x / 2;
            Vector3 ballCyllinder = ballTransform.position - bumperTransform.position;
            Vector3 normal = ballCyllinder - (Vector3.Dot(ballCyllinder, bumperTransform.up) * bumperTransform.up);


            float dist = Vector3.Magnitude(normal);
            if (!(dist < BallRadius + bumperRadius)) continue;
            
            tempBallPos -= VectorSpeed * Time.deltaTime;
                
            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += 1 / FramesCut * VectorSpeed * Time.deltaTime;
                ballCyllinder =ballTransform.position - bumperTransform.position;
                normal =  ballCyllinder - (Vector3.Dot(ballCyllinder, bumperTransform.up) * bumperTransform.up);
                    
                dist = Vector3.Magnitude(normal);
                    
                if (dist < BallRadius + bumperRadius)
                {
                    break;
                }
            }
                
            gameObject.transform.position += 10*Mathf.Abs(BallRadius + bumperRadius-dist) * normal.normalized;
            Vector3 perpendicular = normal.normalized * Vector3.Dot(VectorSpeed, normal.normalized);
            Vector3 parallel = VectorSpeed - perpendicular;
                
            VectorSpeed = parallel  - perpendicular  + ForceDelta*normal.normalized ;

            return tempBallPos;
        }

        return ballTransform.position;
    }

    private Vector3 FlipperCollision(Transform ballTransform)
    {
        foreach (var flipper in FlippersCol)
        {
            Transform flipperTransform = flipper.transform;

            float flipperRadiusX = flipperTransform.localScale.x / 2;
            float flipperRadiusY = flipperTransform.localScale.y / 2;
            float flipperRadiusZ = flipperTransform.localScale.z / 2;

            int index = 0;
            if (flipper.gameObject.CompareTag("Left"))
            {
                index = 0;
            }
            else
            {
                index = 1;
            }
            Vector3 tempBallPos = ballTransform.position;
            Vector3 normalFlipper = flipperTransform.up;
            Vector3 vectorFlipperBall = tempBallPos - flipperTransform.position - flipperTransform.up * flipperRadiusY;

            if (!(Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)) < BallRadius * (1+10*Mathf.Abs(GameManager.Instance.flipPower[index]))) ||
                !(Mathf.Abs(Vector3.Dot(vectorFlipperBall, flipperTransform.right)) < flipperRadiusX + BallRadius) ||
                !(Mathf.Abs(Vector3.Dot(vectorFlipperBall, flipperTransform.forward)) < flipperRadiusZ + BallRadius)) continue;
            
            tempBallPos -= VectorSpeed * Time.deltaTime;
                
            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += i / FramesCut * VectorSpeed * Time.deltaTime;
                vectorFlipperBall = tempBallPos - flipperTransform.position - flipperTransform.up * flipperRadiusY;
                
                if (Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)) < BallRadius)
                {
                    break;
                }
            }

            gameObject.transform.position += 6*Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)+100*Mathf.Abs(GameManager.Instance.flipPower[index])) * normalFlipper;
            
            Vector3 perpendicular= normalFlipper * Vector3.Dot(VectorSpeed, normalFlipper);
            Vector3 parallel = VectorSpeed - perpendicular;

            VectorSpeed = parallel - perpendicular 
                          + normalFlipper *Mathf.Max(0, GameManager.Instance.flipPower[index] *500) *(1+0.5f+Vector3.Dot(vectorFlipperBall, flipperTransform.right)/(flipperRadiusX + BallRadius));

            return tempBallPos;
        }

        return ballTransform.position;
    }
}
