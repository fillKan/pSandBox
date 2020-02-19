using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetter : Singleton<PlayerGetter>
{
    private Player _player;

    public Player player
    {
        get { return _player; }
    }

    public Inventory Inventory
    {
        get { return _player.Inventory; }
    }

    private void Awake()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
    }

    public Vector2 GetPos()
    {
        return player.transform.position;
    }

    public void AddInteractObj(int instanceID, Interaction interaction)
    {
        _player.AddInteractObj(instanceID, interaction);
    }

    public Dictionary<int,Interaction> GetInteractObj()
    {
        return player.InteractObj;
    }

    public void InteractCommend(int xInstanceID)
    {
        _player.FollowInstr<int>(Player.Instructions.DO_INTERACT,xInstanceID);
    }

    public void MovementCommend(Vector2 targetPoint)
    {
        _player.FollowInstr<Vector2>(Player.Instructions.GOTO_POINT, targetPoint);
    }
}
