using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GelSplatter : MonoBehaviour
{
    public GelSystem _gelSystem;
    private int _life;

    private void Awake()
    {
        if (_gelSystem == null)
        {
            _gelSystem = FindObjectOfType<GelSystem>();
        }
    }

    private void Start()
    {
        _life = _gelSystem._splatterLife;
    }

    private void OnTriggerEnter(Collider target)
    {
        OnCharacterTouched(target);
        Debug.Log(target.gameObject.name + " collided with " + this.gameObject.name);
    }
    
    public void OnCharacterTouched(Collider target)
    {
        if (target.gameObject.tag == _gelSystem._playerTagName || target.gameObject.tag == _gelSystem._enemyTagName)
        {
            //hazard, hurt others
            gameObject.transform.localScale = gameObject.transform.localScale / 2;
            _life -= 1;

            if (_life <= 0)
            {
                DestroySplatter();
            }
        }
    }

    public void DestroySplatter()
    {
        if (_gelSystem._isObjectPooling == true)
        {
            //_gelSystem._splatterObjectPool.Return(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
