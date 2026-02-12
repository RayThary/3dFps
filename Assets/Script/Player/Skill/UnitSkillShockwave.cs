using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillShockwave
{
    private UnitSkill unitSkill;   // 코루틴 실행용
    private Unit unit;             // 플레이어 정보(Transform, rigid 등)

    private float damage;
    private float coolTime;
    private float lastUsedTime = -Mathf.Infinity;

    private float slowAmount;
    private float residueDuration;
    private bool doubleShockwave = false;

    private float jumpPower = 7f;
    private float fallPower = 10f;
    private float groundCheckDistance = 0.6f;

    private float shockwaveRadius = 10f;   // 데미지 범위

    private GameObject shockwaveEffectObj;

    public void SetUp(UnitSkill _unitSkill, float _damage, float _coolTime, float _shockwaveRadius, Unit _unit)
    {
        unitSkill = _unitSkill;
        damage = _damage;
        coolTime = _coolTime;
        unit = _unit;
        shockwaveRadius = _shockwaveRadius;
    }

    public bool TryUseSkill()
    {
        if (Time.time < lastUsedTime + coolTime)
            return false;

        lastUsedTime = Time.time;
        unitSkill.StartCoroutine(ShockwaveRoutine());
        return true;
    }

    private IEnumerator ShockwaveRoutine()
    {
        Rigidbody rigid = unit.GetComponent<Rigidbody>();

        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        //중력가속 시간
        yield return new WaitForSeconds(0.3f);

        rigid.AddForce(Vector3.down * fallPower, ForceMode.Impulse);


        while (!IsGrounded())
            yield return null;

        shockwaveEffect();
        ShockwaveDamage(damage); ;


        if (doubleShockwave)
        {
            yield return new WaitForSeconds(0.2f);
            shockwaveEffect();
            ShockwaveDamage(damage);
        }

        if (residueDuration > 0f)
        {
            unitSkill.StartCoroutine(ResidueDamage(residueDuration));
        }

        yield break;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(unit.transform.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Ground"));
    }

    private void shockwaveEffect()
    {
        Vector3 unitVec = unit.transform.position;
        unitVec.y = 0.5f;
        var pooling = PoolingManager.ePoolingObject.Shockwave;
        shockwaveEffectObj = PoolingManager.Instance.CreateObject(pooling, GameManager.instance.PoolingParents[pooling.ToString()]);
        shockwaveEffectObj.transform.position = unitVec;

    }



    private void ShockwaveDamage(float _damage)
    {
        Collider[] _enemys = Physics.OverlapSphere(unit.transform.position, shockwaveRadius, LayerMask.GetMask("Enemy"));

        foreach (var _enemy in _enemys)
        {
            if (_enemy.TryGetComponent(out Enemy enemy))
            {
                enemy.HitEnemy(_damage, 1, false);

                if (slowAmount > 0)
                {
                    enemy.SlowEenemy(0.7f, slowAmount);
                }

            }
        }
    }

    private IEnumerator ResidueDamage(float duration)
    {
        float residualTime = 0f;
        while (residualTime < duration)
        {
            yield return new WaitForSeconds(0.5f);
            shockwaveEffect();
            ShockwaveDamage(damage * 0.3f);
            residualTime += 0.5f;
        }
    }

    public void ApplyUpgrade(UpgradeType type, UnitSkill.Shockwave up)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                damage *= up.damageUp; // 예: 1.2f → 20% 증가
                break;

            case UpgradeType.CoolDown:
                coolTime *= up.coolDownRate;
                if (coolTime < 0.5f) coolTime = 0.5f; // 최소값 방지
                break;

            case UpgradeType.Radius:
                shockwaveRadius += up.radiusUp; // 범위 증가
                break;

            case UpgradeType.Slow:
                slowAmount = up.slowAmount;
                break;

            case UpgradeType.ResidueShockwave:
                residueDuration += up.residueDuration;//지속피해 시간
                break;

            case UpgradeType.DoubleShockwave:
                doubleShockwave = true;
                break;
        }
    }

}
