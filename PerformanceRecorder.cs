using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class PerformanceRecorder : MonoBehaviour
{
    public static void Save(float[] data, string filename)
    {
        List<string[]> rowData = new List<string[]>();

        int generationIndex = 0;

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[2];
        rowDataTemp[0] = "Generation";
        rowDataTemp[1] = "Performance";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < data.Length; i++)
        {
            rowDataTemp = new string[2];

            rowDataTemp[0] = generationIndex.ToString(); // name

            generationIndex++;

            rowDataTemp[1] = data[i].ToString(); // ID

            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(filename);

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private static string getPath(string _filename)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + _filename + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }
}