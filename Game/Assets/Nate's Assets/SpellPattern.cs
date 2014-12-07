using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpellPattern : MonoBehaviour
{

    
    public Conditional CurrentConditional = null;
    public List<Conditional> Conditionals = new List<Conditional>();

    void Awake()
    {
        CurrentConditional = null;
    }

    //public void copyFrom(SpellPattern sp)
    //{
    //    this.CurrentConditional.copyFrom(sp.CurrentConditional);
    //    Conditionals = new List<Conditional>();
    //    foreach (Conditional c in sp.Conditionals)
    //    {
    //        Conditionals.Add(c);
    //    }
    //    for (int i = 0; i < Conditionals.Count; i++)
    //    {
    //        Conditionals.ToArray()[i].copyFrom(sp.Conditionals.ToArray()[i]);
    //    }
    //}

}
