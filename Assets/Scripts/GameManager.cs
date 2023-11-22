using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public enum GameState
    {
        GameStart,
        Stage1,
        Transition1,
        Stage2,
        Transition2,
        Stage3,
        GameOver,
        Dummy,
    }
    //Game State
    [SerializeField] public GameState currentState = GameState.GameStart;

    //State Classes
    private GameStateObject gameStart;
    private GameStateObject stage1;
    private GameStateObject stage2;
    private GameStateObject stage3;
    private GameStateObject transition1;
    private GameStateObject transition2;
    private GameStateObject gameOver;
    //Gameobjects
    public GameObject centralPack;
    public GameObject centralBulletPack;
    public GameObject player;
    public GameObject camera;
    public GameObject spawnController;
    public GameObject boss;
    private GameObject playerInstance;
    private GameObject bossInstance;
    private GameObject pawnInstance1;
    private GameObject pawnInstance2;
    //Cameras
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    //Transforms
    public Transform SpawnPlayerTransform;
    public Transform SpawnBossTransform;
    public Transform Stage2BossTransform;
    public Transform Stage2PlayerTransform;
    public Transform Stage3BossTransform;
    //Controller Scripts
    public static GameManager Instance;
    private PlaneMovement playerMovement;
    public BossAttackController bossAttackController;
    public SplineFollowing splineFollower;
    public Stage2Director stage2Director;
    //State Parameters
    private bool spawnedBoss = false;


    public abstract class GameStateObject
    {
        protected GameManager gameManager;
        public GameStateObject(GameManager manager)
        {
            gameManager = manager;
        }
        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    };

    public class GameStartStage : GameStateObject
    {
        public GameStartStage(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {   
            //The code here is just to get the game running, will set this to engage with UI after the UI system is up.
            gameManager.SetCurrentState(GameState.Stage1);
        }
        public override void OnStateUpdate()
        {
            return;
        }
        public override void OnStateExit()
        {
            return;
        }
    }

    public class GameStage1 : GameStateObject
    {
        //This stage should be called after the gameStartStage, specifically after player clicked gameStart.
        //This stage should set up the play enviroment for copter fighting, and disable it when exiting.
        public GameStage1(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Stage 1");
            gameManager.playerInstance = Instantiate(gameManager.player, gameManager.SpawnPlayerTransform.position, gameManager.SpawnPlayerTransform.rotation);
            gameManager.playerInstance.transform.parent = gameManager.centralPack.transform;
            gameManager.playerMovement = gameManager.playerInstance.GetComponent<PlaneMovement>();
            PlaneShooting playerShooting = gameManager.playerInstance.GetComponent<PlaneShooting>();
            TargetingManager playerTargeting = gameManager.playerInstance.GetComponent<TargetingManager>();
            playerTargeting.centralBulletPack = gameManager.centralBulletPack;
            playerShooting.centralbulletPack = gameManager.centralBulletPack;
            SpawnController spawn = gameManager.spawnController.GetComponent<SpawnController>();
            CameraController cameraController = gameManager.camera.GetComponent<CameraController>();
            gameManager.splineFollower.setTarget(gameManager.playerInstance.transform);
            cameraController.setTarget(gameManager.playerInstance);
            spawn.setTarget(gameManager.playerInstance);
            spawn.startSpawn();
            gameManager.camera1.Follow = gameManager.playerInstance.transform;
            gameManager.camera1.LookAt = gameManager.playerInstance.transform;
           //gameManager.camera2.Follow = gameManager.playerInstance.transform;
           //gameManager.camera2.LookAt = gameManager.playerInstance.transform;
        }
        public override void OnStateUpdate()
        {
            return;
        }
        public override void OnStateExit()
        {
            gameManager.spawnController.SetActive(false);
        }
    };

    public class GameStage2 : GameStateObject
    {
        //This stage should be called by audio manager or environmentManager, after transition1.
        //This stage should set up the boss fight environment, push the "Boss fight start" UI panel,
        //and activate the bossAttackManager.
        public GameStage2(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Stage 2");
            gameManager.playerMovement.setInputStatus(true);
            gameManager.bossInstance = Instantiate(gameManager.boss, gameManager.SpawnBossTransform.position, gameManager.SpawnBossTransform.rotation);
            gameManager.bossInstance.transform.parent = gameManager.centralPack.transform;
            BossController bossController = gameManager.bossInstance.GetComponent<BossController>();
            bossController.centralBulletPack = gameManager.centralBulletPack.transform;
            bossController.setTarget(gameManager.playerInstance);
            bossController.setState(2);
            bossController.MoveToPosition(gameManager.Stage2BossTransform.position);
            gameManager.bossAttackController.setBossInstance(gameManager.bossInstance);
            gameManager.bossAttackController.setAttack(true);
            gameManager.stage2Director.DirectStage2();
        }
        public override void OnStateUpdate()
        {
            
        }
        public override void OnStateExit()
        {

        }
    }

    public class GameStage3 : GameStateObject
    {
        //This stage should be called by transition2, after player exited the tunnel.
        //This stage should set the audio manager to play the correct audio clip for AWACS and boss,
        //and set boss's status to stage3.
        public GameStage3(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Stage 3");
            gameManager.bossInstance.SetActive(true);
            gameManager.stage2Director.DirectStage3();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }

    public class Transition1 : GameStateObject
    {   
        //This stage should be called by spawnManager, after the music has ended.
        //This stage should diable player input, set player to the wanted position,
        //play the transition animation, gradually set the camera to top down angle,
        //and call the environmentManager to change the environment.
        public Transition1(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Transition 1");
            gameManager.camera2.Priority = 3;
            gameManager.playerMovement.setInputStatus(false);
            gameManager.playerMovement.setTopDown();
            gameManager.playerMovement.MoveToPosition(gameManager.Stage2PlayerTransform);
            gameManager.stage2Director.DirectTransition1();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.playerMovement.setInputStatus(true);
        }
    }

    public class Transition2 : GameStateObject
    {
        /// <summary>
        /// This stage should be called by bossAttackManager, after certain amount of time of the attack.
        /// </summary>
        /// <param name="manager"></param>
        //Thi stage should change the camera back to the 3rd person view, set the boss to the transition position,
        //and set the boss to the stage3 position when exiting.
        public Transition2(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Transition 2");
            gameManager.camera2.Priority = 1;
            gameManager.bossInstance.transform.position = gameManager.Stage3BossTransform.position;
            gameManager.bossInstance.SetActive(false);
            gameManager.playerMovement.setTopDown();
            gameManager.stage2Director.DirectTransition2();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.bossInstance.transform.position = gameManager.Stage3BossTransform.position;
        }
    }

    public class GameOver : GameStateObject
    {
        //This stage should be called by playerHealth or the boss instance,
        //after player's death or boss taken too much damage.
        public GameOver(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("GameOver");
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {

        }
    }

    Dictionary<GameState, GameStateObject> stateMap;
    


    private void Awake()
    {
        //Instantiate the game state classes and populate stateMap dictionary
        stateMap = new Dictionary<GameState, GameStateObject>();
        Instance = this;
        gameStart = new GameStartStage(this);
        stage1 = new GameStage1(this);
        stage2 = new GameStage2(this);
        stage3 = new GameStage3(this);
        transition1 = new Transition1(this);
        transition2 = new Transition2(this);
        gameOver = new GameOver(this);
        stateMap.Add(GameState.GameStart, gameStart);
        stateMap.Add(GameState.Stage1, stage1);
        stateMap.Add(GameState.Stage2, stage2);
        stateMap.Add(GameState.Stage3, stage3);
        stateMap.Add(GameState.Transition1, transition1);
        stateMap.Add(GameState.Transition2, transition2);
        stateMap.Add(GameState.GameOver, gameOver);
    }

    public void SetCurrentState(GameState state)
    {
        GameStateObject lastStateObject = stateMap[currentState];
        lastStateObject.OnStateExit();

        GameStateObject currentStateObject = stateMap[state];
        currentStateObject.OnStateEnter();

        currentState = state;
    }

    // Start is called before the first frame update
    public void Start()
    {
        SetCurrentState(GameState.GameStart);
    }

    // Update is called once per frame
    public void Update()
    {
        stateMap[currentState].OnStateUpdate();
    }

    private void OnValidate()
    {   
        if(stateMap != null)
        {
            SetCurrentState(currentState);
        }
    }
}

   

//    public void GameStart()
//    {
//        // Code to start the game
//        Debug.Log("GameStart");
//        playerInstance = Instantiate(player, SpawnPlayerTransform.position, SpawnPlayerTransform.rotation);
//        playerMovement = playerInstance.GetComponent<PlaneMovement>();
//        SpawnController spawn = spawnController.GetComponent<SpawnController>();
//        spawn.setTarget(playerInstance);
//        CameraController cameraController = camera.GetComponent<CameraController>();
//        cameraController.setTarget(playerInstance);
//        currentState = GameState.Stage1;
//    }

//    public void Stage1()
//    {
        
//    }

//    public void Transition1()
//    {
//        // Code to handle transition
//        Debug.Log("Transitioning");
//        camera2.Priority = 3;
//        playerMovement.setInputStatus(false);
//        playerMovement.setTopDown();
//        playerMovement.MoveToPosition(Stage2PlayerTransform.position);
//        //currentState = GameState.Stage2;
//    }

//    public void Stage2()
//    {
//        // Code to handle stage 2
//        playerMovement.setInputStatus(true);
//        if (!spawnedBoss)
//        {   
//            Debug.Log("Boss Fight Start");
//            bossInstance = Instantiate(boss, SpawnBossTransform.position, SpawnBossTransform.rotation);
//            BossController bossController = bossInstance.GetComponent<BossController>();
//            bossController.setTarget(playerInstance);
//            bossController.setState(2);
//            spawnedBoss = true;
//        }
//        else
//        {
//            //BossAttackController
//        }
//    }

//    public void Transistion2()
//    {
//        // Code to handle transistion 2
//        camera2.Priority = 1;
//        playerMovement.setTopDown();
        
//    }

//    public void Stage3()
//    {

//    }

//    public void GameOver()
//    {
//        //Code to handle gameover
//        Debug.Log("GameOver");
//    }

//    private void ToStage1()
//    {
//        currentState = GameState.Stage1;
//    }

//    private void ToTransition1()
//    {
//        currentState = GameState.Transition1;
//    }

//    private void ToStage2()
//    {
//        currentState = GameState.Stage2;
//    }

//    private void ToTransisiton2()
//    {
//        currentState = GameState.Transition2;
//    }

//    private void ToStage3()
//    {
//        currentState = GameState.Stage3;
//    }
//}
