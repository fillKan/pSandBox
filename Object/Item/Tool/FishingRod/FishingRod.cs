using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item, ItemFunction
{
    [Tooltip("낚시찌 오브젝트")]
    public Bobber bobber;

                      public Sprite UnTiedSprt;
    [HideInInspector] public Sprite   TiedSprt;
    [HideInInspector] public SpriteRenderer Renderer;

    private void Reset()
    {
        TryGetComponent<SpriteRenderer>(out Renderer);

        TiedSprt = Renderer.sprite;

        Debug.Log(Renderer);
        Debug.Log(TiedSprt);
    }

    public IEnumerator CarryItem()
    {
        if (!StartWorking(ref _isCarryItem)) yield break;

        if(MouseCursor.Instance.ClickVoid)
        {
            Debug.Log(Renderer.sprite.Equals(TiedSprt));
            
            if(Renderer.sprite.Equals(TiedSprt))
            {
                Renderer.sprite = UnTiedSprt;
            }
            else
            {
                Renderer.sprite = TiedSprt;
            }
        }

        StopWorking(ref _isCarryItem);
        yield break;
    }

    public IEnumerator EquipItem()
    {
        if (!StartWorking(ref _isEquipItem)) yield break;

        // TODO . . .

        StopWorking(ref _isEquipItem);
        yield break;
    }

    public IEnumerator InSlotItem()
    {
        if (!StartWorking(ref _isInSlotItem)) yield break;

        // TODO . . .

        StopWorking(ref _isInSlotItem);
        yield break;
    }

    public IEnumerator UseItem<T>(T xValue) where T : Interaction
    {
        if (!StartWorking(ref _isUseItem)) yield break;

        // TODO . . .

        StopWorking(ref _isUseItem);
        yield break;
    }

    protected override void Init()
    {
        _itemCode = (int)ItemMaster.ItemList.FISHING_ROD;
    }
}
