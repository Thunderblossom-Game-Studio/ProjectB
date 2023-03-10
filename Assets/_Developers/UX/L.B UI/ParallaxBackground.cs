using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{

    [SerializeField] private RawImage bgimg;
    [SerializeField] private float _x, _y; 
    
        
  
    void Update()
    {
        bgimg.uvRect = new Rect(bgimg.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, bgimg.uvRect.size);
    }
}
