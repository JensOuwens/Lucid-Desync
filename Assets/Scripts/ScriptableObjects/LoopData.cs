using UnityEngine;

/// <summary>
/// ToDO: Maybe add more variables?
/// </summary>

[CreateAssetMenu(fileName = "LoopData", menuName = "ScriptableObjects/LoopData", order = 1)]
public class LoopData : ScriptableObject
{
    public int loopNumber;
    public int enemyCount;
    public WaveData[] enemyData;
    public EHealtRule currentHealthRule;
    public EGunRule currentGunRule;
    public EMovementRule currentMovementRule;
    public EGravityType currentGravityType;
    public EUIType currentUIType;
    public EMusicType currentMusicType;

}
