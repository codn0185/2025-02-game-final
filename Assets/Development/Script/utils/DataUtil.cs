using System.IO;
using UnityEngine;

public static class DataUtil
{
    public static string Dir = Path.Combine(Application.persistentDataPath, "Data");
    public static string LoadJsonString(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        return null;
    }

    public static void SaveJsonString(string jsonData, string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(filePath, jsonData);
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

    public static void SaveJson<T>(T data, string filePath) where T : Data
    {
        string jsonData = JsonUtility.ToJson(data, true);
        SaveJsonString(jsonData, filePath);
    }

    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static void CreateDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public static void DeleteDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }
    }   
}