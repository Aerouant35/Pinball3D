using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour
{
    private float LaunchPower;
    private Vector3 InitPos;
    public Slider LaunchPowerUI;
    public GameObject Ball;

    private Collision colscript;
    // Start is called before the first frame update
    void Start()
    {
        InitPos = gameObject.transform.position;
        colscript= Ball.GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            LaunchPower += Time.deltaTime;
            LaunchPower = Mathf.Min(2f, LaunchPower);
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Ball.transform.position.x>5)
            {
                colscript.speed.z += LaunchPower * 15;
            }
            
            LaunchPower = 0;
           
        }

        LaunchPowerUI.value = LaunchPower/2;
        gameObject.transform.position = InitPos - new Vector3(0f,0f,LaunchPower/5) ;
        
    }
    
}
