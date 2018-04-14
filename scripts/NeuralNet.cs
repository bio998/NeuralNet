using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNet : MonoBehaviour {

    //neural network for this 2 ball problem

    public Rigidbody ball1;
    public Rigidbody ball2;

    public float radius_ball = 0.0025f;
    public float radius_perimeter;

    float[][] w1; //weights
    float[][] w2; //weights
    float[][] w3; //weights
    float[] b1; //bias to h layer 1
    float[] b2; //bias to h layer 2
    float[] b3; //bias to output


    float[] xIn = new float[8];

    float[] h1 = new float[8];

    float[] h2 = new float[8];

    float[] xOut = new float[4];

    bool paramsSet = false;


    //parameters for the performance metric
    //public float performanceMetric0 = 0;
    //public float performanceMetric1 = 0;
    public float performanceMetric = 0;
    public bool SimulationEnded = false;
    public int id;
    public int generation;


    Text performanceText;
    float timeAtSpaun;
    float lastPerformance = 0;


    public bool RandomlyPositionBalls = false;
    public bool RandomlyPositionBallsMore = false;

    public float kickForce = 1f;
    public float positionRange = 0.4f;


    void Start()
    {
        performanceText = GetComponentInChildren<Text>();

        timeAtSpaun = Time.timeSinceLevelLoad;

        if (RandomlyPositionBalls)
        {
            RandomlyPlaceBallsToBegin();
        }

        
    }

    void RandomlyPlaceBallsToBegin()
    {

        ball1.position = new Vector3(Random.Range(-0.3f, 0.3f), ball1.position.y, Random.Range(-0.3f, 0.3f));
        ball2.position = new Vector3(Random.Range(-0.3f, 0.3f), ball2.position.y, Random.Range(-0.3f, 0.3f));

        if (RandomlyPositionBallsMore)
        {
            Vector2 randomInsideCircle1 = Random.insideUnitCircle * positionRange;
            Vector2 randomInsideCircle2 = Random.insideUnitCircle * positionRange;
            ball1.position = new Vector3(randomInsideCircle1.x, 0, randomInsideCircle1.y);
            ball2.position = new Vector3(randomInsideCircle2.x, 0, randomInsideCircle2.y);
        }
    }


    //set up the neural network (called from EvolveNetParameters)
    public void SetWeightsAndBiases(NetParameters netParams)
    {
        w1 = netParams.weights1;
        w2 = netParams.weights2;

        b1 = netParams.biases1;
        b2 = netParams.biases2;

        id = netParams.id;
        generation = netParams.generation;

        paramsSet = true;

    }

    //take relevant simulation object parameters as inputs to the neural net
    void PopulateXIn(Rigidbody _ball1, Rigidbody _ball2)
    {
        xIn[0] = _ball1.position.x;
        xIn[1] = _ball1.position.z;
        xIn[2] = _ball1.velocity.x;
        xIn[3] = _ball1.velocity.z;

        xIn[4] = _ball2.position.x;
        xIn[5] = _ball2.position.z;
        xIn[6] = _ball2.velocity.x;
        xIn[7] = _ball2.velocity.z;
    }


    
    

    
	void FixedUpdate () {


        //neural net for determining outputs (forces on balls)


        if (paramsSet && !SimulationEnded)
        {

            //set up of weights and biases done in Start()

            //take the inputs //////////////// INPUT

            PopulateXIn(ball1, ball2);

            //////////////////////////////////  FIRST HIDDEN LAYER
            //multiply by the weights for each node of the first hidden layer and sum

            float[] h1pre = new float[8]; //pre-transfer function store of summed output x weights
            for (int i = 0; i < h1pre.Length; i++)
                h1pre[i] = 0;

            for (int j = 0; j < 8; j++)
            {
                //add the bias

                h1pre[j] = b1[j];

                for (int i = 0; i < 8; i++)
                {
                    h1pre[j] += xIn[i] * w1[i][j];
                }

                //apply the transfer function (sigmoid) to determine the first hidden layer output

                h1[j] = ToolsScript.Sigmoid(h1pre[j]);
            }

            

            ///////////  /////////// OUTPUT
            //determine xOut

            float[] xOutPre2 = new float[4]; //pre-transfer function store of summed output x weights
            for (int i = 0; i < xOutPre2.Length; i++)
                xOutPre2[i] = 0;

            for (int o = 0; o < 4; o++)
            {
                //add the bias

                xOutPre2[o] = b2[o];

                for (int j = 0; j < 8; j++)
                {
                    xOutPre2[o] += h1[j] * w2[j][o];
                }

                xOut[o] = xOutPre2[o];// ToolsScript.Sigmoid(xOutPre2[m]);

            }

            

            //apply output to force on ball

            ball1.AddForce(new Vector3(xOut[0], 0, xOut[1]));
            ball2.AddForce(new Vector3(xOut[2], 0, xOut[3]));



            //calculate the performance metric, which is distance^2/time.  Calculated by doing (d/t)^2 * t

            performanceMetric += ball1.velocity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime;// Time.fixedDeltaTime;
            performanceMetric += ball2.velocity.magnitude * Time.fixedDeltaTime * Time.fixedDeltaTime;// * Time.fixedDeltaTime;

            //performanceMetric = performanceMetric0 * performanceMetric0 + performanceMetric1;

            //performanceText.text = performanceMetric.ToString("F3") + "  " + xOut[0].ToString("F2") + " " + xOut[1].ToString("F2") + " " + xOut[2].ToString("F2") + " " + xOut[3].ToString("F2");


            //some constraints on the simlation

            //stop if there it is progressing too slow

            //if (performanceMetric / (Time.timeSinceLevelLoad - timeAtSpaun) < 0.001f && (Time.timeSinceLevelLoad - timeAtSpaun) > 2f)
            //{
            //     EndSimulation();
            //}

            float timeSinceSpaun = Time.timeSinceLevelLoad - timeAtSpaun;

            //give a jolt to the balls if they are moving too slowly or if spacebar pressed

            if (((performanceMetric - lastPerformance) / Time.fixedDeltaTime < 0.001f && timeSinceSpaun > 2f ) || Input.GetKeyDown(KeyCode.Space))
            {
                ball1.AddForce(new Vector3(Random.Range(-kickForce, kickForce), 0, Random.Range(-kickForce, kickForce)), ForceMode.VelocityChange);
                ball2.AddForce(new Vector3(Random.Range(-kickForce, kickForce), 0, Random.Range(-kickForce, kickForce)), ForceMode.VelocityChange);
            }

            lastPerformance = performanceMetric; //store last frame's performance

            //stop simulation lasted more than 10 seconds

            if(timeSinceSpaun > 10f)
            {
                EndSimulation();

            }

            //stop if ball velocity is greater than some_distance/deltatime

            if (Vector3.Magnitude(ball1.velocity) > 0.05f / Time.fixedDeltaTime || Vector3.Magnitude(ball2.velocity) > 0.05f / Time.fixedDeltaTime){
                EndSimulation();
            }

            //stop if there is a collision: collision detection (collision of two balls or collision with perimeter.

            Vector3 center = Vector3.zero;

            float perimeterRadiusSquared = radius_perimeter * radius_perimeter;

            bool hitOtherBall = Vector3.SqrMagnitude(ball1.position - ball2.position) < radius_ball;
            bool hitPerimeter = Vector3.SqrMagnitude(ball1.position - center) > perimeterRadiusSquared || Vector3.SqrMagnitude(ball2.position - center) > perimeterRadiusSquared;
     
            if (hitOtherBall || hitPerimeter)
            {
                EndSimulation();
            }

            

        }




    }



    public void EndSimulation()
    {
        if (!SimulationEnded)
        {
            SimulationEnded = true;

            Renderer[] rends = GetComponentsInChildren<Renderer>();
            for(int i = 0; i < rends.Length; i++)
            {
                rends[i].enabled = false;
            }

            Destroy(ball1.gameObject);
            Destroy(ball2.gameObject);
            
            //print("simulation ended for " + generation + ": " + id);
        }
    }


    
}
