using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovePoints : MonoBehaviour
{
    [SerializeField]private List<Transform> movePoints = new List<Transform>();
    public List<Transform> GetPoint { get { return movePoints; } }
    private void Awake()
    {
        int childCount = transform.childCount;
        for(int i=0; i < childCount; i++)
        {
            movePoints.Add(transform.GetChild(i));
        }
    }
}
