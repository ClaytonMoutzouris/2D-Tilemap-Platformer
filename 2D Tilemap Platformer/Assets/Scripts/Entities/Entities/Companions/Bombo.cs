using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BomboState { Moving, Exploding };
public class Bombo : Companion
{
    [Header("Bombo")]


    [Header("Attack Info")]
    public ProjectileData projectile;
    public AttackData attackData;

    [Header("Stats")]
    public float movementSpeed = 3.0f;
    public float lifeTime = 3;

    public float interval = 0.5f;
    public float variance = 0.5f;

    public int blowupCount = 5;

    public BomboState currentState = BomboState.Moving;


    protected override void Awake()
    {
        base.Awake();

    }

    public void Start()
    {
        StartCoroutine(BomboFSM());

    }

    public override void SetOwner(PlayerController player)
    {
        base.SetOwner(player);

        SetDirection((EntityDirection)owner.GetDirection());
    }


    IEnumerator BomboFSM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    public IEnumerator Moving()
    {
        float timeStamp = Time.time;

        while(currentState == BomboState.Moving)
        {

            _controller.velocity.x = movementSpeed*GetDirection();
            _controller.move();


            if (Time.time > timeStamp + lifeTime)
            {
                currentState = BomboState.Exploding;
            }


            yield return null;

        }

    }

    public IEnumerator Exploding()
    {
        float timeStamp = Time.time;
        float count = 0;
        _controller.velocity.x = 0;

        while (currentState == BomboState.Exploding)
        {

            _controller.move();


            if (Time.time > timeStamp + interval)
            {

                
                Projectile proj = Instantiate(projectile.projectileBase, transform.position + new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance), 0), Quaternion.identity);

                if(proj != null)
                {
                    Vector2 dir = Vector2.right * GetDirection();

                    proj.SetData(projectile);
                    proj._attackObject.SetOwner(this);
                    proj._attackObject.attackData = attackData;
                    proj._attackObject.attackData.owner = this;

                    proj.SetDirection(dir);


                    timeStamp = Time.time;
                    count++;
                }
            }


            if(count > blowupCount)
            {
                Die();
            }

            yield return null;

        }

    }





}
