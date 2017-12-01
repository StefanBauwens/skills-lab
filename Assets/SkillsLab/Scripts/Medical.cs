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
    protected int _Quantity;

    public int Quantity
    {
        get { return _Quantity; }
        set { _Quantity = value; }
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



    public Medical(string name, string info, string drawer, TypeOfMedicine typeM, int quantity):base(name, info)
    {
        Drawer = drawer;
        Type = typeM;
        _Quantity = quantity;
    }

    public override string ToString()
    {
        return string.Format("[Medical: Name={0}, Drawer={1}, Type={2}, Info={3}, Quantity={4}]", Name, Drawer, Type, Info, Quantity);
    }

    public override string ToResult()
    {
        return string.Format("<b>Name:</b> {0}\n<b>Type:</b> {1}\n<b>Drawer:</b> {2}\t<b>Quantity:</b> {3}\n<b>Info:</b>\n{4}", Name, Type.ToString(), Drawer, Quantity, Info);
    }
}