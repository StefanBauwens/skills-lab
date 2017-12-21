using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System;
using System.IO;

public class MedicalAppData  {

    public List<Patient> mPatients;
    public List<Medicine> mMedicines;
    public List<DeliveryTool> mTools;
    public List<DeliveryMethod> mMethods;
    public List<CabinetDrawer> mDrawers;
    public List<Cabinet> mCabinets;
    public List<Scenario> mScenarios;
    public MetaData mMetaData;

    public MedicalAppData()
    {
        mMetaData = new MetaData();
        mPatients = new List<Patient>() ;
        mMedicines = new List<Medicine>() ;
        mTools = new List<DeliveryTool>();
        mMethods = new List<DeliveryMethod>() ;
        mDrawers = new List<CabinetDrawer>();
        mCabinets = new List<Cabinet>();
        mScenarios = new List<Scenario>();
    }

    public static bool WriteToFile(ref MedicalAppData mediData, string filename)
    {
        bool succeeded = false;
        StreamWriter writer = null;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MedicalAppData));
            writer = new StreamWriter(filename);
            serializer.Serialize(writer, mediData);
            succeeded = true;
        }
        catch (Exception e)
        {
            Debug.Log("Exception writing XML: " + e.Message);
            succeeded = false;

        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
            }
        }
        return succeeded;
    }

    public static bool ReadFromFile(string filename, out MedicalAppData result)
    {
        bool succeeded = false;
        result = null;
        StreamReader reader = null;
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MedicalAppData));
            reader = new StreamReader(filename);
            result = (MedicalAppData)serializer.Deserialize(reader);
            succeeded = true;
        }
        catch (Exception e)
        {
            Debug.Log("Exception reading XML: " + e.Message);
            succeeded = false;

        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
        }
        return succeeded;
    }


}
