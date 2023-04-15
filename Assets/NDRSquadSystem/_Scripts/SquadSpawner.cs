using NDRSquad;

using UnityEngine;

namespace NDRSquad
{
    public class SquadSpawner : MonoBehaviour
    {
        [SerializeField] private string teamName;
        [SerializeField] private Color teamColor;
        [SerializeField] private bool isHostileToPlayer;

        public SquadPattern squadPattern;
        SquadLeader leader;

        private void Start()
        {
            // Instantiate the squad member prefab
            Unit squadMemberPrefab = squadPattern.squadMemberPrefab;

            // Spawn the squad members
            for (int i = 0; i < squadPattern.squadSize; i++)
            {
                // Calculate the position to spawn the squad member
                Vector3 spawnPosition = transform.position + Vector3.right * (i * 3);

                // Instantiate the squad member prefab at the spawn position
                Unit squadMember = Instantiate(squadMemberPrefab, spawnPosition, 
                    Quaternion.identity);

                squadMember.GetComponent<Team>().Init(teamName, teamColor, isHostileToPlayer);
                
                if (i == 0)
                {
                    leader = squadMember.gameObject.AddComponent<SquadLeader>();
                    leader.Init(squadPattern);
                }

                leader.AddUnit(squadMember);
            }
        }

    }
}