using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool tes1;
    public bool tes2;
    // Update is called once per frame
    void Update()
    {
        tes1 = Input.GetMouseButtonDown(1);
        tes2 = Input.GetMouseButtonUp(1);
        if (tes1)
        {
            Debug.Log("눌림");
        }
        if (tes2)
        {
            Debug.Log("눌림2");
        }
        
    }
}
