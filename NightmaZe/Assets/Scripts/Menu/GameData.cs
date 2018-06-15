using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class GameData
{
    #region Instanziierung
    private static GameData instance;

    private GameData()
    {
        if (instance != null)
            return;
        instance = this;
        
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameData();
            }

            return instance;
        }
    }
    #endregion

    #region Settings
    // Volume of SFX and music
    public float musicVolume = 0;

    public float sfxVolume = 0;
    #endregion

    public void SaveSettings()
    {
        BinaryFormatter bF = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameSettings.dat");

        SaveDataSettings data = new SaveDataSettings();
        data.musicVolume = musicVolume;
        data.sfxVolume = sfxVolume;

        bF.Serialize(file, data);
        file.Close();
    }

    public void LoadSettings()
    {
        if (File.Exists(Application.persistentDataPath + "/gameSettings.dat"))
        {
            BinaryFormatter bF = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameSettings.dat", FileMode.Open);
            SaveDataSettings data = (SaveDataSettings)bF.Deserialize(file);
            file.Close();

            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
        }
    }
    
}

[Serializable]
class SaveDataSettings
{
    public float musicVolume;
    public float sfxVolume;
}