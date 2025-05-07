using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("IK 손 Transform")]
    public Transform                  RightHandPosition;
    public Transform                  LeftHandPosition;

    private Animator                  _animator;
    private int                       _layerIndexShot;
    private int                       _layerIndexInjure;

    [Header("무기별 애니메이터 오버라이드")]
    public AnimatorOverrideController GunAnimatorOverride;
    public AnimatorOverrideController SwordAnimatorOverride;
    public AnimatorOverrideController GrenadeAnimatorOverride;
    private WeaponType                _currentWeaponType;

    public Action                     OnFireAnimationEnd;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _layerIndexShot = _animator.GetLayerIndex("Shot Layer");
        _layerIndexInjure = _animator.GetLayerIndex("Injure Layer");
    }

    // 이동 입력 전달
    public void SetMoveInput(float moveForward, float moveRight)
    {
        _animator.SetFloat("MoveForwardAmount", moveForward);
        _animator.SetFloat("MoveRightAmount", moveRight);
    }

    // 총 발사 트리거
    public void PlayShotAnimation()
    {
        _animator.SetTrigger("Shot");
    }

    public void SetInjureLayerWeight(float currentHealth, float maxHealth)
    {
        _animator.SetLayerWeight(_layerIndexInjure, (maxHealth - currentHealth) / maxHealth);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_currentWeaponType != WeaponType.Gun)
        {
            return;
        }

        if (layerIndex != _layerIndexShot)
        {
            return;
        }

        // 가중치 적용
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);


        _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPosition.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandPosition.rotation);

        _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandPosition.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandPosition.rotation);
    }

    public void ChangeWeaponAnimation(WeaponType weaponType)
    {
        _currentWeaponType = weaponType;
        switch (weaponType)
        {
            case WeaponType.Gun:
                _animator.runtimeAnimatorController = GunAnimatorOverride;
                break;
            case WeaponType.Knife:
                _animator.runtimeAnimatorController = SwordAnimatorOverride;
                break;
            case WeaponType.Bomb:
                _animator.runtimeAnimatorController = GrenadeAnimatorOverride;
                break;
        }
    }

    public void OnKnfieFireAnimationEnd()
    {
        OnFireAnimationEnd?.Invoke();
    }
}
