using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{

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
}