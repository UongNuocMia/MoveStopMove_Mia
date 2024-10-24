using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private float speed = 20;
    private Transform target;
    private Vector3 offsetWhenPlay = new(0, 11.67f, -11.18f);
    private Vector3 rotationOffsetPlay = new(44.7f, 0, 0);
    private Vector3 offsetWhenShop = new(0, 2.95f, 6.95f);
    private Vector3 rotationOffsetShop = new(31f, 180f, 0);
    private Vector3 offsetWhenMainMenu = new(0, 5.01f, 6.22f);
    private Vector3 rotationOffsetMainMenu = new(31f, 180f, 0);

    private Vector3 offset;
    private Vector3 rotation;
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
                offset = offsetWhenMainMenu;
                rotation = rotationOffsetMainMenu;
                break;
            case GameState.GamePlay:
                offset = offsetWhenPlay;
                rotation = rotationOffsetPlay;
                break;
            case GameState.Shop:
                offset = offsetWhenShop;
                rotation = rotationOffsetShop;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (target != null)
        {
            tf.SetPositionAndRotation(Vector3.Lerp(tf.position, target.position + offset, Time.deltaTime * speed),
                Quaternion.Lerp(tf.rotation, Quaternion.Euler(rotation), Time.deltaTime * speed));
        }
    }
}
