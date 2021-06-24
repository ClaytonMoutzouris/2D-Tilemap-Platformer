using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothFollow : MonoBehaviour
{
    public static SmoothFollow instance;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private List<PlayerController> players;
	private Vector3 _smoothDampVelocity;
	
	
	void Awake()
	{
        instance = this;

        players = new List<PlayerController>();
	}

    public void AddPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public void RemovePlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

    }


    void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
        if(players == null || players.Count <= 0)
        {
            return;
        }

        Vector3 averageVector = Vector3.zero;

        foreach(PlayerController player in players)
        {
            averageVector += player.transform.position;
        }

        averageVector /= players.Count;



		transform.position = Vector3.SmoothDamp( transform.position, averageVector - cameraOffset, ref _smoothDampVelocity, smoothDampTime );

	}
	
}
