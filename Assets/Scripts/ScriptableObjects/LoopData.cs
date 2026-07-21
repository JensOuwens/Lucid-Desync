using UnityEngine;

/// <summary>
/// ToDO: Add enemy wave
/// </summary>

[CreateAssetMenu(fileName = "LoopData", menuName = "ScriptableObjects/LoopData", order = 1)]
public class LoopData : ScriptableObject
{
    public int loopNumber;
    public int enemyCount;
    //enemy type
    public EHealtRule currentHealthRule;
    public EGunRule currentGunRule;
    public EMovementRule currentMovementRule;
    public EUIType currentUIType;
    public EMusicType currentMusicType;

}
