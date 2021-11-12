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
        VectorSpeed.y -= 2*GameManager.Gravity * Time.fixedDeltaTime;
        
        PlaneCollision(BallTransform);
        transform.position = WallCollision(BallTransform);
        transform.position = BumperCollision(BallTransform);
        transform.position = FlipperCollision(BallTransform);
        
        if (VectorSpeed.magnitude*Time.fixedDeltaTime > 2*BallRadius)
        {
            VectorSpeed *=2 * BallRadius / VectorSpeed.magnitude*Time.fixedDeltaTime ;
        }
        transform.position += VectorSpeed * Time.fixedDeltaTime;
        
        TriggerBox(BallTransform);
    }

    private void PlaneCollision(Transform ballTransform)
    {
        ForceDelta = 0.5f;
        
        foreach (var plane in EntityManager.Instance.planes)
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

    private void TriggerBox(Transform Ball)
    {
        foreach (var Trigger in EntityManager.Instance.triggerBox)
        {

            Vector3 vectorWallBall = Ball.position - Trigger.position;

            if ((Mathf.Abs(Vector3.Dot(vectorWallBall, Trigger.up)) < Trigger.localScale.y/2 + BallRadius) &&
                (Mathf.Abs(Vector3.Dot(vectorWallBall, Trigger.right)) < Trigger.localScale.x/2 + BallRadius) &&
                (Mathf.Abs(Vector3.Dot(vectorWallBall, Trigger.forward)) < Trigger.localScale.z/2 + BallRadius))
            {
                GameManager.Instance.MinusBall();
                Debug.Log("col");

            }
        
        }

    }

    private Vector3 WallCollision(Transform ballTransform)
    {
        ForceDelta = 0.7f;
        
        foreach (var wall in EntityManager.Instance.walls)
        {
            Transform wallTrans = wall.transform;

            Vector3 tempBallPos = ballTransform.position;
            Vector3 normalWall = wallTrans.up;
            Vector3 vectorWallBall = ballTransform.position - wallTrans.position;

            if (!(Mathf.Abs(Vector3.Dot(vectorWallBall, normalWall)) < BallRadius + wallTrans.localScale.y/2) ||
                !(Mathf.Abs(Vector3.Dot(vectorWallBall, wallTrans.right)) < wallTrans.localScale.x/2 ) ||
                !(Mathf.Abs(Vector3.Dot(vectorWallBall, wallTrans.forward)) < wallTrans.localScale.z/2)) continue;
            
            tempBallPos -= VectorSpeed * Time.fixedDeltaTime;

            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += i / FramesCut * VectorSpeed * Time.fixedDeltaTime;
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
        
        foreach (var bumper in EntityManager.Instance.bumpers)
        {
            Vector3 tempBallPos = ballTransform.position;

            Transform bumperTransform = bumper.transform;
            
            float bumperRadius = bumperTransform.localScale.x / 2;
            Vector3 ballCyllinder = ballTransform.position - bumperTransform.position;
            Vector3 normal = ballCyllinder - (Vector3.Dot(ballCyllinder, bumperTransform.up) * bumperTransform.up);


            float dist = Vector3.Magnitude(normal);
            if (!(dist < BallRadius + bumperRadius )) continue;
            
            tempBallPos -= VectorSpeed * Time.fixedDeltaTime;
                
            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += 1 / FramesCut * VectorSpeed * Time.fixedDeltaTime;
                ballCyllinder =ballTransform.position - bumperTransform.position;
                normal =  ballCyllinder - (Vector3.Dot(ballCyllinder, bumperTransform.up) * bumperTransform.up);
                    
                dist = Vector3.Magnitude(normal);
                    
                if (dist < BallRadius + bumperRadius)
                {

                    break;
                }
            }
                
            gameObject.transform.position +=1.5f* Mathf.Abs( bumperRadius+BallRadius- dist) * normal.normalized;
            Vector3 perpendicular = normal.normalized * Vector3.Dot(VectorSpeed, normal.normalized);
            Vector3 parallel = VectorSpeed - perpendicular;
                
            VectorSpeed = parallel  - perpendicular  + ForceDelta*normal.normalized ;

            ScoreManager.Instance.AddScore(EntityManager.Instance.bumperScore);
            
            return tempBallPos;
        }

        return ballTransform.position;
    }

    private Vector3 FlipperCollision(Transform ballTransform)
    {
        foreach (var flipper in EntityManager.Instance.flippersCol)
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

            if (!(Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)) < BallRadius * (1+Mathf.Abs(GameManager.Instance.flipPower[index]))) ||
                !(Mathf.Abs(Vector3.Dot(vectorFlipperBall, flipperTransform.right)) < flipperRadiusX + BallRadius) ||
                !(Mathf.Abs(Vector3.Dot(vectorFlipperBall, flipperTransform.forward)) < flipperRadiusZ + BallRadius)) continue;
            
            tempBallPos -= VectorSpeed * Time.fixedDeltaTime;
                
            for (int i = 0; i < FramesCut; i++)
            {
                tempBallPos += i / FramesCut * VectorSpeed * Time.fixedDeltaTime;
                vectorFlipperBall = tempBallPos - flipperTransform.position - flipperTransform.up * flipperRadiusY;
                
                if (Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)) < BallRadius)
                {
                    break;
                }
            }

            gameObject.transform.position += 6*Mathf.Abs(Vector3.Dot(vectorFlipperBall, normalFlipper)+100*Mathf.Abs(GameManager.Instance.flipPower[index])) * normalFlipper;
            
            Vector3 perpendicular= normalFlipper * Vector3.Dot(VectorSpeed, normalFlipper);
            Vector3 parallel = VectorSpeed - perpendicular;

            VectorSpeed = parallel - 0.8f*perpendicular 
                          + normalFlipper *Mathf.Max(0, GameManager.Instance.flipPower[index] *100) *(1.5f+Vector3.Dot(vectorFlipperBall, flipperTransform.right)/(flipperRadiusX + BallRadius));

            return tempBallPos;
        }

        return ballTransform.position;
    }
}
