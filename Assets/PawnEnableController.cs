using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEnableController : MonoBehaviour
{
    public GameObject ActorPawn1;
    public GameObject ActorPawn2;
    public GameObject ActorBoss;

    public void actorPawn1SetActive(bool status)
    {
        ActorPawn1.SetActive(status);
    }

    public void actorPawn2SetActive(bool status)
    {
        ActorPawn2.SetActive(status);
    }

    public void actorBossSetActive(bool status)
    {
        ActorBoss.SetActive(status);
    }
}
