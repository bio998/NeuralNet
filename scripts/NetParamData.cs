using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wrapper to save data to json

[System.Serializable]
public class NetParamData {

    public string note = "";
    public int generation;

    public float[] performanceHistory;

    //weights

    public float[] w_ij_i0;
    public float[] w_ij_i1;
    public float[] w_ij_i2;
    public float[] w_ij_i3;
    public float[] w_ij_i4;
    public float[] w_ij_i5;
    public float[] w_ij_i6;
    public float[] w_ij_i7;


    
    public float[] w_out_j0;
    public float[] w_out_j1;
    public float[] w_out_j2;
    public float[] w_out_j3;
    public float[] w_out_j4;
    public float[] w_out_j5;
    public float[] w_out_j6;
    public float[] w_out_j7;


    public float[] b_j;
    public float[] b_out;




    public NetParamData(NetParameters _netParameters)
    {

        
        w_ij_i0 = _netParameters.weights1[0];
        w_ij_i1 = _netParameters.weights1[1];
        w_ij_i2 = _netParameters.weights1[2];
        w_ij_i3 = _netParameters.weights1[3];
        w_ij_i4 = _netParameters.weights1[4];
        w_ij_i5 = _netParameters.weights1[5];
        w_ij_i6 = _netParameters.weights1[6];
        w_ij_i7 = _netParameters.weights1[7];

        w_out_j0 = _netParameters.weights2[0];
        w_out_j1 = _netParameters.weights2[1];
        w_out_j2 = _netParameters.weights2[2];
        w_out_j3 = _netParameters.weights2[3];
        w_out_j4 = _netParameters.weights2[4];
        w_out_j5 = _netParameters.weights2[5];
        w_out_j6 = _netParameters.weights2[6];
        w_out_j7 = _netParameters.weights2[7];

        b_j = _netParameters.biases1;
        b_out = _netParameters.biases2;

        generation = _netParameters.generation;
        note = _netParameters.note;
    }

    public NetParamData(NetParameters _netParameters, float[] _performanceHistory)
    {


        w_ij_i0 = _netParameters.weights1[0];
        w_ij_i1 = _netParameters.weights1[1];
        w_ij_i2 = _netParameters.weights1[2];
        w_ij_i3 = _netParameters.weights1[3];
        w_ij_i4 = _netParameters.weights1[4];
        w_ij_i5 = _netParameters.weights1[5];
        w_ij_i6 = _netParameters.weights1[6];
        w_ij_i7 = _netParameters.weights1[7];

        w_out_j0 = _netParameters.weights2[0];
        w_out_j1 = _netParameters.weights2[1];
        w_out_j2 = _netParameters.weights2[2];
        w_out_j3 = _netParameters.weights2[3];
        w_out_j4 = _netParameters.weights2[4];
        w_out_j5 = _netParameters.weights2[5];
        w_out_j6 = _netParameters.weights2[6];
        w_out_j7 = _netParameters.weights2[7];

        b_j = _netParameters.biases1;
        b_out = _netParameters.biases2;

        generation = _netParameters.generation;
        note = _netParameters.note;

        performanceHistory = _performanceHistory;
    }

}
