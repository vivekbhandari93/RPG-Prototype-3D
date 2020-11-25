using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float weaponDamage = 10f;

        Health target;
        float timeSinceLastAttack = 0;


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (!target) { return; }

            if (target.IsDead()) { return; }

            if (!GetIsCloseEnough())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }


        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if(timeSinceLastAttack > timeBetweenAttack)
            {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }


        //called on the animation
        void Hit()
        {
            if (!target) { return; }
            target.TakeDamage(weaponDamage);
        }


        private bool GetIsCloseEnough()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }


        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartActionScheduler(this);
            target = combatTarget.GetComponent<Health>();
        }


        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
    }
}
