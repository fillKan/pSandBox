using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public Rigidbody2D GetRigidbody2D;

    private IEnumerator WaitBiting;

    private bool Catch;

    private ItemExisting item;
    private ItemSlotSprt sprt;

    private void Start()
    {
        transform.GetChild(0).TryGetComponent(out sprt);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Water"))
        {
            if(WaitBiting == null)
            {
                WaitBiting = CR_waitBiting();

                StartCoroutine(WaitBiting);
            }
        }
    }

    private IEnumerator CR_waitBiting()
    {
        while (gameObject.activeSelf)
        {
            float waitTime = Random.Range(5.0f, 10.0f);

            for (float i = 0; i < waitTime; i += Time.deltaTime)
            {
                yield return null;
            }

            Vector2 InitPos = transform.position;

            Catch = true;

            for (float i = 0; i < 0.6f; i += Time.deltaTime)
            {
                transform.position = InitPos + (Random.insideUnitCircle * 0.05f);

                yield return null;
            }
            Catch = false;

            transform.position = InitPos;
        }
        yield break;
    }

    #region 함수 설명 : 
    /// <summary>
    /// 잡힌 물고기를 낚습니다. *휙!*
    /// </summary>
    #endregion
    public void CatchFish(Vector2 force)
    {
        if(Catch)
        {
            Debug.Log("Catch !");

            sprt.ShowItemExisting(Random.Range(7, 13));

            Catch = false;
        }
        GetRigidbody2D.AddForce(force);

        if (WaitBiting != null)
        {
            StopCoroutine(WaitBiting);

            WaitBiting = null;
        }     
    }

    private void OnDisable()
    {
        if (WaitBiting != null)
        {
            StopCoroutine(WaitBiting);

            WaitBiting = null;
        }

        if (sprt.ItemData != ItemMaster.ItemList.NONE)
        {
            item = ItemMaster.Instance.TakeItemExisting(sprt.ItemData);
            item.transform.position = transform.position;
            item.gameObject.SetActive(true);

            //PlayerGetter.Instance.Inventory.AddItemInventory(ItemMaster.Instance.TakeItemExisting(sprt.ItemData));

            sprt.HideItemExisting();
        }
    }
}
