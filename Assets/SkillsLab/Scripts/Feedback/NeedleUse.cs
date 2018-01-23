using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedleOption
{
    IV,
    IM,
    SC,
    Transfer
}
public class NeedleUse : MonoBehaviour {
    //used to check what kind of injections this needle can be used for
    public NeedleOption[] CanBeUsedFor;
}
