using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : ObjectEntity, IInteractable
{

    public Portal otherPortal;
    Entity owner;

    public float warpDelay = 0.2f;
    public ParticleSystem warpOutVisual;
    public ParticleSystem warpInVisual;

    public static Dictionary<Entity, List<Portal>> portals = new Dictionary<Entity, List<Portal>>();

    public void SetOwner(Entity entity)
    {
        owner = entity;

        if(portals.ContainsKey(owner))
        {
            portals[owner].Add(this);

            Portal previous = portals[owner][0];

            for(int i = portals[owner].Count - 1; i >= 0; i--)
            {
                portals[owner][i].SetOtherPortal(previous);
                previous = portals[owner][i];
            }
        } else
        {
            portals.Add(owner, new List<Portal> { this });
        }
    }

    public void Interact(Entity entity)
    {
        if(otherPortal)
        {
            Debug.Log("Warping Entity");
            entity.StartCoroutine(WarpEntity(entity));
        }
    }

    public IEnumerator WarpEntity(Entity entity)
    {
        float timestamp = Time.time;
        ParticleSystem origin = Instantiate(warpOutVisual, entity.transform.position, Quaternion.identity);

        while (Time.time < timestamp + warpDelay)
        {
            yield return null;
        }

        entity.transform.position = otherPortal.transform.position;
        ParticleSystem destination = Instantiate(warpInVisual, entity.transform.position, Quaternion.identity);



    }

    public void SetOtherPortal(Portal portal)
    {
        otherPortal = portal;
    }

    public Portal GetOtherPortal()
    {
        return otherPortal;
    }

    public void OnDestroy()
    {
        if (portals.ContainsKey(owner))
        {
            portals[owner].Remove(this);

            if(portals[owner].Count > 0)
            {
                Portal previous = portals[owner][0];

                for (int i = portals[owner].Count - 1; i >= 0; i--)
                {
                    portals[owner][i].SetOtherPortal(previous);
                    previous = portals[owner][i];
                }
            }
        }
    }
}
