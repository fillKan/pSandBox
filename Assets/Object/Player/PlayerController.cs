using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Player _Player;

    public void Interaction(InteractableObject target)
    {
        _Player.InteractionOrder(target);
    }
    public void OrderCancel()
    {
        _Player.OrderCancel();
    }
}
