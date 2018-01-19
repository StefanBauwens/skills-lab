using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;

[Serializable]
public enum PatientType
{
    adult = 0,
    child,
    pregnant,
    senior
}

public enum Sex
{
    male = 0,
    female,
    x
}

public class Patient : SearchResult //inheritance added by stefan
{
    [XmlAttribute]
    public int mID;
    public string mName;
    public int mAge;
    public int mWeight;
    public PatientType mType;
    public Sex mSex;
    public List<string> mAllergies;

    public Patient() // == unknown patient
    {
        mID = 0;
        mName = "John Doe";

        mAge = 45;
        mWeight = 80;
        mSex = Sex.male;
        mAllergies = new List<string>();

        mType = PatientType.adult;
    }
    //Following added by stefan

    public Patient (Patient patient) //this constructor will clone another patient
    {
        this.mID = patient.mID;
        this.mName = patient.mName;
        this.mAge = patient.mAge;
        this.mWeight = patient.mWeight;
        this.mType = patient.mType;
        this.mSex = patient.mSex;
        this.mAllergies = patient.mAllergies;
    }

    public string Name //returns lastname
    {
        get{
            //mName is for example "Ho Su Yan" where "Ho" is firstname and "su Yan" is last name
            mName.Substring(mName.IndexOf(' ') + 1); //returns "su Yan" (normally) in the case of "Ho Su Yan"
            return mName;
        }
    }

    public string FirstName
    {
        get
        {
            return mName.Split(' ')[0]; //returns the first string. "Ho" in the case of "Ho Su Yan"
        }
    }

    public override string ToResult()
    {
        return string.Format("<b>Name:</b> {0}\n<b>Age:</b> {1}\t<b>Sex:</b> {2}\n<b>Weight:</b> {3}\n<b>Allergies:</b> {4}", mName, mAge, mSex.ToString(), (mWeight + "Kg"), String.Join(", ", mAllergies.ToArray()));
    }

}