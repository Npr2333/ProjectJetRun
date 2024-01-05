using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameRestartController : MonoBehaviour
{
    public GameManager gameManager;
    public PlaneHealth planeHealth;
    public PlaneMovement planeMovement;
    public SpawnController spawnController;
    public BossAttackController bossAttackController;
    public EnvironmentController environmentController;
    public HUDController hudController;
    //public Stage1Director stage1Director;
    //public Stage2Director stage2Director;
    public PlayableDirector stage1Timeline;
    public PlayableDirector stage2Timeline;
    public GameObject centralPack;
    public Transform centralPackReset;
    public CinemachineVirtualCamera stage2Camera;
    public ScoreQueue scoreQueue;
    public GradualBlackScreen gradualBlackScreen;

    public void restartGame()
    {
        //spawnController.stopSpawn();
        planeHealth.resetHealth();
        planeMovement.resetStatus();
        environmentController.returnToOriginal();
        hudController.resetHUD();
        centralPack.transform.position = centralPackReset.position;
        scoreQueue.resetStatus();
        //stage1Director.StopDirect();
        //stage2Director.stopDirect();
        gradualBlackScreen.Show();
    }
}
