using UnityEngine;

namespace NDRSquad
{
    [CreateAssetMenu(fileName = "New Squad", menuName = "NDRSquad/New Squad")]
    public class SquadPattern : ScriptableObject
    {
        public new string name;
        public Sprite squadPortrait;
        public Unit squadMemberPrefab;
        public int squadSize = 4;
        public int damageModifier = 10;
        public int moraleModifier = 50;
        public float moveDistance = 10f;
        public float spacing = 3f;
    }
}
