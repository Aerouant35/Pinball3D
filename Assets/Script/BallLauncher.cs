using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour
{
    private float LaunchPower;
    
    private Vector3 InitPos;
    
    [SerializeField]
    private Slider launchPowerUI;
        
    [Header("Parameters")]
    [SerializeField]
    private float maxPower = 2;

    [SerializeField]
    private float posThreshold = 2;

    [SerializeField]
    private float deltaForce = 2;

    [SerializeField]
    private float deltaPosition = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        InitPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = LaunchBall();
    }

    private Vector3 LaunchBall()
    {
        if (Input.GetButton("Launcher"))
        {
            LaunchPower += Time.deltaTime;
            LaunchPower = Math.Min(LaunchPower, maxPower);
        }

        if (Input.GetButtonUp("Launcher"))
        {
            if (Ball.Instance.transform.position.x > posThreshold)
            {
                Ball.Instance.VectorSpeed.z += LaunchPower * deltaForce;
            }
            
            launchPowerUI.value = LaunchPower = 0;
            Ball.Instance.transform.position += transform.localScale.y / 10* transform.up; ;
        }

        launchPowerUI.value = LaunchPower / maxPower;
        
        return InitPos - new Vector3( 0f, 0f, LaunchPower / deltaPosition);
    }
}
