using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : SearchResult
{
    protected string _FirstName;
    protected int _Age;
    protected bool _IsMale;
    protected float _Weight;
    protected string _Allergies;
    protected string _Complications;
    protected string _CurrentMedicine;
    protected string _PreviousDosages;
    protected string _Prescription;


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

    public float Weight
    {
        get { return _Weight; }
        set { _Weight = value; }
    }

    public string Allergies
    {
        get { return _Allergies; }
        set { _Allergies = value; }
    }

    public string Complications
    {
        get { return _Complications; }
        set { _Complications = value; }
    }

    public string CurrentMedicine
    {
        get { return _CurrentMedicine; }
        set { _CurrentMedicine = value; }
    }

    public string PreviousDosages
    {
        get { return _PreviousDosages; }
        set { _PreviousDosages = value; }
    }

    public string Prescription
    {
        get { return _Prescription; }
        set { _Prescription = value; }
    }


    public Patient(string name, string firstname, string info, int age, bool isMale, float weight, string allergies, string complications, string currentMedicine, string previousDosages, string prescription):base(name, info)
    {
        _FirstName = firstname;
        _Age = age;
        _IsMale = isMale;
        _Weight = weight;
        _Allergies = allergies;
        _Complications = complications;
        _CurrentMedicine = currentMedicine;
        _PreviousDosages = previousDosages;
        _Prescription = prescription;
    }

    public override string ToString() //not finished, but not used so not really neccesary
    {
        return string.Format("[Patient: FirstName={0}, Age={1}, IsMale={2}, Name={3}, Info={4}]", FirstName, Age, IsMale, Name, Info);
    }

    public override string ToResult()
    {
        return string.Format("<b>Name:</b> {0}\t<b>FirstName:</b> {1}\n<b>Age:</b> {2}\t<b>Sex:</b> {3}\n<b>Weight:</b> {4}\n<b>Allergies:</b> {5}\n<b>Complications:</b> {6}\n<b>Current medicine:</b> {7}\n<b>Previous dosages:</b> {8}\n<b>Prescription:</b> {9}\n<b>Info:</b>\n{10}", Name, FirstName, Age, IsMale ? "M" : "F", (Weight + "Kg"), Allergies, Complications, CurrentMedicine, PreviousDosages, Prescription, Info);
    }

}