
using System.Collections;

using UnityEngine;
using UnityEngine.AI;

namespace NDRSquad
{
    public class Unit : MonoBehaviour, IDamagable, IAttackable
    {
        [SerializeField] Animator animator;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform m_transform;

        private SquadLeader squadLeader;
        private float lastAttackTime = -Mathf.Infinity;

        public Transform Transform { get => m_transform; set => m_transform = value; }
        public NavMeshAgent Agent { get => agent; set => agent = value; }
        public GameObject Target { get; set; }
        public bool AggressiveMode { get; set; } = true;
        public bool DefensiveMode { get; set; }
        public bool HoldFire { get; set; }
        public float AttackRange { get; set; } = 20f;
        public float AttackCooldown { get; set; } = 1f;
        public float Health { get; private set; }
        public float AttackDamage { get; set; }
        public float MaxHealth { get; private set; }
        public Team Team { get; set; }
        public bool IsDead { get; private set; }

        public bool IsAlive { get; private set; }


        public void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        private void Update()
        {
            animator.SetFloat("movement", agent.velocity.magnitude / agent.speed);
            CheckForEnemiesInRange();
        }
        public void Attack(IAttackable target)
        {
            if (target == null || !CanAttack(target)) return;

            // Check if the target is within attack range
            float distance = Vector3.Distance(transform.position, target.Transform.position);
            if (distance > AttackRange)
            {
                SetDestination(target.Transform.position);
                return;
            }

            // Rotate towards the target
            Vector3 direction = target.Transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, agent.angularSpeed * Time.deltaTime);

            // Play attack animation
            animator.SetTrigger("attack");

            // Deal damage to the target after the animation finishes
            StartCoroutine(DealDamageAfterAnimationFinishes(target));
        }

        private IEnumerator DealDamageAfterAnimationFinishes(IAttackable target)
        {
            // Wait for the attack animation to finish
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

            // Deal damage to the target
            target.TakeDamage(AttackDamage);

            // Reset the attack animation
            animator.ResetTrigger("attack");

            // Update the last attack time
            lastAttackTime = Time.time;
        }

        private IEnumerator CheckForEnemiesInRange()
        {
            while (true)
            {
                if (AggressiveMode)
                {
                    Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
                    foreach (Collider hitCollider in hitColliders)
                    {
                        Unit unit = hitCollider.GetComponent<Unit>();
                        if (unit != null && unit != this && unit.transform.root != transform.root)
                        {
                            if (unit.GetComponent<Team>().IsHostile(GetComponent<Team>()))
                            {
                                // Attack the unit if it's an enemy
                                Attack(unit);
                                Debug.Log("AA");
                            }
                            else if (DefensiveMode)
                            {
                                // If in defensive mode and the unit is a friendly, move to its position
                                SetDestination(unit.transform.position);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(0.5f); // Wait for half a second before checking again
            }
        }



        public void SetSquadLeader(SquadLeader squadLeader)
        {
            this.squadLeader = squadLeader;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // Notify the squad leader that this unit died
            if (squadLeader != null)
            {
                squadLeader.RemoveUnit(this);
            }

            // Destroy this game object
            Destroy(gameObject);
        }

        public void Heal(float amount)
        {
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public bool CanAttack(IAttackable target)
        {
            bool canAttack = false;

            if (target != null && IsInRange(target) && Time.time >= lastAttackTime + AttackCooldown && !HoldFire)
            {
                canAttack = true;
            }

            return canAttack;
        }


        public bool IsInRange(IAttackable target)
        {
            if (target == null) 
                return false;

            Vector3 delta = transform.position - target.Transform.position;
            float distanceSqr = delta.sqrMagnitude;
            float rangeSqr = AttackRange * AttackRange;

            return distanceSqr <= rangeSqr;
        }

        public float GetAttackRange()
        {
            return AttackRange;
        }

        public float GetAttackCooldown()
        {
            return AttackCooldown;
        }
    }

}
