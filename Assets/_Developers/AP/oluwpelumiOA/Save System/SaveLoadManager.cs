using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public enum LogType { Info,Warning,Error }
    
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Game Save/";
    public static string SAVE_EXTENSION = ".txt";
    public static readonly string keyWord = "159078364";

    public static void Save<T>(string fileName, T t, bool encypted = false) where T : class
    {
        if (!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER);
        string json = JsonUtility.ToJson(t);
        if (encypted) json = EncryptDecrypt(json);
        File.WriteAllText(string.Concat(SAVE_FOLDER, fileName, SAVE_EXTENSION), json);
    }

    public static T Load<T>(string fileName, bool encypted = false) where T : class
    {
        if (File.Exists(SAVE_FOLDER + fileName + SAVE_EXTENSION))
        {
            string saveString = File.ReadAllText(string.Concat(SAVE_FOLDER, fileName, SAVE_EXTENSION));
            if (encypted) saveString = EncryptDecrypt(saveString);

            if (saveString != null)
            {
                T t = JsonUtility.FromJson<T>(saveString);
                return t;
            }
            return null;
        }
        else
        {
            LogError(LogType.Error, "File does not exist");
            return null;
        }
    }

    public static void Delete(string fileName)
    {
        if (File.Exists(SAVE_FOLDER + fileName + SAVE_EXTENSION)) File.Delete(SAVE_FOLDER + fileName + SAVE_EXTENSION);
        else LogError(LogType.Error, "File does not exist");
    }

    public static string EncryptDecrypt(string saveString)
    {
        string encryptedString = string.Empty;
        for (int i = 0; i < saveString.Length; i++) encryptedString += (char)(saveString[i] ^ keyWord[i % keyWord.Length]);
        return encryptedString;
    }

    public static void LogError(LogType logType,  string message)
    {
        switch (logType)
        {
            case LogType.Info: Debug.Log("<color=green" + message + "</color>");  break;
            case LogType.Warning: Debug.Log("<color=yellow>" + message + "</color>");  break;
            case LogType.Error:  Debug.Log("<color=red>" + message + "</color>"); break;
        }
    }
}
