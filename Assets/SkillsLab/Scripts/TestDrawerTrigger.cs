using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestDrawerTrigger : MonoBehaviour {

    private UnityAction medicineSelectedListener;

    void Awake()
    {
        medicineSelectedListener = new UnityAction(OnMedicineSelected);
    }

    void OnEnable()
    {
        EventManager.StartListening("enableDrawer", medicineSelectedListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("enableDrawer", medicineSelectedListener);
    }


    void OnMedicineSelected()
    {
        Debug.Log("OnMedicineSelected is called!");
        Managers.DrawersMan.SetActiveDrawer("tablets");
    }
}
