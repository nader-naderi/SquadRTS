using System.Collections.Generic;

using UnityEngine;

namespace NDRSquad
{
    public class SquadLeader : MonoBehaviour
    {

        private List<Unit> units = new List<Unit>();
        private SquadPattern pattern;
        [SerializeField] float spread = 2;
        public void Init(SquadPattern pattern) => this.pattern = pattern;

        public void AddUnit(Unit unit)
        {
            if (units.Count < pattern.squadSize)
            {
                units.Add(unit);
                unit.SetSquadLeader(this);
            }
        }

        public void RemoveUnit(Unit unit)
        {
            if (units.Contains(unit))
            {
                units.Remove(unit);
                unit.transform.parent = null;
            }
        }

        public void Move(Vector3 destination, float spread = 2f)
        {
            Vector3 squadCenter = GetSquadCenter();
            Vector3 squadDirection = (destination - squadCenter).normalized;

            for (int i = 0; i < units.Count; i++)
            {
                float offsetAngle = Random.Range(-spread, spread) + i * spread * 2 / units.Count;
                Quaternion offsetRotation = Quaternion.AngleAxis(offsetAngle, Vector3.up);
                Vector3 offsetDirection = offsetRotation * squadDirection;
                Vector3 unitDestination = destination + offsetDirection * pattern.spacing * i;
                units[i].SetDestination(unitDestination);
            }
        }

        private Vector3 GetSquadCenter()
        {
            Vector3 center = Vector3.zero;
            foreach (Unit unit in units)
            {
                center += unit.transform.position;
            }
            center /= units.Count;
            return center;
        }



        public void Attack(Unit target)
        {
            foreach (Unit unit in units)
            {
                unit.Attack(target);
            }
        }
        public void SetTarget(GameObject target)
        {
            foreach (Unit unit in units)
            {
                unit.Target = target;
            }
        }

        public void SetAggressiveMode(bool value)
        {
            foreach (Unit unit in units)
            {
                unit.AggressiveMode = value;
            }
        }

        public void SetDefensiveMode(bool value)
        {
            foreach (Unit unit in units)
            {
                unit.DefensiveMode = value;
            }
        }

        public void SetHoldFire(bool value)
        {
            foreach (Unit unit in units)
            {
                unit.HoldFire = value;
            }
        }
    }
}

