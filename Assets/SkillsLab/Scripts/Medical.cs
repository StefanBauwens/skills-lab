using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfMedicine
{
    Pill,
    EffervescentTablet,
    Tubes,
    IDKYET
}

public class Medical : SearchResult
{
    protected string _Drawer; //which drawer the medicine is located in
    protected TypeOfMedicine _Type;

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

    public Medical(string name, string info, string drawer, TypeOfMedicine typeM):base(name, info)
    {
        Drawer = drawer;
        Type = typeM;
    }
}