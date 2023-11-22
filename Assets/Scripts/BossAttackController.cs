using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BossAttackController : MonoBehaviour
{
    public GameObject bossInstance;
    public GameObject pawnInstance1;
    public GameObject pawnInstance2;
    private BossController bossController;
    private BossController pawnController1;
    private BossController pawnController2;
    private SplineAnimate pawnAnimate1;
    private SplineAnimate pawnAnimate2;
    private int currentEventIndex = 0;
    private float elapsedTime = 0f;
    public bool attacking = false;

    [System.Serializable]
    public class TimedEvent
    {
        public float time; 
        public int stateIndex;
        // 1: followPlayer, 2: cannonAttack, 3: missileAttack, 4: railgunAttack
        public int enemyIndex = 0; //0 for boss, 1 for pawn1, 2 for pawn2
    }

    public List<TimedEvent> timedEvents;

    public void Start()
    {
        if (bossInstance)
        {
            setBossInstance(bossInstance);
        }
        setPawnInstance(pawnInstance1, pawnInstance2);
    }

    private void Update()
    {   
        if(attacking)
        {
            elapsedTime += Time.deltaTime;

            if (currentEventIndex < timedEvents.Count && elapsedTime >= timedEvents[currentEventIndex].time && timedEvents[currentEventIndex].enemyIndex <= 0)
            {
                bossController.setState((timedEvents[currentEventIndex].stateIndex));

                currentEventIndex++;
                return;
            }else if(currentEventIndex < timedEvents.Count && elapsedTime >= timedEvents[currentEventIndex].time && timedEvents[currentEventIndex].enemyIndex == 1)
            {
                pawnController1.setState((timedEvents[currentEventIndex].stateIndex));
                pawnAnimate1.Play();

                currentEventIndex++;
                return;
            }else if(currentEventIndex < timedEvents.Count && elapsedTime >= timedEvents[currentEventIndex].time && timedEvents[currentEventIndex].enemyIndex == 2)
            {
                pawnController2.setState((timedEvents[currentEventIndex].stateIndex));
                pawnAnimate2.Play();

                currentEventIndex++;
                return;
            }
        }
        return;
    }

    public void setAttack(bool status)
    {
        attacking = status;
    }

    public void setBossInstance(GameObject boss)
    {
        bossInstance = boss;
        bossController = bossInstance.GetComponent<BossController>();
    }

    public void setPawnInstance(GameObject pawn1, GameObject pawn2)
    {
        pawnInstance1 = pawn1;
        pawnController1 = pawnInstance1.GetComponent<BossController>();
        pawnAnimate1 = pawnInstance1.GetComponent<SplineAnimate>();
        pawnInstance2 = pawn2;
        pawnController2 = pawnInstance2.GetComponent<BossController>();
        pawnAnimate2 = pawnInstance2.GetComponent<SplineAnimate>();
    }
}
