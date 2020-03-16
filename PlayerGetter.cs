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

    public void InteractCommend(int xInstanceID)
    {
        Player_Instructions.Instance.FollowInstr(Instructions.DO_INTERACT, xInstanceID);
    }

    public void MovementCommend(Vector2 targetPoint)
    {
        Player_Instructions.Instance.FollowInstr(Instructions.GOTO_POINT, targetPoint);
    }
}
