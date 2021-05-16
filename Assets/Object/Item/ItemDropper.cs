using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropTablePage
{
    [Space()] public ItemName Name;
    [Range(0f, 1f)] public float Probablity;

    [Space()]
    [Min(0)] public int DropMin;
    [Min(0)] public int DropMax;
}

public class ItemDropper : MonoBehaviour
{
    public DropTablePage[] DropTable;
    private ItemMaster _ItemMaster;

    public void DropItem(float probablity, Action<DroppedItem> dropAction = null)
    {
        for (int i = DropTable.Length - 1; i >= 0; --i)
        {
            var page = DropTable[i];

            if (probablity <= page.Probablity)
            {
                LazyInit();
                int count = UnityEngine.Random.Range(page.DropMin, page.DropMax);

                for (int j = 0; j < count; j++)
                {
                    var drop = _ItemMaster.GetDroppedItem(page.Name);
                    drop.transform.position = transform.position;

                    dropAction?.Invoke(drop);
                }
            }
        }
    }
    private void LazyInit()
    {
        _ItemMaster ??= ItemMaster.Instance;
    }
}
