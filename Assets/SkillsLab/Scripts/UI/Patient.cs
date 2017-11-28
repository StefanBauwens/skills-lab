using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : MonoBehaviour {
    protected string _FirstName;
    protected string _LastName;
    protected int _Age;
    protected bool _IsMale;
    protected string _Info;

    public string FirstName {
        get { return _FirstName; }
        set { _FirstName = value; }
    }

    public string LastName {
        get { return _LastName; }
        set { _LastName = value; }
    }

    public int Age {
        get { return _Age; }
        set { _Age = value; }
    }

    public bool IsMale {
        get { return _IsMale; }
        set { _IsMale = value; }
    }

    public string Info {
        get { return _Info; }
        set { _Info = value; }
    }


}
