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

    private void Awake()
    {
        _player = FindObjectOfType(typeof(Player)) as Player;
    }

    public Vector2 GetPos()
    {
        return player.transform.position;
    }

    public void AddInteractObj(Interaction interaction)
    {
        player.AddInteractObj(interaction);
    }

}
