using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : Singleton<MouseCursor>
{
    public Camera MainCamera;

    private SpriteRenderer targetSprite;

    public  ItemSlot  SelectSlot
    {
        get { return _selectSlot; }
    }
    private ItemSlot _selectSlot;

    #region 변수 설명 : 
    /// <summary>
    /// 현재 마우스 좌클릭으로 허공을 클릭했는지의 여부를 반환합니다.
    /// </summary>
    #endregion
    public bool ClickVoid
    {
        get 
        {
            if(Input.GetMouseButtonDown(0))
            {
                return (targetSprite == null && _selectSlot == null && CarryItem == ItemName.NONE);
            }
            return false; 
        }
    }

    #region 변수 설명 : 
    /// <summary>
    /// 현재 마우스 우클릭으로 허공을 클릭했는지의 여부를 반환합니다.
    /// </summary>
    #endregion
    public bool RightClickVoid
    {
        get
        {
            if (Input.GetMouseButtonDown(1))
            {
                return (targetSprite == null && _selectSlot == null && CarryItem == ItemName.NONE);
            }
            return false;
        }
    }

    private Stack<ItemName> _carryItems = new Stack<ItemName>();

    public ItemName CarryItem {
        get 
        {
            if (_carryItems.Count == 0) return ItemName.NONE;
            return _carryItems.Peek(); 
        } 
    }
    public ItemSlotSprt SlotSprt;

    #region 함수 설명 :
    /// <summary>
    /// 마우스로 들고있는 아이템의 갯수를 더하는 함수 
    /// </summary>
    /// <param name="item">
    ///더할 아이템
    /// </param>
    #endregion
    public void AddCarryItem(ItemName item)
    {
        if(_carryItems.Count == 0)
        {
            _carryItems.Push(item);
        }
        else if(_carryItems.Peek() == item)
        {
            _carryItems.Push(item);
        }
    }

    #region 함수 설명 :
    /// <summary>
    /// 마우스로 들고있는 아이템의 갯수를 줄이는 함수 
    /// </summary>
    /// <param name="item">
    /// 줄일 갯수
    /// </param>
    #endregion
    public void DelCarryItem(int count = 1)
    {
        for(int i = 0; i < count; ++i)
        {
            _carryItems.Pop();
        }
    }

    private void Start()
    {
        StartCoroutine(CR_update());
    }

    private IEnumerator CR_update()
    {
        while(true)
        {
            if(CarryItem != ItemName.NONE)
            {
                SlotSprt.ShowItemExisting(CarryItem);
            }
            else
            {
                SlotSprt.HideItemExisting();
            }
            if (Input.GetMouseButtonDown(0))
            {
                // 마우스로 취할 수 있는 동작이 없다면, 마우스로 클릭한 지점으로 이동한다.
                if (ClickVoid)
                {
                    PlayerGetter.Instance.MovementCommend(transform.position);
                }

                #region 아이템 슬롯에게 작용
                if (_selectSlot != null)
                {
                    _selectSlot.AddItem(CarryItem);
                }
                else if(CarryItem != ItemName.NONE)
                {
                    DroppedItem item = ItemMaster.Instance.GetDroppedItem(CarryItem);

                    item.transform.position = (Vector2)transform.position;
                    item.gameObject.SetActive(true);
                    _carryItems.Pop();
                }
                #endregion

                #region 오브젝트 상호작용 지시
                if (targetSprite != null)
                {
                    PlayerGetter.Instance.InteractCommend(targetSprite.gameObject.GetInstanceID());
                }
                #endregion

                

            }
            else if(Input.GetMouseButtonDown(1))
            {
                #region 아이템 슬롯에게 작용
                if (_selectSlot != null)
                {
                    _selectSlot.SubtractItem();
                }
                #endregion
            }

            transform.position = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition);

            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        InItemSlot(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(_selectSlot)
        {
            if(targetSprite) ExitObject();
            return;
        }

        if (Player_Interaction.Instance.InObjCheck(other.gameObject.GetInstanceID()))
        {
            if(other.TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
            {
                if (!spr.Equals(targetSprite)) EnterObject(spr);
            }         
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        OutItemSlot(other);

        if (targetSprite != null)
        {
            if (other.gameObject.Equals(targetSprite.gameObject))
            {
                ExitObject();
            }
        }
    }

    private void EnterObject(SpriteRenderer enterSpr)
    {
        if (targetSprite == null) targetSprite = enterSpr;

        else if(SortingLayer.GetLayerValueFromID(enterSpr.sortingLayerID) >= SortingLayer.GetLayerValueFromID(targetSprite.sortingLayerID))
        {
            targetSprite.color = Color.white;

            targetSprite = enterSpr;
        }
        if (enterSpr != targetSprite) return;

        targetSprite.color = new Color(0.9f, 1, 0.5f);
    }

    private void ExitObject()
    {
        targetSprite.color = Color.white;

        targetSprite = null;
    }

    #region 함수 설명 :
    /// <summary>
    /// 마우스가 작용을 받을 슬롯을 지정하였는지를 판단하여, 선택한 슬롯의 값을 조정하는 함수.
    /// </summary>
    /// <param name="collider">
    /// 지정할 오브젝트의 콜라이더
    /// </param>
    #endregion
    private void InItemSlot(Collider2D collider)
    {
        if(collider.TryGetComponent<ItemSlot>(out ItemSlot slot))
        {
            _selectSlot = slot;
        }
    }
    #region 함수 설명 :
    /// <summary>
    /// 마우스가 선택한 아이템 슬롯에서 벗어났는지를 판단하여, 선택한 슬롯의 값을 조정하는 함수.
    /// </summary>
    /// <param name="collider">
    /// 벗어난 오브젝트의 콜라이더
    /// </param>
    #endregion
    private void OutItemSlot(Collider2D collider)
    {
        if(MouseRepeater.Instance.ActionObj.ContainsKey(collider.gameObject.GetInstanceID()))
        {
            if(MouseRepeater.Instance.ActionObj[collider.gameObject.GetInstanceID()].Equals(_selectSlot))
            {
                _selectSlot = null;
            }
        }
    }
}
