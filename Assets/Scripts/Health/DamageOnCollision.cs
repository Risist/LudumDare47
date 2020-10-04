using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    /// To prevent multiplay hit counts per attack this list will keep track of targets already damaged
    /// So they wont be damaged anymore unless next attack sequence occurs
    List<IDamageable> damaged = new List<IDamageable>();
    public LayerMask requiredMask = ~0;

    public DamageData damageDataOnce;
    public DamageData damageDataContinous;
    public DamageData damageDataEnter;
    public DamageData damageDataImpact;

    Vector2 lastPosition;
    private void FixedUpdate()
    {
        lastPosition = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enabled)
            return;

        if ((requiredMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        var damagable = other.GetComponent<IDamageable>();
        damageDataEnter.position = transform.position;
        damageDataContinous.position = transform.position;
        damageDataOnce.position = transform.position;

        if (damagable != null && other.transform.root != transform.root)
        {
            damagable.DealDamage(damageDataContinous);
            if (AttemptToDamage(damagable))
                damagable.DealDamage(damageDataOnce);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;

        if ((requiredMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        var damagable = other.GetComponent<IDamageable>();
        damageDataEnter.position = transform.position;
        damageDataContinous.position = transform.position;
        damageDataOnce.position = transform.position;

        if (damagable != null && other.transform.root != transform.root)
        {
            damagable.DealDamage(damageDataEnter);
            if (AttemptToDamage(damagable))
                damagable.DealDamage(damageDataOnce);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!enabled)
            return;

        if ((requiredMask.value & (1 << collision.gameObject.layer)) == 0)
            return;

        var damagable = collision.gameObject.GetComponent<IDamageable>();
        if (damagable == null)
        {
            damagable = collision.gameObject.GetComponentInParent<IDamageable>();
        }
        damageDataEnter.position = transform.position;
        damageDataContinous.position = transform.position;
        damageDataOnce.position = transform.position;
        
        if (damagable != null && collision.gameObject.transform.root != transform.root)
        {
            DamageData damageDataImpactTemp = new DamageData();
            damageDataImpactTemp.damage = damageDataImpact.damage * collision.relativeVelocity.magnitude + damageDataContinous.damage;
            damageDataImpactTemp.staggerIncrease = damageDataImpact.staggerIncrease * collision.relativeVelocity.magnitude + damageDataContinous.staggerIncrease;
            damageDataImpactTemp.position = transform.position;
            damageDataImpactTemp.direction = collision.GetContact(0).normal;

            damagable.DealDamage(damageDataImpactTemp);

            if (AttemptToDamage(damagable))
                damagable.DealDamage(damageDataOnce);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled)
            return;

        if ((requiredMask.value & (1 << collision.gameObject.layer)) == 0)
            return;

        var damagable = collision.gameObject.GetComponent<IDamageable>();
        if (damagable == null)
        {
            damagable = collision.gameObject.GetComponentInParent<IDamageable>();
        }
        damageDataEnter.position = transform.position;
        damageDataContinous.position = transform.position;
        damageDataOnce.position = transform.position;
        if (damagable != null && collision.gameObject.transform.root != transform.root)
        {
            DamageData damageDataImpactTemp = new DamageData();
            damageDataImpactTemp.damage = damageDataImpact.damage * collision.relativeVelocity.magnitude + damageDataContinous.damage;
            damageDataImpactTemp.staggerIncrease = damageDataImpact.staggerIncrease * collision.relativeVelocity.magnitude + damageDataContinous.staggerIncrease;
            damageDataImpactTemp.position = transform.position;
            damageDataImpactTemp.direction = collision.GetContact(0).normal;

            damagable.DealDamage(damageDataImpactTemp);
            if (AttemptToDamage(damagable))
                damagable.DealDamage(damageDataOnce);
        }
    }

    /// tells if it is possible to deal damage to target
    /// and records that damage had meed done
    public bool AttemptToDamage(IDamageable target)
    {
        foreach (var it in damaged)
            if (it == target)
                return false;
        damaged.Add(target);
        return true;
    }

    private void OnEnable()
    {
        damaged.Clear();
    }
}
