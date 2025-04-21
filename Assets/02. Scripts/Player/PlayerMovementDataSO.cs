using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementDataSO", menuName = "Scriptable Objects/PlayerMovementDataSO")]
public class PlayerMovementDataSO : ScriptableObject
{
    // 속력
    public float    WalkSpeed = 7f;
    public float    RunSpeed = 12f;
    public float    RollSpeed = 20f;
    public float    ClimbSpeed = 15f;

    // 스테미나
    public float    MaxStamina = 10f;
    public float    IncreaseStaminaRate = 3f;
    public float    DecreaseStaminaRate = 1f;
    public float    RollStamina = 3f;
    public float    ClimbStaminaRate = 5f;

    // 점프
    public float    JumpPower = 10f;
    public int      MaxJumpCount = 2;
}
