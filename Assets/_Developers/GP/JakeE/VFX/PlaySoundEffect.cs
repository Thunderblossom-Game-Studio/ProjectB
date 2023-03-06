using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
   [SerializeField] private string _soundID;

   public void PlaySound() =>
      AudioManager.PlaySoundEffect(_soundID);
}
