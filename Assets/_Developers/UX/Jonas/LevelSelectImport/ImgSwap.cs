using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pelumi.Juicer;
using UnityEngine.UI;

public class ImgSwap : MonoBehaviour
{
    [SerializeField] private AdvanceButton CurrentImg;
    [SerializeField] private Image ImageR;
    [SerializeField] private Image ImageL;

    [SerializeField] public Sprite[] Images;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        CurrentImg = GetComponent<AdvanceButton>();
        ChangeImage(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void ChangeImage(int imageNum)
    {
        CurrentImg.image.sprite = Images[imageNum];
        ImageR.sprite = Images[imageNum+1 < Images.Length ? imageNum+1 : 0];
        ImageL.sprite = Images[imageNum-1 >= 0 ? imageNum-1 : Images.Length - 1];


    }
}
