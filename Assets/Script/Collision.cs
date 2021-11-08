using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    struct Ruler
    {
        public Vector3 Base;
        public Vector3 Axis;
    }
    
    
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
    private float Acc = -9.81f;
    // Start is called before the first frame update
    void Start()
    {
        speed = Vector3.zero;



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        gameObject.transform.position += speed* Time.deltaTime;
        speed.y += Acc * Time.deltaTime;

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
            VplaneBall = gameObject.transform.position - wall.gameObject.transform.position;
            
            if (Mathf.Abs( Vector3.Dot(VplaneBall,NormalPlane))<gameObject.transform.localScale.x/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,XPlane))< wall.transform.localScale.x *10/2 &&
                Mathf.Abs( Vector3.Dot(VplaneBall,ZPlane))<wall.transform.localScale.z*10/2)
            {
                print("col");


                
                Vector3 perpendicular= NormalPlane * Vector3.Dot(speed, NormalPlane);
                Vector3 parallel = speed - perpendicular;
                speed = parallel - 1f*perpendicular;
            }
        }

        foreach (var cylinder in Cylinders)
        {
            float CylinderRadius = cylinder.transform.localScale.x / 2;
            float dist = Vector3.Distance(cylinder.transform.position, gameObject.transform.position);
            if (dist< gameObject.transform.localScale.x/2 + CylinderRadius)
            {
                Vector3 CylinderBall = gameObject.transform.position - cylinder.transform.position;
                Vector3 perpendicular= CylinderBall * Vector3.Dot(speed, CylinderBall);
                Vector3 parallel = speed - perpendicular;
                speed = parallel -  1.5f*perpendicular;

            }
        }
        
        
        

    }


    

}
