using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class EvolveNetParameters : MonoBehaviour {

    public GameObject NeuralSimulationPrefab;

    public int population = 30;

    public int numberOfGenerations = 100;

    public float startVariation = 1;
    public float initialBias = 0;

    public float mutationAmount = 0.1f;
    public int mutationFrequency = 10;

    public bool randomiseBallPosition = false;

    int generation = 0;

    NeuralNet[] currentGeneration; //population members of the current generation

    NetParameters[] currentGenParameters; //parameters of the current generation;

    public static float bestPerformanceSoFar = 0;

    // a single set to test consistency
    NetParameters testParams;

    List<float> performanceList = new List<float>();

    string simulationType = "";

    public bool loadData = false;
    public string fileToLoad = "";

    Text displayInfo;


    void Start () {

        GameObject obj = GameObject.FindGameObjectWithTag("ohd");
        if(obj != null)
            displayInfo = obj.GetComponent<Text>();

        //create 0th generation of parameter sets

        currentGenParameters = new NetParameters[population];

        //generate random weights and biases for 0th generation, set id and generation id

        //testParams = NetParameters.SetUniformValues(0, 0, 2f, 2f);

        for (int i = 0; i < currentGenParameters.Length; i++)
        {
            //currentGenParameters[i] = new NetParameters(NetParameters.SetRandomValues(i, generation, startVariation, 0));
            //currentGenParameters[i] = new NetParameters(testParams);


            if (loadData)
            {
                currentGenParameters[i] = SaveAndLoadParams.LoadData(fileToLoad);
            }else
            {
                currentGenParameters[i] = new NetParameters(NetParameters.SetRandomValues(i, generation, startVariation, 0));

            }

        }

        
        generation = currentGenParameters[0].generation;

        if (loadData)
        {
            performanceList = SaveAndLoadParams.LoadDataPerformanceHistory(fileToLoad).ToList();
        }



        //spaun simulation with generation 00 paramters

        currentGeneration = SpaunNewGeneration(currentGenParameters);

        

    }


    bool FinishedStop = false;
    
	
	void Update () {

        

        if (!FinishedStop)
        {

            


            //check if all simulations have ended

            int finishedSims = 0;

            for (int i = 0; i < currentGeneration.Length; i++)
            {
                if (currentGeneration[i].SimulationEnded)
                    finishedSims++;

            }

            //array for storing performance values for sorting based on performance later

            float[] performances = new float[currentGeneration.Length];
            int[] ids = new int[currentGeneration.Length];


            //if all have finished then store performance values

            if (finishedSims == currentGeneration.Length)
            {
                for (int i = 0; i < currentGeneration.Length; i++)
                {
                    performances[i] = currentGeneration[i].performanceMetric;

                    currentGenParameters[i].performance = currentGeneration[i].performanceMetric;


                    ids[i] = i;
                }





                //sort generation into order of best performance

                //bubble sort, keeping track of id movements
                for (int i = 0; i < currentGeneration.Length; i++)
                {
                    for (int j = 0; j < currentGeneration.Length - 1; j++)
                    {
                        if (performances[j + 1] > performances[j])
                        {
                            float temp = performances[j];
                            performances[j] = performances[j + 1];
                            performances[j + 1] = temp;

                            int tempInt = ids[j];
                            ids[j] = ids[j + 1];
                            ids[j + 1] = tempInt;
                        }
                    }
                }

                for(int i = 0; i < currentGeneration.Length; i++)
                {
                }

                //print the best performance so far

                bestPerformanceSoFar = ( performances[0]);
                performanceList.Add(bestPerformanceSoFar);

                float worst = (performances[currentGeneration.Length - 1]);
                print("Generation " + generation + " best " + bestPerformanceSoFar + " worst " + worst + " mss");


                //take the best 5 NetworkParameters based on performance

                NetParameters[] bestOfGeneration = new NetParameters[5];

                for (int i = 0; i < 5; i++)
                {
                    bestOfGeneration[i] = new NetParameters(currentGenParameters[ids[i]]); //duplicates the parameters from current generation winners
                    //print("best performances going to next round " + bestOfGeneration[i].performance);

                }

                

                //increments generation counter for the next generation

                generation++;



                



                if (generation < numberOfGenerations)
                {
                    if(displayInfo != null)
                        displayInfo.text = "generation\n" + generation.ToString() + "\nscore\n" + (bestPerformanceSoFar*100f).ToString("F3");

                    //creates a new generation

                    //spaun new set taking the 2 best from the last one
                    for (int i = 0; i < 2; i++)
                    {
                        currentGenParameters[i] = new NetParameters(bestOfGeneration[i]);
                        currentGenParameters[i].generation = generation;
                        currentGenParameters[i].id = i;
                    }


                    /*
                    for (int i = 2; i < 32; i++)
                    {
                        //mutate
                        currentGenParameters[i] = new NetParameters(MutateParameters(i, generation, bestOfGeneration[0]));

                    }

                    for (int i = 32; i < 62; i++)
                    {
                        //cross breed
                        currentGenParameters[i] = new NetParameters(CrossBreed(i, generation, bestOfGeneration[0], bestOfGeneration[1], false));

                    }

                    for (int i = 62; i < 100; i++)
                    {
                        //cross breed with mutation
                        currentGenParameters[i] = new NetParameters(CrossBreed(i, generation, bestOfGeneration[0], bestOfGeneration[1], true));

                    }

                    for (int i = 100; i < currentGenParameters.Length; i++)
                    {
                        //cross breed with mutation
                        currentGenParameters[i] = new NetParameters(SetRandomValues(i, generation));

                    }

                    for (int i = currentGenParameters.Length - 10; i < currentGenParameters.Length; i++)
                    {
                        //currentGenParameters[i] = new NetParameters(SetRandomValues(i, generation));
                        //currentGenParameters[i] = new NetParameters(SetRandomValues(i, generation));

                    }
                    */

                    for (int i = 2; i < 76; i++)
                    {
                        //mutate
                        //currentGenParameters[i] = new NetParameters(MutateParameters(i, generation, bestOfGeneration[0]));

                    }
                    for (int i = 76; i < currentGenParameters.Length; i++)
                    {
                        //mutate
                        //currentGenParameters[i] = new NetParameters(MutateParameters(i, generation, bestOfGeneration[1]));

                    }

                    for (int i = 2; i < currentGenParameters.Length; i++)
                    {
                        //currentGenParameters[i] = new NetParameters(bestOfGeneration[0]);
                        //currentGenParameters[i] = new NetParameters(bestOfGeneration[i%5]);  
                        //currentGenParameters[i] = new NetParameters(MutateParameters(i, generation, currentGenParameters[i]));
                        //currentGenParameters[i] = new NetParameters(SetRandomValues(i, generation));
                        //currentGenParameters[i] = new NetParameters(SetUniformValues(0, generation));
                        //currentGenParameters[i] = new NetParameters(testParams);
                        currentGenParameters[i] = new NetParameters(NetParameters.CrossBreed(i, generation, bestOfGeneration[0], bestOfGeneration[1], mutationAmount, mutationFrequency));
                        

                        //currentGenParameters[i] = new NetParameters(MutateParameters(i, generation, bestOfGeneration[0]));
                       // currentGenParameters[i] = new NetParameters(CrossBreed(i, generation, bestOfGeneration[0], bestOfGeneration[1], true));


                    }

                    if(bestPerformanceSoFar < 0.030f)
                    for (int i = currentGenParameters.Length - 350; i < currentGenParameters.Length; i++)
                    {
                            
                            //currentGenParameters[i] = new NetParameters(NetParameters.CrossBreed(i, generation, bestOfGeneration[0], bestOfGeneration[1], 2f, 2));
                            //currentGenParameters[i] = new NetParameters(NetParameters.SetRandomValues(i, generation, 2f, 2));


                    }

                    simulationType = "cross breed";
                    hiddenlayers = 1;

                    //destroy current generation :(

                    foreach (NeuralNet NN in currentGeneration)
                    {
                        Destroy(NN.gameObject);
                    }

                    //spaun new generation
                    currentGeneration = SpaunNewGeneration(currentGenParameters);


                    //if the performance reaches a threshold then add challenge to the task by altering the positions of the balls
                    if(bestPerformanceSoFar > 0.07f)
                    {
                       triggerPositionRandomisation = true;
                       // randomiseBallPosition = true;

                        mutationFrequency = 30;
                        mutationAmount = 1f;
                    }

                    if (triggerPositionRandomisation || randomiseBallPosition)
                    {
                        for (int i = 0; i < currentGeneration.Length; i++)
                        {
                            currentGeneration[i].RandomlyPositionBalls = randomiseBallPosition;
                            //currentGeneration[i].positionRange = bestPerformanceSoFar * 5f < 0.4f ? bestPerformanceSoFar * 5f : 0.4f;
                        }
                    }

                }else
                {
                    EndRun(ids);
                }

            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndRun(ids);
            }
        }
    }

    int hiddenlayers = 2;

    bool triggerPositionRandomisation = false;


    void EndRun(int[] _ids)
    {
        FinishedStop = true;
        print("End of simulation run");

        string fileName = simulationType + "Pop " + population + " - Freq " + mutationFrequency + " - Amp " + mutationAmount + " - GenNo " + (generation - 1) + "  " + System.DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        PerformanceRecorder.Save(performanceList.ToArray(), fileName);

        //store this generation to file

        SaveAndLoadParams.SaveData(fileName, currentGenParameters[_ids[0]], performanceList.ToArray());
        
    }




    NeuralNet[] SpaunNewGeneration(NetParameters[] _generationX)
    {
        NeuralNet[] units = new NeuralNet[_generationX.Length];

        for (int i = 0; i < _generationX.Length; i++)
        {
            GameObject gX_unit = Instantiate(NeuralSimulationPrefab, this.transform);

            //gX_unit.transform.position += Vector3.down * 0.105f * (float)i; //puts the simulations on different levels so they dont interfere with each other.

            NeuralNet unit = gX_unit.GetComponent<NeuralNet>();
                
            unit.SetWeightsAndBiases(_generationX[i]);

            units[i] = unit;

        }

        return units;

    }


    
}
