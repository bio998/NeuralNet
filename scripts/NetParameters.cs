using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetParameters{

    //params
    public float[][] weights1;
    public float[][] weights2;

    public float[] biases1;
    public float[] biases2;


    //for storing performance
    public int id;
    public int generation;
    public float performance = 0;
    public string note = "";

    
    // CONSTRUCTORS of network parameter data
    

    public NetParameters(int _id, int _generation, float[][] _weights1, float[][] _weights2, float[] _biases1, float[] _biases2)
    {
        this.id = _id;
        this.generation = _generation;

        this.weights1 = _weights1;
        this.weights2 = _weights2;

        this.biases1 = _biases1;
        this.biases2 = _biases2;

        //check if valid set
        if (weights1[0].Length != 8 || weights2[0].Length != 4)
        {
            //throw error

            Debug.LogError("weights wrong dimensions weights");
        }
        
        
        if(weights1.Length != 8 || weights2.Length != 8)
        {
            //throw error
            Debug.LogError("weights wrong dimensions");

        }

        if (biases1.Length != 8)
        {
            //throw error
            Debug.LogError("weights wrong dimensions b1");

        }
        if (biases2.Length != 4)
        {
            //throw error
            Debug.LogError("weights wrong dimensions b2");

        }

    }


    //build from existing instance of the class
    public NetParameters (NetParameters _netParameters)
    {
        this.weights1 = _netParameters.weights1;
        this.weights2 = _netParameters.weights2;
        this.biases1 = _netParameters.biases1;
        this.biases2 = _netParameters.biases2;
        this.performance = _netParameters.performance;
    }


    //build from data that was stored as the serializable format NetParamData
    public NetParameters (NetParamData _netParamData)
    {

        float[][] w1 = new float[8][]; ; //weights
        float[][] w2 = new float[8][]; ; //weights
        float[] b1 = new float[8]; ; //bias to h layer 1
        float[] b2 = new float[4]; ; //bias to output
        
        w1[0] = _netParamData.w_ij_i0;
        w1[1] = _netParamData.w_ij_i1;
        w1[2] = _netParamData.w_ij_i2;
        w1[3] = _netParamData.w_ij_i3;
        w1[4] = _netParamData.w_ij_i4;
        w1[5] = _netParamData.w_ij_i5;
        w1[6] = _netParamData.w_ij_i6;
        w1[7] = _netParamData.w_ij_i7;

        
        w2[0] = _netParamData.w_out_j0;
        w2[1] = _netParamData.w_out_j1;
        w2[2] = _netParamData.w_out_j2;
        w2[3] = _netParamData.w_out_j3;
        w2[4] = _netParamData.w_out_j4;
        w2[5] = _netParamData.w_out_j5;
        w2[6] = _netParamData.w_out_j6;
        w2[7] = _netParamData.w_out_j7;

        b1 = _netParamData.b_j;
        b2 = _netParamData.b_out;
        
        
        this.weights1 = w1;
        this.weights2 = w2;

        this.biases1 = b1;
        this.biases2 = b2;

        this.note = _netParamData.note;
        this.generation = _netParamData.generation;
    }










    // // New parameter generators (mutate, crossbreed, random, uniform etc.)



    public static NetParameters SetRandomValues(int _id, int _generation, float _variation, float _initialBias)
    {


        float[][] w1 = new float[8][]; ; //weights
        float[][] w2 = new float[8][]; ; //weights
        float[] b1 = new float[8]; ; //bias to h layer 1
        float[] b2 = new float[4]; ; //bias to output


        float xavierN1 = _variation * Mathf.Sqrt(6f / (8f + 8f));
        float xavierN2 = _variation * Mathf.Sqrt(6f / (8f + 4f));



        for (int i = 0; i < w1.Length; i++)
        {
            w1[i] = new float[8];
            w2[i] = new float[4];

            for (int j = 0; j < 8; j++)
            {
                w1[i][j] = Random.Range(-xavierN1, xavierN1);
                if (j < 4)
                    w2[i][j] = Random.Range(-xavierN2, xavierN2);
            }

            b1[i] = _initialBias;
            if (i < 4)
                b2[i] = _initialBias;
        }



        NetParameters randomNetParameters = new NetParameters(_id, _generation, w1, w2, b1, b2);

        return randomNetParameters;


    }




    public static NetParameters MutateParameters(int _id, int _generation, NetParameters _originalParameters, float _mutationAmplitude, float _mutationFrequency)
    {

        float[][] w1 = new float[8][]; ; //weights
        float[][] w2 = new float[8][]; ; //weights
        float[] b1 = new float[8]; ; //bias to h layer 1
        float[] b2 = new float[4]; ; //bias to output

        //reference input NetParameters
        float[][] w1_ = _originalParameters.weights1;
        float[][] w2_ = _originalParameters.weights2;

        float[] b1_ = _originalParameters.biases1;
        float[] b2_ = _originalParameters.biases2;

        for (int i = 0; i < w1.Length; i++)
        {
            w1[i] = new float[8]; //initialise
            w2[i] = new float[4];

            //copy values from input NetParameters

            for (int j = 0; j < 8; j++)
            {
                w1[i][j] = w1_[i][j];
                if (j < 4)
                    w2[i][j] = w2_[i][j];
            }
        }

        //if(Random.Range(1,frequency)%frequency == 0) //1/frequency chance of occurance
        float xavierN1 = _mutationAmplitude * Mathf.Sqrt(6f / (8f + 8f));
        float xavierN2 = _mutationAmplitude * Mathf.Sqrt(6f / (8f + 4f));

        for (int i = 0; i < w1.Length; i++)
        {

            for (int j = 0; j < 8; j++)
            {
                if (Random.Range(0, _mutationFrequency) % _mutationFrequency == 0)
                    w1[i][j] += Random.Range(-xavierN1, xavierN1);

                if (j < 4)
                {
                    if (Random.Range(0, _mutationFrequency) % _mutationFrequency == 0)
                            w2[i][j] += Random.Range(-xavierN1, xavierN1);
                }
            }
            if (Random.Range(0, _mutationFrequency) % _mutationFrequency == 0)
                b1[i] += Random.Range(-xavierN1, xavierN1);
            
            if (i < 4)
            {
                if (Random.Range(0, _mutationFrequency) % _mutationFrequency == 0)
                    b2[i] += Random.Range(-xavierN1, xavierN1);
            }
        }

       

        NetParameters mutatedNetParams = new NetParameters(_id, _generation, w1,w2,b1,b2);

        return mutatedNetParams;


    }





    public static NetParameters CrossBreed(int _id, int _generation, NetParameters _originalParameters, NetParameters _originalParameters2, float _mutationAmplitude, int _mutationFrequency)
    {

        float[][] w1 = new float[8][]; ; //weights
        float[][] w2 = new float[8][]; ; //weights
        float[] b1 = new float[8]; ; //bias to h layer 1
        float[] b2 = new float[4]; ; //bias to output

        //reference input NetParameters
        float[][] w1_a = _originalParameters.weights1;
        float[][] w2_a = _originalParameters.weights2;

        float[] b1_a = _originalParameters.biases1;
        float[] b2_a = _originalParameters.biases2;

        //reference input NetParameters2
        float[][] w1_b = _originalParameters2.weights1;
        float[][] w2_b = _originalParameters2.weights2;
        
        float[] b1_b = _originalParameters2.biases1;
        float[] b2_b = _originalParameters2.biases2;

        //parameters for mutation   if(Random.Range(1,frequency)%frequency == 0) //1/frequency chance of occurance
        float xavierN1 = _mutationAmplitude * Mathf.Sqrt(6f / (8f + 8f));
        float xavierN2 = _mutationAmplitude * Mathf.Sqrt(6f / (8f + 4f));

        for (int i = 0; i < w1.Length; i++)
        {
            w1[i] = new float[8]; //initialise
            w2[i] = new float[4];

            //copy values from input NetParameters

            for (int j = 0; j < 8; j++)
            {
                //weights crossbreed

                //cross
                w1[i][j] = Random.Range(0, 2) == 0 ? w1_a[i][j] : w1_b[i][j];

                //mutate
                w1[i][j] += Random.Range(0, _mutationFrequency) % _mutationFrequency == 0 ? Random.Range(-xavierN1, xavierN1) : 0;

                if (j < 4)
                {
                    w2[i][j] = Random.Range(0, 2) == 0 ? w2_a[i][j] : w2_b[i][j];
                    w2[i][j] += Random.Range(0, _mutationFrequency) % _mutationFrequency == 0 ? Random.Range(-xavierN1, xavierN1) : 0;
                }
            }

            //biases 

            //cross
            b1[i] = Random.Range(0, 2) == 0 ? b1_a[i] : b1_b[i];
            //mutate
            b1[i] += Random.Range(0, _mutationFrequency) % _mutationFrequency == 0 ? Random.Range(-xavierN1, xavierN1) : 0;

            if (i < 4)
            {
                //cross
                b2[i] = Random.Range(0, 2) == 0 ? b2_a[i] : b2_b[i];
                //mutate
                b2[i] += Random.Range(0, _mutationFrequency) % _mutationFrequency == 0 ? Random.Range(-xavierN1, xavierN1) : 0;
            }
        }
        

        NetParameters crossedParams = new NetParameters(_id, _generation, w1, w2, b1, b2);

        return crossedParams;


    }




    public static NetParameters SetUniformValues(int _id, int _generation, float _weight, float _bias)
    {

        float[][] w1 = new float[8][]; ; //weights
        float[][] w2 = new float[8][]; ; //weights
        float[] b1 = new float[8]; ; //bias to h layer 1
        float[] b2 = new float[4]; ; //bias to output
        
        for (int i = 0; i < w1.Length; i++)
        {
            w1[i] = new float[8];
            w2[i] = new float[4];

            for (int j = 0; j < 8; j++)
            {
                w1[i][j] = _weight;
                if (j < 4)
                    w2[i][j] = _weight;
            }

            b1[i] = _bias; 
            if (i < 4)
                b2[i] = _bias;
        }
        

        NetParameters uniformParams = new NetParameters(_id, _generation, w1,w2,b1,b2);

        return uniformParams;


    }
}
