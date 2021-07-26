using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcHandAxe : Item, IUseItem
{
    [Header("OrcHandAxe Property")]
    [SerializeField] private float LoggingValue;
    [SerializeField] private Animator _Animator;
    [SerializeField] private Collider2D _AxeBlade;

    private bool _IsAlreadyInit = false;

    private int _AnimControlKey;

    private ContactFilter2D _ContactFilter;
    private List<Collider2D> _ContactList;

    private delegate bool IsInteractable(GameObject @object, out InteractableObject interactableObject);
    private IsInteractable InteractableCheck;

    public void UseItem(InteractableObject target)
    {
        if (!_IsAlreadyInit)
        {
            _AnimControlKey = _Animator.GetParameter(0).nameHash;
            InteractableCheck = InteractionManager.Instance.IsInteractable;

            _ContactFilter = new ContactFilter2D
            {
                useTriggers = true
            };
            _ContactList = new List<Collider2D>();

            _IsAlreadyInit = true;
        }
        if (target is Tree)
        {
            _Animator.SetBool(_AnimControlKey, true);
        }
    }
    public override bool IsUsing(ItemInterface itemInterface)
    {
        return itemInterface == ItemInterface.Use;
    }
    private void AE_RewindOver()
    {
        _Animator.SetBool(_AnimControlKey, false);
    }
    private void AE_AxeSwing()
    {
        MainCamera.Instance.CameraShake(0.85f, 0.35f);
        PlayerStat.Instance[Stat.Logging] += LoggingValue;

        _AxeBlade.OverlapCollider(_ContactFilter, _ContactList);
        foreach (var coll in _ContactList)
        {
            if (InteractableCheck(coll.gameObject, out var inter))
            {
                if (inter is Tree) inter.Interaction();
            }
        }
        _ContactList.Clear();
        PlayerStat.Instance[Stat.Logging] -= LoggingValue;
    }
}
