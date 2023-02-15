using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string gameDataFileName = "";
    private string settingsDataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionKey = "blimps";   

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.gameDataFileName = dataFileName + "_game";
        this.settingsDataFileName = dataFileName + "_settings";
        this.useEncryption = useEncryption;
    }

    #region Settings Data

    public SettingsData LoadSettingsData()
    {
        string fullPath = Path.Combine(dataDirPath, settingsDataFileName);

        SettingsData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }


                loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
            }
            catch (Exception e)
            {

                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        
        return loadedData;
    }

    public void SaveSettingsData(SettingsData settingsData)
    {
        string fullPath = Path.Combine(dataDirPath, settingsDataFileName);

        try
        {
            string dataToSave = JsonUtility.ToJson(settingsData);

            if (useEncryption)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {

            Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
        }     
    }
    #endregion
    
    #region Game Data
    public GameData LoadGameData()
    {
        string fullPath = Path.Combine(dataDirPath, gameDataFileName);

        GameData loadedData = null;
        
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }


                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);


            }
            catch (Exception e)
            {

                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        
        return loadedData;
    }

    public void SaveGameData(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, gameDataFileName);
        
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error while saving data to file: " + fullPath + "\n" + e);
        }
    }
    #endregion

    
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        //XOR encryption
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }

        return modifiedData;
    }
}
