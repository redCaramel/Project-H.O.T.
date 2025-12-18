using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelicopterController : MonoBehaviour
{
    public AudioSource HelicopterSound;
    public ControlPanel ControlPanel;
    public Rigidbody HelicopterModel;
    public HeliRotorController MainRotorController;
    public HeliRotorController SubRotorController;

    [Header("Forces")]
    public float LiftPower = 35f;
    public float MovePower = 18f;
    public float TiltPower = 3.5f;
    public float YawPower = 2.5f;

    [Header("Limits")]
    public float MaxTiltAngle = 25f;
    public float MaxEngineForce = 1.5f;
    public float HeightEffect = 200f;

    private float engineForce;         // 0~MaxEngineForce 사이
    private Vector2 moveInput;         // x=좌우, y=전후
    private float yawInput;            // 좌우회전
    private Vector2 tilt;              // 시각적 틸트

    private bool isOnGround = true;

    public float EngineForce
    {
        get { return engineForce; }
        set
        {
            engineForce = Mathf.Clamp(value, 0f, MaxEngineForce);
            MainRotorController.RotarSpeed = engineForce * 80f;
            SubRotorController.RotarSpeed = engineForce * 40f;
            HelicopterSound.pitch = Mathf.Clamp(engineForce / MaxEngineForce, 0.6f, 1.3f);
        }
    }

    void Start()
    {
        ControlPanel.KeyPressed += OnKeyPressed;
    }

    void FixedUpdate()
    {
        ApplyLift();
        ApplyMovement();
        ApplyTilt();
        ApplyYaw();
    }

    // 1. 상승력 처리
    private void ApplyLift()
    {
        float heightPercent = Mathf.Clamp01(HelicopterModel.position.y / HeightEffect);
        float liftMultiplier = 1f - Mathf.Pow(heightPercent, 1.5f);   // 고도 높을수록 힘 감소
        float lift = engineForce * LiftPower * liftMultiplier;

        HelicopterModel.AddForce(Vector3.up * lift, ForceMode.Force);
    }

    // 2. 수평 이동 처리 (힘 기반)
    private void ApplyMovement()
    {
        if (isOnGround) return;

        Vector3 forward = HelicopterModel.transform.forward * moveInput.y;
        Vector3 right = HelicopterModel.transform.right * moveInput.x;
        Vector3 desiredDir = (forward + right).normalized;

        HelicopterModel.AddForce(desiredDir * MovePower, ForceMode.Force);
    }

    // 3. 시각적 틸트 + 안정 복귀
    private void ApplyTilt()
    {
        tilt.x = Mathf.Lerp(tilt.x, moveInput.x * MaxTiltAngle, Time.deltaTime * TiltPower);
        tilt.y = Mathf.Lerp(tilt.y, moveInput.y * MaxTiltAngle, Time.deltaTime * TiltPower);

        HelicopterModel.transform.localRotation = Quaternion.Euler(tilt.y, HelicopterModel.transform.localEulerAngles.y, -tilt.x);
    }

    // 4. Yaw 회전
    private void ApplyYaw()
    {
        if (isOnGround) return;

        float yawTorque = yawInput * YawPower * HelicopterModel.mass;
        HelicopterModel.AddRelativeTorque(0f, yawTorque, 0f, ForceMode.Force);
    }

    // 키 입력 처리
    private void OnKeyPressed(PressedKeyCode[] keys)
    {
        float x = 0f;
        float y = 0f;
        float yaw = 0f;

        foreach (var k in keys)
        {
            switch (k)
            {
                case PressedKeyCode.SpeedUpPressed:
                    EngineForce += 0.05f;
                    break;

                case PressedKeyCode.SpeedDownPressed:
                    EngineForce -= 0.06f;
                    break;

                case PressedKeyCode.ForwardPressed:
                    if (!isOnGround) y += 1f;
                    break;

                case PressedKeyCode.BackPressed:
                    if (!isOnGround) y -= 1f;
                    break;

                case PressedKeyCode.LeftPressed:
                    if (!isOnGround) x -= 1f;
                    break;

                case PressedKeyCode.RightPressed:
                    if (!isOnGround) x += 1f;
                    break;

                case PressedKeyCode.TurnLeftPressed:
                    if (!isOnGround) yaw -= 1f;
                    break;

                case PressedKeyCode.TurnRightPressed:
                    if (!isOnGround) yaw += 1f;
                    break;

                case PressedKeyCode.StopProcessing:
                    SceneManager.LoadScene(0);
                    break;
            }
        }

        moveInput.x = Mathf.Lerp(moveInput.x, x, 0.2f);
        moveInput.y = Mathf.Lerp(moveInput.y, y, 0.2f);
        yawInput = Mathf.Lerp(yawInput, yaw, 0.2f);
    }

    private void OnCollisionEnter()
    {
        isOnGround = true;
    }
    private void OnCollisionExit()
    {
        isOnGround = false;
    }
}
