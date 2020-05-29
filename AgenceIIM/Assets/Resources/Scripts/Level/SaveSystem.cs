using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    #region Score

    public static void SavePoints(int[] points, string _name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path;

        path = Application.persistentDataPath + "/point" + _name + ".save";

        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, points);
        stream.Close();
    }

    public static void SetPoints(string _nameMonde, int idLevel, int score)
    {
        int[] points = LoadPoints(_nameMonde);

        if (idLevel >= points.Length)
        {
            Debug.LogError("Save error");
        }
        else
        {
            if (points[idLevel] > score || points[idLevel] == -1)
            {
                points[idLevel] = score;
            }
        }
        SavePoints(points, _nameMonde);
    }

    public static int[] LoadPoints(string _nameMonde)
    {
        string path = Application.persistentDataPath + "/point" + _nameMonde + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int[] points = formatter.Deserialize(stream) as int[];
            stream.Close();
            return points;
        }
        else
        {
            SavePoints(new int[0], _nameMonde);
            return LoadPoints(_nameMonde);
        }
    }

    #endregion

    #region level
    public static void SaveLevel(Level level, string _name = null)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
        if (_name == null)
        {
            path = Application.persistentDataPath + "/level" + level.nameLevel + ".save";
        }
        else
        {
            path = Application.persistentDataPath + "/level" + _name + ".save";
        }
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(level);
        formatter.Serialize(stream, data);
        stream.Close();


    }

    public static LevelData LoadLevel(string nameLevel)
    {
        string path = Application.persistentDataPath + "/level" + nameLevel + ".save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();
            return data;
        }
        else
        {
            SaveLevel(new Level(), nameLevel);
            return LoadLevel(nameLevel);
        }
    }
    #endregion
}