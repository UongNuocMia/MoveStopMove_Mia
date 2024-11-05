using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private float speed = 20;
    private Transform target;
    private Vector3 offsetWhenPlay = new(0, 13.8f, -10.75f);
    private Vector3 rotationOffsetPlay = new(50, 0, 0);
    private Vector3 offsetWhenShop = new(0, 2.95f, 6.95f);
    private Vector3 rotationOffsetShop = new(31f, 180f, 0);
    private Vector3 offsetWhenMainMenu = new(0, 5.01f, 6.22f);
    private Vector3 rotationOffsetMainMenu = new(31f, 180f, 0);

    private Vector3 offsetPos;
    private Vector3 offsetRot;
    private Transform tf;

    private void Start()
    {
        tf = transform;
    }

    public void FindCharacter(Transform playerTransform)
    {
        target = playerTransform;
    }
    public void OnChangeOffSet(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                offsetPos = offsetWhenMainMenu;
                offsetRot = rotationOffsetMainMenu;
                break;
            case GameState.GamePlay:
                offsetPos = offsetWhenPlay;
                offsetRot = rotationOffsetPlay;
                break;
            case GameState.Shop:
                offsetPos = offsetWhenShop;
                offsetRot = rotationOffsetShop;
                break;
            default:
                break;
        }
    }
    private void LateUpdate()
    {
        if (target != null)
        {
            tf.SetPositionAndRotation(Vector3.Lerp(tf.position, target.position + offsetPos, Time.deltaTime * speed),
                Quaternion.Lerp(tf.rotation, Quaternion.Euler(offsetRot), Time.deltaTime * speed));
            Camera.main.fieldOfView = 60 + (target.localScale.x - 1) * 10;
        }
    }
}
