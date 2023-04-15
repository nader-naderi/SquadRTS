namespace NDRSquad
{
    public interface IDamagable
    {
        float Health { get; }
        float MaxHealth { get; }
        bool IsDead { get; }
        void TakeDamage(float damage);
        void Die();
        void Heal(float amount);
    }

}
