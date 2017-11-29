using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : SearchResult
{
    protected string _FirstName;
    protected int _Age;
    protected bool _IsMale;

    public string FirstName
    {
        get { return _FirstName; }
        set { _FirstName = value; }
    }

    public int Age
    {
        get { return _Age; }
        set { _Age = value; }
    }

    public bool IsMale
    {
        get { return _IsMale; }
        set { _IsMale = value; }
    }

    public Patient(string name, string firstname, string info, int age, bool isMale):base(name, info)
    {
        _FirstName = firstname;
        _Age = age;
        IsMale = isMale;
    }

}