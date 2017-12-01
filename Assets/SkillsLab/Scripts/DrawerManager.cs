using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrawerManager : MonoBehaviour {

    public byte nrOfDrawers;
    private Drawer[] drawers;
    private Drawer activeDrawer;

    // Use this for initialization
    void Start () {
        drawers = new Drawer[nrOfDrawers];
        drawers = FindObjectsOfType<Drawer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private byte GetCorrectDrawerIndex(string medicine)
    {
        int index = -1;
        int count = 0;
        foreach(Drawer drawer in drawers)
        {
            if(drawer.medicineInDrawer == medicine)
            {
                index = System.Array.IndexOf(drawers, drawer);
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
        activeDrawer.SetLightStatus(true);
        activeDrawer.SetGrabStatus(true);
        activeDrawer.SetRigidbodyStatus(true);
        //activeDrawer.SetConfigJointStatus(true);
    }
}
