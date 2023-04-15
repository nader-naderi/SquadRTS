
using UnityEngine;

namespace NDRSquad
{
    public interface IAttackable
    {
        GameObject gameObject { get; }
        Transform Transform { get; }
        float AttackRange { get; set; }  
        float AttackCooldown { get; set; }
        float AttackDamage { get; set; }

        bool IsAlive { get; }
        void TakeDamage(float damage);
        void Die();

        void Attack(IAttackable target);
        bool CanAttack(IAttackable target);
        bool IsInRange(IAttackable target);
        float GetAttackRange();
        float GetAttackCooldown();
    }

}
