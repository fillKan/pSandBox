using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, Interaction
{
    public  GameObject ItemContainer;
    public  GameObject ContainerText;

    public  Sprite sprOpen;
    private Sprite sprClosed;

    private SpriteRenderer Renderer;

    private void Start()
    {
        RegisterInteraction();

        Renderer  = GetComponent<SpriteRenderer>();
        sprClosed = Renderer.sprite;
    }

    public GameObject InteractObject()
    {
        return gameObject;
    }

    public void OperateAction<T>(T xValue) where T : ItemFunction
    {
        if(Renderer.sprite.Equals(sprClosed))
        {
            Renderer.sprite = sprOpen;
            ItemContainer.SetActive(true);
            ContainerText.SetActive(true);
        }
        else
        {
            Renderer.sprite = sprClosed;
            ItemContainer.SetActive(false);
            ContainerText.SetActive(false);
        }
    }

    public void RegisterInteraction()
    {
        Player_Interaction.Instance.InObjRegister(gameObject.GetInstanceID(), this);
    }
}
