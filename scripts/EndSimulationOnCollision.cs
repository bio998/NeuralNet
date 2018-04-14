using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSimulationOnCollision : MonoBehaviour {


    void OnCollisionEnter(Collision col)
    {
        GetComponentInParent<NeuralNet>().EndSimulation();
    }
}
