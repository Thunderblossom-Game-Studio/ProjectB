using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GelSystem : MonoBehaviour
{
    public ObjectPool<GelSplatter> _splatterObjectPool;
    public GameObject _dropletPrefab;
    public GameObject _splatterPrefab;

    [Header("Object Pooling")]
    public bool _isObjectPooling = false;
    //[SerializeField] bool _collectionChecks = true;
    //[SerializeField] int _maxPoolSize = 20;

    [Header("Gel Splatter")]
    public string _groundLayerName = "Ground";
    public string _playerTagName = "Player";
    public string _enemyTagName = "Enemy";
    public int _splatterLife = 3;
    public float _splatterScale = 1f;
    public float _splatterDepth = 0.4f;
    public float _splatterHeightAddition = 0.5f;
    public float _splatterRotationMin = -180f;
    public float _splatterRotationMax = 180f;

    public void CreateSplatter(GameObject droplet, Transform ground)
    {
        if (_isObjectPooling == true)
        {
            Debug.LogWarning("Disable Pooling!");
            var splatter = new GameObject("GelSplatter").AddComponent<GelSplatter>();
        }
        else
        {
            GameObject splatter = Instantiate(_splatterPrefab, new Vector3(droplet.transform.position.x, droplet.transform.position.y + _splatterHeightAddition, droplet.transform.position.z), Quaternion.Euler(0,0,0), ground);
            splatter.transform.up = ground.transform.up;
            splatter.transform.SetParent(null);
            splatter.transform.Rotate(-90, 0, Random.Range(_splatterRotationMin, _splatterRotationMax));
            splatter.transform.localScale = new Vector3 (_splatterScale, _splatterScale, _splatterDepth);
        }
    }

    public void CreateDroplet(Transform origin)
    {
        if (_isObjectPooling == true)
        {
            Debug.LogWarning("Disable Pooling!");
        }
        else
        {
            Instantiate(_dropletPrefab, origin.position, origin.rotation);
        }
    }

    public void CreateDroplet(Transform origin, Vector3 direction, float force)
    {
        if (_isObjectPooling == true)
        {
            Debug.LogWarning("Disable Pooling!");
        }
        else
        {
            GameObject droplet = Instantiate(_dropletPrefab, origin.position, origin.rotation);
            droplet.transform.forward = direction;
            droplet.GetComponent<Rigidbody>().AddForce(droplet.transform.forward * force, ForceMode.Impulse);
        }
    }
}
