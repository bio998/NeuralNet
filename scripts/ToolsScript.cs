using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsScript : MonoBehaviour {

    //contains important things like dot products and vector and matrix algebra

    public static float Sigmoid(float _x)
    {
        return 1f / (1f + Mathf.Exp(-_x));
    }

    

	void Start () {
		
	}
	
	void Update () {
		
	}
}
