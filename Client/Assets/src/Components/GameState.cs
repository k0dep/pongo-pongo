using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    public bool IsPlayerHasAuthority;
    public float SendRate;
}