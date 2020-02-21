using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 클래스 설명 : 
/// <summary>
/// 플레이어가 들고있는 아이템의 스프라이트를 띄우고, 컨트롤하는 클래스.
/// </summary>
#endregion
public class PlayerCarryItem : MonoBehaviour
{
    public ItemSlotSprt SlotSprt;
    public ItemSlot CarryItemSlot;

    #region 함수 설명 :
    /// <summary>
    /// 들고있는 아이템 스프라이트의 정보를 업데이트하는 함수.
    /// <para>
    /// 이 함수를 통해 어떤 아이템이 보여질 것 인지, 그 아이템의 방향은 어떠한지를 설정한다.
    /// </para>
    /// </summary>
    /// <param name="playerFlipX">
    /// 플레이어 스프라이트의 flipX값. 이 값을 통해 들고있는 아이템의 방향을 설정한다.
    /// </param>
    #endregion
    public void Patch(bool playerFlipX)
    {
        // 들고있는 아이템이 없다면 스프라이트를 띄우지 않는다.
        if(CarryItemSlot.ContainItem == null)
        {
            SlotSprt.HideItemSprt();
        }

        else
        {
            // 플레이어의 flipX값과 자신의 flipX값이 다르다면, 플레이어의 flipX값에 맞춘다.
            if (SlotSprt.Renderer.flipX != playerFlipX)
            {
                SlotSprt.Renderer.flipX  = playerFlipX;

                // 플레이어의 flipX값에 따라 스프라이트의 방향을 맞춘다.
                if (playerFlipX)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, -22.5f);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, 0,  22.5f);
                }
            }
            
            // 현재 띄워지고 있는 스프라이트가 없다면?
            if(SlotSprt.Renderer.sprite == null)
            {
                SlotSprt.ShowItemSprt(CarryItemSlot.ContainItem.itemCode);
            }

            // 현재 띄워지고 있는 스프라이트와 들고있는 아이템의 스프라이트를 비교하는 이유는,
            // ShowItemSprt함수의 중복 실행을 막기 위해서다. 그런데 만약 비교하는 이 과정이 더 소모가 심하다면..

            else if (!SlotSprt.Renderer.sprite.Equals(CarryItemSlot.SlotSprt.Renderer.sprite))
            {
                SlotSprt.ShowItemSprt(CarryItemSlot.ContainItem.itemCode);
            }

        }
    }
}
