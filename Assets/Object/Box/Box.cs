using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject
{
    [Header("ItemBox Property")]
    public GameObject ItemContainer;
    public GameObject ContainerText;

    public  Sprite sprOpen;
    private Sprite sprClosed;

    private bool _IsOpend = false;

    public override void OnActive()
    {
        base.OnActive(); 
        
        sprClosed = Renderer.sprite;
    }
    public override void Interaction()
    {
        if (_IsOpend)
        {
            Renderer.sprite = sprClosed;
            ItemContainer.SetActive(false);
            ContainerText.SetActive(false);
        }
        else
        {
            Renderer.sprite = sprOpen;
            ItemContainer.SetActive(true);
            ContainerText.SetActive(true);
        }
        _IsOpend = !_IsOpend;
    }
}
