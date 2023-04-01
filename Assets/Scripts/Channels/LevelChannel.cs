using Assets.Scripts.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelChannel",  menuName = "BlockPlatformer/Channels/LevelChannel")]
public class LevelChannel : ScriptableObject
{
    public readonly GameEvent ExitOpen = new("DoorOpen");
    public readonly GameEvent LevelExit = new("LevelExit");
    public readonly GameEvent PlayerDied = new("PlayerDied");
}
