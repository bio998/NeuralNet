using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWithRandomColour : MonoBehaviour {

    public Renderer ball1;
    public Renderer ball2;

	void Start () {

        Material mat = new Material(ball1.material);
        mat.color = Color.HSVToRGB(Random.Range(0f, 1f), 0.3f, 1f);
        ball1.material = mat;
        ball2.material = mat;
		
	}
	
	void Update () {
		
	}
}
