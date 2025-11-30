using System.IO;
using UnityEngine;

public static class DataUtil
{
    public static string DataDirectory = Application.persistentDataPath;

    public static string LoadJsonString(string filePath)
    {
        string fullPath = Path.Combine(DataDirectory, filePath);
        if (File.Exists(fullPath))
        {
            return File.ReadAllText(fullPath);
        }
        return null;
    }

    public static void SaveJsonString(string filePath, string jsonData)
    {
        string fullPath = Path.Combine(DataDirectory, filePath);
        string directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(fullPath, jsonData);
    }

    public static T LoadJson<T>(string filePath) where T : Data
    {
        string jsonData = LoadJsonString(filePath);
        if (!string.IsNullOrEmpty(jsonData))
        {
            return JsonUtility.FromJson<T>(jsonData);
        }
        return null;
    }

    public static void SaveJson<T>(string filePath, T data) where T : Data
    {
        string jsonData = JsonUtility.ToJson(data, true);
        SaveJsonString(filePath, jsonData);
    }


}