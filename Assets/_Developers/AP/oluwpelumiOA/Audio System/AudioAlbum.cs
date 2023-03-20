using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Album", menuName = "Audio/Audio Album")]
public class AudioAlbum : ScriptableObject
{
    [SerializeField] private string[] audioIDArray;

    public void Switch(ref int lastID)
    {
        lastID = lastID >= audioIDArray.Length - 1 ? 0 : lastID + 1;
        AudioManager.ChangeMusicWithFade(audioIDArray[lastID], true);
    }
}
