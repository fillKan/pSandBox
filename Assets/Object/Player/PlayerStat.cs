using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat
{
    Logging, MoveSpeed
}
[Serializable]
public class StatValuePair
{
    [Space()]
    public Stat Stat;
    public float Value;
}
public class PlayerStat : Singleton<PlayerStat>
{
    [SerializeField] private StatValuePair[] _StartStat;

    private Dictionary<Stat, float> _Storage;
    public float this[Stat stat]
    {
        get
        {
            if (_Storage.TryGetValue(stat, out float value))
            {
                return value;
            }
            _Storage.Add(stat, 0f);
            return 0f;
        }
        set
        {
            if (_Storage.ContainsKey(stat))
            {
                _Storage[stat] = value;
                return;
            }
            _Storage.Add(stat, value);
        }
    }

    private void Awake()
    {
        _Storage = new Dictionary<Stat, float>();
        for (int i = 0; i < _StartStat.Length; ++i)
        {
            var statPair = _StartStat[i];
            _Storage.Add(statPair.Stat, statPair.Value);
        }
    }
    #region public property
    public float Logging
    {
        get => _Storage[Stat.Logging];
    }
    public float MoveSpeed
    {
        get => _Storage[Stat.MoveSpeed];
    }
    #endregion
}
