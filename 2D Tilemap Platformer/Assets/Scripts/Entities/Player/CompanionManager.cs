using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{

    List<Companion> companions = new List<Companion>();

    public List<Companion> Companions { get => companions; set => companions = value; }

    public void AddCompanion(Companion companion)
    {

        Companions.Add(companion);
        UpdateCompanionIndexes();

    }

    public void RemoveCompanion(Companion companion)
    {
        if (Companions.Contains(companion))
        {
            Companions.Remove(companion);
            UpdateCompanionIndexes();
        }
    }

    public void UpdateCompanionIndexes()
    {
        for(int i = 0; i < Companions.Count; i++)
        {
            Companions[i].companionIndex = i;
        }
    }

}
