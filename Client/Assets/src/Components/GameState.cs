using UnityEngine;
using System;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    [SerializeField]
    private float _sendRate;
    public float SendRate
    {
        get => _sendRate;
        set => _sendRate = value;
    }

    public int BestScore { get; set; }
    
    public int CurrentScore { get; set; }

    public bool IsNetworkMatch { get; set; }

    public bool IsPlayerHasAuthority { get; set; }

    public bool HardMode { get; set; }

    public bool IsLoaded { get; set; }
}