using System.Collections.Generic;

using UnityEngine;

namespace NDRSquad
{
    using UnityEngine;

    public class Team : MonoBehaviour
    {
        [SerializeField] private string teamName;
        [SerializeField] private Color teamColor;
        [SerializeField] private bool isHostileToPlayer;

        public void Init(string teamName, Color teamColor, bool isHostileToPlayer)
        {
            this.teamName = teamName;
            this.teamColor = teamColor;
            this.isHostileToPlayer = isHostileToPlayer;
        }

        public string TeamName { get => teamName; }
        public Color TeamColor { get => teamColor; }
        public bool IsHostileToPlayer { get => isHostileToPlayer; }

        public bool IsHostile(Team other)
        {
            return other != null && isHostileToPlayer != other.isHostileToPlayer;
        }
    }
}
