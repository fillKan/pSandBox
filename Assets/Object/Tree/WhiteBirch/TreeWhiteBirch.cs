using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWhiteBirch : Tree, IInteraction
{
    public GameObject InteractObject()
    {
        return gameObject;
    }
    public void OperateAction<T>(T xValue) where T : IItemFunction
    {
        if (xValue == null && !_doingChopTree)
        {
            StartCoroutine(CR_logging());

            Debug.Log(xValue);
        }

        else StartCoroutine(xValue.UseItem(this));
    }

    public void RegisterInteraction()
    {
        Player_Interaction.Instance.InObjRegister(gameObject.GetInstanceID(), this);
    }


    public override void DropItem()
    {
        int repeat = Random.Range(8, 14);

        DroppedItem tItem;

        for (int i = 0; i < repeat; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                 tItem = ItemMaster.Instance.TakeItemExisting(ItemName.SEED_WHITEBIRCH);
            }
            else tItem = ItemMaster.Instance.TakeItemExisting(ItemName.LOG_WHITEBIRCH);

            tItem.gameObject.SetActive(true);
            tItem.transform.position = transform.position;
        }
    }

    protected override void InitTree()
    {
        _fDurability = 20;

        RegisterInteraction();

        TryGetComponent<SpriteRenderer>(out _sprtRenderer);
    }

    private IEnumerator CR_logging()
    {
        yield return StartCoroutine(CR_chopTree(StateStorage.Instance.TreeLogging, 0.4f, 0.06f));

        if(_fDurability <= 0)
        {
            DropItem();

            yield return StartCoroutine(CR_cutDown());
        }

        yield break;
    }
}
