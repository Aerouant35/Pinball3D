using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    
    private Vector3 speed;
    
    private Vector3 VplaneBall;
    private Vector3 NormalPlane;
    private Vector3 XPlane;
    private Vector3 ZPlane;
    private Vector3 HorPlane;
    private Vector3 NormalSpeed;
    private Vector3 HorSpeed;
    public List<GameObject> Planes;
    public List<GameObject> Cylinders;
    public List<GameObject> Walls;
    public List<GameObject> Flippers;
    public GameObject LeftFlipper;
    public GameObject RightFlipper;
    private float Acc = -9.81f;

    private float Tleft;
    private float TRight;
    // Start is called before the first frame update
    void Start()
    {
        speed = Vector3.zero;
        Tleft = 0f;
        TRight = 0f;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 ActualSpeed = speed;
        gameObject.transform.position += speed * Time.deltaTime;
        speed.y += Acc * Time.deltaTime;
        Vector3 BallPos = gameObject.transform.position;


        foreach (var plane in Planes)
        {
            NormalPlane = plane.transform.up;
            XPlane = plane.transform.right;
            ZPlane = plane.transform.forward;
            VplaneBall = gameObject.transform.position - plane.gameObject.transform.position;
            
            if (Mathf.Abs( Vector3.Dot(VplaneBall,NormalPlane))<gameObject.transform.localScale.x/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,XPlane))< plane.transform.localScale.x *10/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,ZPlane))<plane.transform.localScale.z*10/2)
            {
                print("col");


                
                Vector3 perpendicular= NormalPlane * Vector3.Dot(speed, NormalPlane);
                Vector3 parallel = speed - perpendicular;
                speed = parallel - 0.5f *perpendicular;
            }
        }
        
        foreach (var wall in Walls)
        {
            NormalPlane = wall.transform.up;
            XPlane = wall.transform.right;
            ZPlane = wall.transform.forward;
            VplaneBall = BallPos - wall.gameObject.transform.position;
            
            if (Mathf.Abs( Vector3.Dot(VplaneBall,NormalPlane))<gameObject.transform.localScale.x/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,XPlane))< wall.transform.localScale.x *10/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,ZPlane))<wall.transform.localScale.z*10/2)
            {

                BallPos -= ActualSpeed * Time.deltaTime;
                for (int i = 0; i < 1000; i++)
                {
                    BallPos += i / 1000 * ActualSpeed * Time.deltaTime;
                    VplaneBall = BallPos - wall.gameObject.transform.position;
                    if (Mathf.Abs(Vector3.Dot(VplaneBall, NormalPlane)) < gameObject.transform.localScale.x / 2)
                    {
                        break;
                    }
                }
                gameObject.transform.position = BallPos;
                Vector3 perpendicular= NormalPlane * Vector3.Dot(speed, NormalPlane);
                Vector3 parallel = speed - perpendicular;
                speed = parallel - .9f*perpendicular;
            }
        }

        foreach (var cylinder in Cylinders)
        {
            float CylinderRadius = cylinder.transform.localScale.x / 2;
            float dist = Vector3.Distance(cylinder.transform.position, BallPos);
            if (dist< gameObject.transform.localScale.x/2 + CylinderRadius)
            {
                BallPos -= ActualSpeed * Time.deltaTime;
                for (int i = 0; i < 1000; i++)
                {
                    BallPos += i / 1000 * ActualSpeed * Time.deltaTime;
                    dist = Vector3.Distance(cylinder.transform.position, BallPos);
                    if (dist < gameObject.transform.localScale.x / 2 + CylinderRadius)
                    {
                        break;
                    }
                }
                gameObject.transform.position = BallPos;
                Vector3 CylinderBall = gameObject.transform.position - cylinder.transform.position;
                Vector3 perpendicular= CylinderBall * Vector3.Dot(speed, CylinderBall);
                Vector3 parallel = speed - perpendicular;
                speed = 1.5f*parallel -  1.5f*perpendicular;

            }
        }
        
        //flipper

        foreach (var flipper in Flippers)
        {
            NormalPlane = flipper.transform.up;
            XPlane = flipper.transform.right;
            ZPlane = flipper.transform.forward;
            
            VplaneBall = BallPos - flipper.gameObject.transform.position - flipper.transform.up* flipper.transform.localScale.y/2;
            if (Mathf.Abs( Vector3.Dot(VplaneBall,NormalPlane))<gameObject.transform.localScale.x/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,XPlane))< flipper.transform.localScale.x/2 + gameObject.transform.localScale.x / 2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,ZPlane))<flipper.transform.localScale.z/2 + gameObject.transform.localScale.x / 2)
            {

                BallPos -= ActualSpeed * Time.deltaTime;
                for (int i = 0; i < 1000; i++)
                {
                    BallPos += i / 1000 * ActualSpeed * Time.deltaTime;
                    VplaneBall = BallPos - flipper.gameObject.transform.position - flipper.transform.up * flipper.transform.localScale.y / 2;
                    if (Mathf.Abs(Vector3.Dot(VplaneBall, NormalPlane)) < gameObject.transform.localScale.x / 2)
                    {
                        break;
                    }
                }
                gameObject.transform.position = BallPos;
                Vector3 perpendicular= NormalPlane * Vector3.Dot(speed, NormalPlane);
                Vector3 parallel = speed - perpendicular;
                speed = parallel - perpendicular;
            }
        }

        
        if (Input.GetAxis("Horizontal" )<0)
        {
            Tleft += 5*Time.deltaTime;
            Tleft= Mathf.Min(1, Tleft);
        }
        else
        {
            Tleft -= 5*Time.deltaTime;
            Tleft = Mathf.Max(0, Tleft);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            TRight += 5 * Time.deltaTime;
            TRight = Mathf.Min(1, TRight);
        }
        else
        {
            TRight -= 5 * Time.deltaTime;
            TRight = Mathf.Max(0, TRight);
        }
        Debug.Log( Tleft);
        LeftFlipper.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(35, -35,Tleft),0);
        RightFlipper.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(-35, 35, TRight),0);




    }


    

}
