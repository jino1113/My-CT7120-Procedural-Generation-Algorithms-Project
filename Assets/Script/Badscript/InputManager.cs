using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    { get { return _instance; } }

    private PlayerControl playerCon;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerCon = new PlayerControl();
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerCon.Enable();
    }

    private void OnDisable()
    {
        playerCon.Disable();
    }

    public Vector2 GetPlayerOnMove()
    {
        return playerCon.Player.OnMove.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerCon.Player.Look.ReadValue<Vector2>();
    }
}
