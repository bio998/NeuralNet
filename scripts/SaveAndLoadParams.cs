using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoadParams : MonoBehaviour {

    //functions for storing and retrieving neural net weights and bias parameters


    public static NetParameters LoadData(string _filename)
    {
        NetParamData _netParamData;

        string filePath = Path.Combine(Application.streamingAssetsPath, _filename);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            _netParamData = JsonUtility.FromJson<NetParamData>(dataAsJson);

            print("data loaded eg " + _netParamData.b_out[0].ToString());

            return new NetParameters(_netParamData);
        }
        else
        {
            Debug.LogError("Could not load parameter data");
            return null;
        }
    }

    public static float[] LoadDataPerformanceHistory(string _filename)
    {
        NetParamData _netParamData;

        string filePath = Path.Combine(Application.streamingAssetsPath, _filename);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            _netParamData = JsonUtility.FromJson<NetParamData>(dataAsJson);

            print("data loaded, performance history");

            return _netParamData.performanceHistory;
        }
        else
        {
            Debug.LogError("Could not load history data");
            return null;
        }
    }

    public static void SaveData(string _filename, NetParameters _netParameters, float[] _performanceHistory)
    {
        NetParamData data = new NetParamData(_netParameters);

        string dataAsJson = JsonUtility.ToJson(data, true);

        string filePath = Path.Combine(Application.streamingAssetsPath, _filename);

        File.WriteAllText(filePath, dataAsJson);

    }
}
