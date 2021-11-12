using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;

    public List<Transform> planes;
    public List<Transform> bumpers;
    public List<Transform> walls;
    public List<Transform> flippersCol;
    public List<Transform> triggerBox;

    public int bumperScore = 10;
    
    private void Awake()
    {
        if (!ReferenceEquals(Instance, null)){
            Destroy(Instance);
        }
        
        Instance = this;
    }
}
