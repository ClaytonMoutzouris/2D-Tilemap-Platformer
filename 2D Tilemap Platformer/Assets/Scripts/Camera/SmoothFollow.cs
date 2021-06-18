using UnityEngine;
using System.Collections;


public class SmoothFollow : MonoBehaviour
{
    public static SmoothFollow instance;
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private PhysicsBody2D _playerController;
	private Vector3 _smoothDampVelocity;
	
	
	void Awake()
	{
        instance = this;

        transform = gameObject.transform;

        if (!target)
        {
            return;
        }
		_playerController = target.GetComponent<PhysicsBody2D>();
	}

    public void SetPlayer(PlayerController player)
    {
        _playerController = player._controller;
        target = _playerController.transform;
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
        if(!target)
        {
            return;
        }

		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
	}
	
}
