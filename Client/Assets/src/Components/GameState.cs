using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    public bool IsNetworkMatch;
    public bool IsPlayerHasAuthority;
    public float SendRate;
}