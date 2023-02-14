using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    void LoadGameData(GameData data);

    void SaveGameData(ref GameData data);

    void LoadSettingsData(SettingsData data);

    void SaveSettingsData(ref SettingsData data);
}
