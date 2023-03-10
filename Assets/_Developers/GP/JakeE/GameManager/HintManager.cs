using System.Collections;
using System.Collections.Generic;
using Pelumi.Juicer;
using Unity.VisualScripting;
using UnityEngine;

public class HintManager : Singleton<HintManager>
{
    [SerializeField] private float _waitDelay = 2.0f;
    [SerializeField] private JuicerVector3Properties _appearProperties;
    [SerializeField] private JuicerVector3Properties _disappearProperties;
    
    [field:SerializeField] public GameObject WelcomeHint { get; private set; }
    [field:SerializeField] public GameObject DrivingHint { get; private set; }
    [field:SerializeField] public GameObject CollectPackageHint { get; private set; }
    [field:SerializeField] public GameObject ShootingHint { get; private set; }
    [field:SerializeField] public GameObject BattleEnemyHint { get; private set; }
    [field:SerializeField] public GameObject GoodbyeHint { get; private set; }
    
    public IEnumerator HintRoutine(GameObject hintObject) 
    {
        hintObject.SetActive(true);
        
        yield return Juicer.DoVector3
            (null, Vector3.zero, (scale) 
                => hintObject.transform.localScale = scale, _appearProperties);
        
        yield return new WaitForSecondsRealtime(_waitDelay);
        
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                break;
            yield return null;
        }
        
        yield return Juicer.DoVector3
        (null, Vector3.one, (scale) 
            => hintObject.transform.localScale = scale, _disappearProperties);
        
        hintObject.SetActive(false);
    }
}

