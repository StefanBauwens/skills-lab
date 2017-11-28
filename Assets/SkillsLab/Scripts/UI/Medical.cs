using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfMedicine {
    Pill,
    EffervescentTablet,
    Tubes,
    IDKYET
}

public class Medical : MonoBehaviour {
    protected string _Name;
    protected string _Info;
    protected string _Drawer; //which drawer the medicine is located in
    protected TypeOfMedicine _Type;

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    public string Info
    {
        get { return _Info; }
        set { _Info = value; }
    }

    public string Drawer
    {
        get { return _Drawer; }
        set { _Drawer = value; }
    }

    public TypeOfMedicine Type
    {
        get { return _Type; }
        set { _Type = value; }
    }
}
