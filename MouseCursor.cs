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
    public  Stack<Item>  CarryItems 
    {
        get 
        {
            return _carryItems; 
        }
    }
    private Stack<Item> _carryItems = new Stack<Item>();

    public Item CarryItem {
        get 
        {
            if (_carryItems.Count == 0) return null;
            return _carryItems.Peek(); 
        } 
    }
   
    #region 함수 설명 :
    /// <summary>
    /// 마우스로 들고있는 아이템의 갯수를 더하는 함수 
    /// </summary>
    /// <param name="item">
    ///더할 아이템
    /// </param>
    #endregion
    public void AddCarryItem(Item item)
    {
        if(_carryItems.Count == 0)
        {
            _carryItems.Push(item);
        }
        else if(_carryItems.Peek().itemCode == item.itemCode)
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
            if (Input.GetMouseButtonDown(0))
            {
                // 마우스로 취할 수 있는 동작이 없다면, 마우스로 클릭한 지점으로 이동한다.
                if (targetSprite == null && _selectSlot == null && CarryItem == null)
                {
                    PlayerGetter.Instance.MovementCommend(transform.position);
                }

                #region 아이템 슬롯에게 작용
                if (_selectSlot != null)
                {
                    _selectSlot.OperateAction(0);
                }
                else if(CarryItem != null)
                {
                    _carryItems.Peek().transform.position = (Vector2)transform.position;
                    _carryItems.Peek().gameObject.SetActive(true);
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
                    _selectSlot.OperateAction(1);
                }
                #endregion
            }

            //Xtransform.position = MainCamera.WorldToScreenPoint(Input.mousePosition);
            //transform.position = MainCamera.WorldToViewportPoint(Input.mousePosition);
            //transform.position = MainCamera.ScreenToViewportPoint(Input.mousePosition);
            
            transform.position = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-MainCamera.transform.position.z));
            transform.position = new Vector3(transform.position.x, -4, transform.position.z);

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        InItemSlot(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (PlayerGetter.Instance.GetInteractObj().ContainsKey(other.gameObject.GetInstanceID()))
        {
            if(other.TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
            {
                if (!spr.Equals(targetSprite)) EnterObject(spr);
            }         
        }
    }
    private void OnTriggerExit(Collider other)
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
    private void InItemSlot(Collider collider)
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
    private void OutItemSlot(Collider collider)
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
