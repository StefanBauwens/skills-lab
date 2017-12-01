using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class Managers : MonoBehaviour {

    public static PlayerManager Player { get; private set; }
    public static DrawerManager DrawersMan { get; private set; }

    private void Awake()
    {
        Player = GetComponent<PlayerManager>();
        DrawersMan = GetComponent<DrawerManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Medicine selected!");
            EventManager.TriggerEvent("enableDrawer");
        }
    }
}
