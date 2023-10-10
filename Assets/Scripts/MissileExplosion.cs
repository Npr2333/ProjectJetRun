using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissileExplosion : MonoBehaviour
{
    public ParticleSystem Explode;
    public int Damage;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target && target.tag == "DestructableObstacles") 
        {   
            if(target.tag == "DestructableObstacles")
            {
                DestructableObstacles obstacle = collision.gameObject.GetComponent<DestructableObstacles>();
                obstacle.OnHit(Damage);
            }
            else if(target.tag == "IndestructableObstacles")
            {
                //IndustructableObstacles obstacle = collision.gameObject.GetComponent<IndustructableObstacles>();
                //obstacle.OnHit(Damage);
                ParticleSystem effect = Instantiate(Explode, gameObject.transform.position, Quaternion.identity);
                effect.Play();
                Destroy(gameObject);
            }
            else if(target.tag == "Plane")
            {
                PlaneHealth plane = collision.gameObject.GetComponent<PlaneHealth>();
                plane.ReduceHealth(Damage, gameObject.transform.position);
            }
        }

    }
}
