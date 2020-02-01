using UnityEngine;

[CreateAssetMenu(fileName = "Rules", menuName = "ScriptableObjects/Game Rules", order = 2)]
public class GameRules : ScriptableObject
{
    [SerializeField] private float _startingHitGoodThreshold = 3f;
    [SerializeField] private float _startingHitLoseThreshold = 3f;
    [SerializeField] private float _startingMinTimeBetweenScenes = 1f;
    [SerializeField] private float _startingMaxTimeBetweenScenes = 5f;
    [SerializeField] private float _difficultyIncrease = 0.5f;

    public float HitGoodThreshold { get; private set; }
    public float HitLoseThreshold { get; private set; }
    public float MinTimeBetweenScenes { get; private set; }
    public float MaxTimeBetweenScenes { get; private set; }

    public void Initialize()
    {
        HitGoodThreshold = _startingHitGoodThreshold;
        HitLoseThreshold = _startingHitLoseThreshold;
        MinTimeBetweenScenes = _startingMinTimeBetweenScenes;
        MaxTimeBetweenScenes = _startingMaxTimeBetweenScenes;
    }

    public float GetTimeBetweenScenes()
    {
       var value = Random.Range(MinTimeBetweenScenes, MaxTimeBetweenScenes);
        return value;
    }

    public void IncreaseDifficulty()
    {
        HitGoodThreshold -= _difficultyIncrease;
        HitLoseThreshold -= _difficultyIncrease;
        MinTimeBetweenScenes -= _difficultyIncrease;
        MaxTimeBetweenScenes -= _difficultyIncrease;

        if (HitGoodThreshold < 0.3f) HitGoodThreshold = 0.3f;
        if (HitLoseThreshold < 1f) HitLoseThreshold = 0.7f;
        if (MinTimeBetweenScenes < 0.5f) MinTimeBetweenScenes = 0.3f;
        if (MaxTimeBetweenScenes < 1f) MaxTimeBetweenScenes = 0.5f;
    }
}
