using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPickup : MonoBehaviour
{
    [Tooltip("The ID of the Audio Clip stored in the SFX Bank.")]
    [SerializeField] private string audioID;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Audio Cube: Collided with player.");
            if (!AudioManager.Instance) Debug.LogWarning("Audio Cube: Please make sure there is an Audio Manager in the scene!");
            AudioManager.PlaySoundEffect(audioID);
            Destroy(gameObject);
        }
    }
}





