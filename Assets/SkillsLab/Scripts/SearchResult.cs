using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchResult : MonoBehaviour {

    protected string _Name;
    protected string _Info;

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

    public SearchResult(string name, string info)
    {
        _Name = name;
        _Info = info;
    }

    public override string ToString()
    {
        return string.Format("[SearchResult: Name={0}, Info={1}]", Name, Info);
    }

    public virtual string ToResult()
    {
        return string.Format("Name: {0}\nInfo:\n{1}", Name, Info);
    }
}
