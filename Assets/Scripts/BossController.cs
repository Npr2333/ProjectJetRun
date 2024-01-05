using System;
using System.Collections;
using System.Collections.Generic;
using Tarodev;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Accessibility;

public class BossController : MonoBehaviour
{
    public enum EnemyState
    {
        FollowPlayer,
        CannonAttack,
        MissileAttack,
        RailgunAttack
    }
    //Entites
    public Rigidbody plane;
    //Animator
    public Animator animator;
    //Central Packs
    public Transform centralBulletPack;
    //Projectiles
    public Rigidbody Bullet;
    public GameObject Missile;
    public GameObject RailgunShot;
    //Projectile Particles
    public ParticleSystem CannonFlare;
    //Projectile Audios
    public AudioSource Speaker;
    public AudioClip CannonClip;
    public AudioClip MissileClip;
    public AudioClip RailgunClip;
    //Movements
    private CharacterController _controller;
    public float followSpeed = 5f;
    public float plannedSpeed = 100f;
    public float acceleration = 10f;
    public float followRange = 1f;
    public float attackRange = 10f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float SpeedX = 0f;
    private float SpeedY = 0f;
    private Vector3 LastPosition;
    private bool isMoving = false;
    //Parameters
    public float speed = 30f;
    public int cannonDamage = 50;
    //Attack Movement Multiplier
    public float AttackMovementMultiplier = 2f;
    //Follow Multiplier
    public float FollowMovementMultiplier = 1f;
    //Model Rotator
    public GameObject planeRotator;
    public GameObject planeModel;
    public float rollDegree = 15f;
    public float pitchDegree = 15f;
    public float rotationSpeed = 30f;
    private Quaternion targetRotation;
    //Health
    public float health = 1000f;
    public ParticleSystem ExplosionEffect;
    //Target
    public GameObject player;
    private Vector3 playerPosition;
    //Sonic Boom
    public ParticleSystem sonicBoom;
    //Death Explosion
    public ParticleSystem deathBoom;
    //Cannon Shooting
    public Transform FireTransform;
    public float RPM = 600f;
    public float LaunchForce = 10f;
    private float m_FireCooldown = 1f;
    private float m_FireCooldownLeft = 0.0f;
    //Missile Launch
    public Transform MissileTransform;
    public int MissileAtATime = 1;
    private bool LaunchedMissile = false;
    //Railgun Launch
    public LaunchRailGun launcher;
    private bool LaunchedBeam = false;
    //State
    public EnemyState currentState;
    //Qualification Variables
    public bool isSplineControlled = false;
    private bool plannedMoving = false;
    private bool isDead = false;
    public Vector3 plannedPosition;
    //Canvases
    public Canvas canvas;
    public GameObject deathCross;
    void Start()
    {
        currentState = EnemyState.FollowPlayer;
        _controller = GetComponent<CharacterController>();
        plane = GetComponent<Rigidbody>();
        LastPosition = transform.position;
        launcher = gameObject.GetComponent<LaunchRailGun>();
        RPM = RPM / 60;
        if(deathCross)
        {
            deathCross.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {   

        if (Mathf.Abs(SpeedX) > 0.1f || Mathf.Abs(SpeedY) > 0.1f)
        {
            isMoving = true;
            //Debug.Log("Moving");
        }
        else
        {
            isMoving = false;
        }

        SpeedX = (transform.position.x - LastPosition.x) / Time.deltaTime;
        SpeedY = (transform.position.y - LastPosition.y) / Time.deltaTime;


        LastPosition = transform.position;

        if(player)
        {
            playerPosition = player.transform.position;
        }

        if (!plannedMoving)
        {
            switch (currentState)
            {
                case EnemyState.FollowPlayer:
                    FollowPlayer();
                    break;
                case EnemyState.CannonAttack:
                    CannonAttack();
                    break;
                case EnemyState.MissileAttack:
                    MissileAttack();
                    break;
                case EnemyState.RailgunAttack:
                    RailgunAttack();
                    break;
            }
        }
        else if(plannedMoving && plannedPosition != null)
        {
            toPosition(plannedPosition);
        }

        //Debug.Log(currentState);
    }

    //Implementation for states
    public void FollowPlayer()
    {
        TrackPlayer(1, FollowMovementMultiplier);
        LaunchedMissile = false;
        LaunchedBeam = false;
    }

    public void CannonAttack()
    {
        TrackPlayer(AttackMovementMultiplier);
        if (!Speaker.isPlaying)
        {
            Speaker.Play();
        }
        if (m_FireCooldownLeft > 0)
        {
            m_FireCooldownLeft -= Time.deltaTime;
        }

        // Fire button is pressed and cooldown has finished
        if ( m_FireCooldownLeft <= 0)
        {
            CannonFire();
            m_FireCooldownLeft = m_FireCooldown / RPM;  // Start cooldown
        }
    }

    public void MissileAttack()
    {
        TrackPlayer(AttackMovementMultiplier);
        if(player != null && !LaunchedMissile)
        {
            LaunchMissile();
        }
        currentState = EnemyState.FollowPlayer;
    }

    public void RailgunAttack()
    {
        TrackPlayer(0);
        if(!LaunchedBeam)
        {
            launcher.Fire();
            LaunchedBeam = true;
        }
        currentState = EnemyState.FollowPlayer;
    }

    public void setState(int i)
    {
        switch(i)
        {
            case 1:
                currentState = EnemyState.FollowPlayer;
                break;
            case 2:
                currentState = EnemyState.CannonAttack;
                break;
            case 3:
                currentState = EnemyState.MissileAttack;
                break;
            case 4:
                currentState = EnemyState.RailgunAttack;
                break;
            default:
                break;
        }
    }

    //Boss Health Logic
    public void OnHit(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            if (deathCross)
            {
                deathCross.SetActive(true);
            }
            canvas.gameObject.SetActive(false);
            Death();
            //gameObject.SetActive(false);
        }
    }

    //Boss Death Related
    private void Death()
    {
        //Handle boss Death
        Debug.Log("Boss is dead");
        Explode();
    }

    private void Explode()
    {
        //Handle the particle system of plane explosion
        ParticleSystem explosion = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        explosion.Play();
        StartCoroutine(startDeath(3f));

    }

    private IEnumerator startDeath(float time)
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(time);
        deathBoom.Play();
        yield return new WaitForSeconds(deathBoom.duration);
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm)
        {
            gm.SetCurrentState(GameManager.GameState.Transition3);
        }
        gameObject.SetActive(false);
    }

    //Boss Attack Related
    private void TrackPlayer(float multiplier, float followMultiplier = 1f)
    {
        if (isSplineControlled)
        {
            return;
        }
        Vector3 direction = (playerPosition - transform.position).normalized;

        float headingX = direction.x > 0 ? 1f : -1f;
        float headingY = direction.y > 0 ? 1f : -1f;
        headingX = Mathf.Abs(direction.x) > followRange ? 1f : direction.x;
        headingY = Mathf.Abs(direction.y) > followRange ? 1f : direction.y;

        velocityX = Mathf.Lerp(velocityX, headingX * followSpeed * multiplier, acceleration * multiplier * Time.deltaTime);
        velocityY = Mathf.Lerp(velocityY, headingY * followSpeed * multiplier, acceleration * multiplier * Time.deltaTime);

        Vector3 moveX = transform.right * velocityX * followMultiplier * Time.deltaTime;
        Vector3 moveY = transform.up * velocityY * Time.deltaTime;

        _controller.Move(moveX + moveY);
        
    }
    private void CannonFire()
    {
        Rigidbody shell = Instantiate(Bullet, FireTransform.position, FireTransform.rotation) as Rigidbody;
        EnemyProjectile projectile = shell.gameObject.GetComponent<EnemyProjectile>();
        projectile.setDamage(cannonDamage);
        shell.transform.parent = centralBulletPack.transform;
        ParticleSystem flare = Instantiate(CannonFlare, FireTransform.position, FireTransform.rotation) as ParticleSystem;
        flare.transform.parent = transform;
        flare.Play();

        shell.velocity = LaunchForce * FireTransform.forward;
    }

    private void LaunchMissile()
    {
        Quaternion rotation = Quaternion.identity;
        for (int i = 0; i < MissileAtATime; i++)
        {

            GameObject rocket = Instantiate(Missile, MissileTransform.position, rotation * Quaternion.Euler(i * 360 / MissileAtATime, i * 360 / MissileAtATime, i * 360 / MissileAtATime));
            rocket.transform.parent = centralBulletPack.transform;
            MissileMovement msl = rocket.GetComponent<MissileMovement>();
            msl.target = player;
        }
        LaunchedMissile = true;
    }

    private void LateUpdate()
    {
        if (isSplineControlled)
        {
            return;
        }
        float headingX = SpeedX > 0 ? 1f : -1f;
        headingX = Mathf.Abs(SpeedX) > 20 ? headingX / Mathf.Abs(headingX) * 1f : SpeedX/20;
        float headingY = SpeedY > 0 ? 1f : -1f;
        headingY = Mathf.Abs(SpeedY) > 20 ? headingY / Mathf.Abs(headingY) * 1f : SpeedY/20;
        if (isMoving)
        {
            targetRotation = Quaternion.Euler(pitchDegree * -headingY, 0, rollDegree * -headingX);
        }
        else
        {
            targetRotation = Quaternion.identity;
        }
        planeRotator.transform.rotation = Quaternion.Lerp(planeRotator.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }


    public void setTarget(GameObject target)
    {
        player = target;
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        plannedMoving = true;
        plannedPosition = newPosition;
    }

    private void toPosition(Vector3 newPosition)
    {
        Vector3 direction = (newPosition - transform.localPosition).normalized;

        Vector3 move = direction * plannedSpeed * Time.deltaTime;

        Vector3 position = Vector3.MoveTowards(transform.localPosition, newPosition, move.magnitude);

        _controller.Move(move);

        if (Vector3.Distance(transform.localPosition, newPosition) <= 0.2f)
        {
            plannedMoving = false;
        }
    }

    //Hyper Sonic Related
    public void isHyperSonic(float time)
    {
        startVapor(time);
    }

    private IEnumerator startVapor(float time)
    {
        sonicBoom.Play();
        yield return new WaitForSeconds(time);
        sonicBoom.Stop();
    }

    public void isLocked()
    {
        canvas.gameObject.SetActive(true);
    }

    public void notLocked()
    {
        canvas.gameObject.SetActive(false);
    }

    public void resetStatus()
    {
        isDead = false;
        planeModel.SetActive(true);
        animator.SetBool("isDead", false);
        planeModel.transform.localPosition = Vector3.zero;
        planeModel.transform.localRotation = Quaternion.identity;
    }

    public void setMultiplier(float multiplier)
    {
        FollowMovementMultiplier = multiplier;
    }
}
