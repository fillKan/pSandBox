using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHandAxe : Item, IEquipItem, IUseItem
{
    [Header("OrcHandAxe Property")]
    [SerializeField] private float LoggingValue;
    [SerializeField] private Animator _Animator;
    [SerializeField] private Collider2D _AxeBlade;

    private int _AnimControlKey;
    private InteractionManager _Interaction;

    public void OnEquipItem()
    {
        _AnimControlKey = _Animator.GetParameter(0).nameHash;
        _Interaction ??= InteractionManager.Instance;
    }
    public void DisEquipItem()
    {

    }
    public void UseItem(InteractableObject target)
    {
        if (target is Tree)
        {
            _Animator.SetBool(_AnimControlKey, true);
        }
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Equip
            || itemInterface == ItemInterface.Use;
    }
    private void AE_RewindOver()
    {
        _Animator.SetBool(_AnimControlKey, false);
    }
    private void AE_AxeSwing()
    {
        MainCamera.Instance.CameraShake(0.85f, 0.35f);
        StateStorage.Instance.IncreaseState(States.TREE_LOGGING, LoggingValue);

        var filter = new ContactFilter2D();
        filter.useTriggers = true;

        var result = new List<Collider2D>();

        _AxeBlade.OverlapCollider(filter, result);
        foreach (var coll in result)
        {
            if (_Interaction.IsInteractable(coll.gameObject, out var inter))
            {
                if (inter is Tree) inter.Interaction();
            }
        }
        StateStorage.Instance.DecreaseState(States.TREE_LOGGING, LoggingValue);
    }
}
