using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    LineRenderer line;

    public Transform ball1;
    public Transform ball2;
    Vector3[] positions = new Vector3[2];

    NeuralNet neuralNet;

    Material material;

    


    void Start () {

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;

        positions[0] = ball1.position;
        positions[1] = ball2.position;
        line.SetPositions(positions);
        line.enabled = true;

        neuralNet = GetComponentInParent<NeuralNet>();

        material = GetComponent<Renderer>().material;
		
	}
	
	void Update () {

        float bestLastPerformance = 0;
        bestLastPerformance = EvolveNetParameters.bestPerformanceSoFar;

        if(ball1 != null)
            positions[0] = ball1.position;
        if(ball2 != null)
            positions[1] = ball2.position;
        line.SetPositions(positions);

        float hue = neuralNet.performanceMetric / bestLastPerformance;

        material.SetColor("_TintColor", Color.HSVToRGB(hue, 1f, 0.1f));
        
		
	}
}
