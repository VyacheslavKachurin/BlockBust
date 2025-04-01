using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "ScoreReference", menuName = "ScriptableObjects/ScoreReference")]
    public class ScoreReference : ScriptableObject
    {
        public int TileMultiplier = 1;
        public int LineMultiplier = 2;
    }
}