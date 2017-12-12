using System; // Add for Action
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrawerManager : MonoBehaviour {

    public GameObject blueVanasLight;
    private Action<EventParam> medicineSelectedListener;
    private int nrOfDrawers;
    private Drawer[] drawers;
    private Drawer activeDrawer;

    void OnEnable()
    {
        EventManagerParam.StartListening(GameEvent.UNLOCK_DRAWER, medicineSelectedListener);
    }

    void OnDisable()
    {
        EventManagerParam.StopListening(GameEvent.UNLOCK_DRAWER, medicineSelectedListener);
    }

    void Awake()
    {
        // Instantiate Action<EventParam> and add a function
        medicineSelectedListener = new Action<EventParam>(OnMedicineSelected);
    }

    void Start () {
        nrOfDrawers = FindObjectsOfType<Drawer>().Length;
        drawers = new Drawer[nrOfDrawers];
        drawers = FindObjectsOfType<Drawer>();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Create parameter to pass to the event
            EventParam medicine = new EventParam();
            // Choose specific type from different params in struct and assign value
            medicine.param1 = "Ibuprofen";

            // Trigger event by passing event name and parameter
            EventManagerParam.TriggerEvent(GameEvent.UNLOCK_DRAWER, medicine);
        }
    } 

    // Returns index of drawer with the right medicine
    private byte GetCorrectDrawerIndex(string medicine)
    {
        int index = -1;
        foreach(Drawer drawer in drawers)
        {
            if(drawer.medicineInDrawer == medicine)
            {
                index = Array.IndexOf(drawers, drawer);
            }
            else
            {
                Debug.Log("Can't find drawer with specified medicine");
            }
        }
        return (byte)index;
    }

    // Get drawer with specified medicine
    private Drawer GetCorrectDrawer(string medicine)
    {       
        return drawers[GetCorrectDrawerIndex(medicine)];
    }
    
    // Called when patient and medicine is selected
    public void SetActiveDrawer(string medicine)
    {
        activeDrawer = GetCorrectDrawer(medicine);
        activeDrawer.SetGrabStatus(true);
        activeDrawer.SetRigidbodyStatus(true);
        StartCoroutine(DisableLights());
    }

    // Called when "unlockDrawer" event is triggered
    void OnMedicineSelected(EventParam medicine)
    {
        // Pass the string value from medicine param
        Managers.DrawersMan.SetActiveDrawer(medicine.param1);
    }

    private IEnumerator DisableLights() //enable not disable :P
    {
        // Enable smallLight on blueVanas
        if (activeDrawer.name.Contains("BlueDrawer"))
        {
            blueVanasLight.SetActive(true);
            activeDrawer.SetLightStatus(true);
            yield return new WaitForSeconds(10);
            blueVanasLight.SetActive(false);
        }
        else // grayVanas drawer
        {
            activeDrawer.SetLightStatus(true);
            yield return new WaitForSeconds(10);
        }

        activeDrawer.SetLightStatus(false);
    }
}
