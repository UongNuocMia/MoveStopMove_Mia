using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private float speed = 20;
    private Transform target;
    private Vector3 offsetWhenPlay = new Vector3(0,10.54f,-7.96f);
    private Vector3 rotationOffsetPlay = new Vector3(52f, 0, 0);
    private Vector3 offsetWhenShop = new Vector3(0, 2.95f, 6.95f); 
    private Vector3 rotationOffsetShop = new Vector3(31f, 180f, 0);
    private Vector3 offsetWhenMainMenu = new Vector3(0, 5.01f, 6.22f);
    private Vector3 rotationOffSetMainMenu = new Vector3(31f, 180f, 0);

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
                rotation = rotationOffSetMainMenu;
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
            tf.position = Vector3.Lerp(tf.position, target.position + offset, Time.deltaTime * speed);
            tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.Euler(rotation), Time.deltaTime * speed);
        }
    }
}
