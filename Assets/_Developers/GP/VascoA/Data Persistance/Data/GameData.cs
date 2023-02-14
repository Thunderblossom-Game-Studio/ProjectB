using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int points;
    public float health;
    public Vector3 position;

    public GameData()
    {
        this.points = 0;
        this.health = 0;
        this.position = Vector3.zero;
    }
}
