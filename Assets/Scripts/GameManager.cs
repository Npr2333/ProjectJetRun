using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{   
    public enum GameState
    {
        GameStart,
        Opening,
        Stage1,
        Transition1,
        Stage2,
        Transition2,
        Stage3,
        Transition3,
        ScoreCount,
        GameOver,
        Dummy,
    }
    //Game State
    [SerializeField] public GameState currentState = GameState.GameStart;

    //State Classes
    private GameStateObject gameStart;
    private GameStateObject opening;
    private GameStateObject stage1;
    private GameStateObject stage2;
    private GameStateObject stage3;
    private GameStateObject transition1;
    private GameStateObject transition2;
    private GameStateObject transition3;
    private GameStateObject scoreCount;
    private GameStateObject gameOver;
    //Gameobjects
    public GameObject centralPack;
    public GameObject centralBulletPack;
    public GameObject player;
    public GameObject camera;
    public GameObject spawnController;
    public GameObject bossInstance;
    private GameObject playerInstance;
    private GameObject pawnInstance1;
    private GameObject pawnInstance2;
    //Speakers
    public AudioFade stage2AudioFade;
    //Cameras
    public CinemachineVirtualCamera openingCamera;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public UICamController UIcam;
    //Transforms
    public Transform SpawnPlayerTransform;
    public Transform SpawnBossTransform;
    public Transform Stage2BossTransform;
    public Transform Stage2PlayerTransform;
    public Transform Stage3BossTransform;
    //Timelines
    public PlayableDirector openingTimeline;
    public PlayableDirector stage1Timeline;
    public PlayableDirector stage2Timeline;
    public PlayableDirector endGameTimeline;
    //Score
    public GameObject mainCanvas;
    public ScoreQueue scoreQueue;
    public ShowScore showScore;
    //Canvas
    public GameObject openingCanvas;
    //Controller Scripts
    public static GameManager Instance;
    private PlaneMovement playerMovement;
    private PlaneShooting playerShooting;
    private TargetingManager playerMSL;
    private PlaneRailGunLaunch planeRailGunLaunch;
    public BossAttackController bossAttackController;
    public SplineFollowing splineFollower;
    public Stage1Director stage1Director;
    public Stage2Director stage2Director;
    public TeleportManager teleportManager;
    public GameRestartController gameRestartController;
    public SpeakerPlay stage2Speaker;
    public OptimizeController optimizeController;
    public CentralPackMovement centralPackMovement;
    public EnvironmentController environmentController;
    //State Parameters
    private bool spawnedBoss = false;
    //State Indicators
    private bool gameStarted = false;
    //Boss Parameter
    public float escapeMultiplier = -2f;


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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameManager.UIcam.setDepth(0);
            UIManager.Instance.PushPanel(UIPanelType.StartMenu);
            gameManager.environmentController.returnToOriginal();
        }
        public override void OnStateUpdate()
        {
            if (!gameManager.gameStarted)
            {
                return;
            }
            else
            {
                gameManager.SetCurrentState(GameState.Opening);
            }
        }
        public override void OnStateExit()
        {
            //gameManager.SetCurrentState(GameState.Stage1);
            gameManager.gameStarted = false;
            gameManager.UIcam.setDepth(-2);
            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.MainHud);
        }
    }

    public class OpeningStage : GameStateObject
    {
        public OpeningStage(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameManager.camera.GetComponent<AudioListener>().enabled = false;
            gameManager.openingCamera.GetComponent<AudioListener>().enabled = true;
            gameManager.openingCanvas.SetActive(true);
            gameManager.openingCamera.Priority = 10;
            gameManager.openingTimeline.Play();
        }
        public override void OnStateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                gameManager.SetCurrentState(GameManager.GameState.Stage1);
            }
        }
        public override void OnStateExit()
        {
            gameManager.openingTimeline.Stop();
            gameManager.camera.GetComponent<AudioListener>().enabled = true;
            gameManager.openingCamera.GetComponent<AudioListener>().enabled = false;
            gameManager.openingCamera.Priority = -100;
            gameManager.openingCanvas.SetActive(false);
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
            //Lock Cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //Player related
            //gameManager.playerInstance = Instantiate(gameManager.player, gameManager.SpawnPlayerTransform.position, gameManager.SpawnPlayerTransform.rotation);
            //gameManager.playerInstance.transform.parent = gameManager.centralPack.transform;
            //playerTargeting.centralBulletPack = gameManager.centralBulletPack;
            //playerShooting.centralbulletPack = gameManager.centralBulletPack;
            //gameManager.splineFollower.setTarget(gameManager.playerInstance.transform);
            gameManager.playerInstance = gameManager.player;
            gameManager.playerInstance.transform.localPosition = gameManager.SpawnPlayerTransform.transform.localPosition;
            gameManager.playerMovement = gameManager.playerInstance.GetComponent<PlaneMovement>();
            gameManager.planeRailGunLaunch = gameManager.playerInstance.GetComponent<PlaneRailGunLaunch>();
            PlaneShooting playerShooting = gameManager.playerInstance.GetComponent<PlaneShooting>();
            PlaneHealth playerHealth = gameManager.playerInstance.GetComponent<PlaneHealth>();
            //gameManager.playerShooting = gameManager.playerInstance.GetComponent<PlaneShooting>();
            gameManager.playerMSL = gameManager.playerInstance.GetComponent<TargetingManager>();
            playerHealth.cameraInstance = gameManager.camera1.GetComponent<CameraShake>();
            TargetingManager playerTargeting = gameManager.playerInstance.GetComponent<TargetingManager>();
            gameManager.playerInstance.SetActive(true);
            //Stage1 director
            Stage1Director stage1Director = gameManager.stage1Director;
            SpawnController spawn = gameManager.spawnController.GetComponent<SpawnController>();
            spawn.setTarget(gameManager.playerInstance);
            //stage1Director.DirectStage1();
            gameManager.stage1Timeline.Play();
            //spawn.startSpawn();
            //Set teleport parameters
            gameManager.teleportManager.setDissolve(gameManager.playerInstance.transform.Find("Dissolve").gameObject);
            gameManager.teleportManager.player = gameManager.playerInstance;
            //Camera related
            //CameraController cameraController = gameManager.camera.GetComponent<CameraController>();
            //cameraController.setTarget(gameManager.playerInstance);
            gameManager.camera1.Follow = gameManager.playerInstance.transform;
            gameManager.camera1.LookAt = gameManager.playerInstance.transform;
            gameManager.mainCanvas.SetActive(true);
            //gameManager.optimizeController.setOptimize(true);
            gameManager.centralPackMovement.setMoving(true);
        }
        public override void OnStateUpdate()
        {
            return;
        }
        public override void OnStateExit()
        {
            SpawnController spawnController = gameManager.spawnController.GetComponent<SpawnController>();
            spawnController.stopSpawn();
            gameManager.stage1Timeline.Stop();
            //gameManager.spawnController.SetActive(false);
        }
    }

    public class GameStage2 : GameStateObject
    {
        //This stage should be called by audio manager or environmentManager, after transition1.
        //This stage should set up the boss fight environment, push the "Boss fight start" UI panel,
        //and activate the bossAttackManager.
        public GameStage2(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Debug.Log("In Stage 2");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameManager.camera2.Priority = 3;
            gameManager.playerMovement.setTopDown();
            gameManager.playerMovement.setInputStatus(true);
            //gameManager.bossInstance = Instantiate(gameManager.boss, gameManager.SpawnBossTransform.position, gameManager.SpawnBossTransform.rotation);
            //gameManager.bossInstance.transform.parent = gameManager.centralPack.transform;
            gameManager.bossInstance.transform.position = gameManager.SpawnBossTransform.position;
            gameManager.bossInstance.transform.rotation = gameManager.SpawnBossTransform.rotation;
            gameManager.bossInstance.SetActive(true);
            gameManager.bossAttackController.setBossInstance(gameManager.bossInstance);
            BossController bossController = gameManager.bossInstance.GetComponent<BossController>();
            //bossController.centralBulletPack = gameManager.centralBulletPack.transform;
            //bossController.setTarget(gameManager.playerInstance);
            bossController.setState(1);
            bossController.MoveToPosition(gameManager.Stage2BossTransform.localPosition);
            //gameManager.bossAttackController.setAttack(true);
            //gameManager.stage2Director.DirectStage2();
            gameManager.mainCanvas.SetActive(false);
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameManager.bossInstance.transform.position = gameManager.Stage3BossTransform.position;
            gameManager.bossInstance.SetActive(true);
            BossController bossController = gameManager.bossInstance.GetComponent<BossController>();
            bossController.setMultiplier(gameManager.escapeMultiplier);
            bossController.setState(1);

            //gameManager.stage2Director.DirectStage3();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.stage2Timeline.Stop();
            gameManager.stage2AudioFade.FadeOut();
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
            gameManager.playerMovement.setInputStatus(false);
            //gameManager.playerShooting.enabled = false;
            gameManager.playerMSL.enabled = false;
            gameManager.planeRailGunLaunch.enabled = false;
            gameManager.playerMovement.MoveToPosition(gameManager.Stage2PlayerTransform);
            //gameManager.stage2Director.DirectTransition1();
            gameManager.stage2Timeline.Play();
            gameManager.bossInstance.transform.position = gameManager.SpawnBossTransform.position;
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
            gameManager.bossInstance.SetActive(false);
            gameManager.playerMovement.setTopDown();
            //gameManager.stage2Director.DirectTransition2();
            gameManager.mainCanvas.SetActive(true);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.bossInstance.transform.position = gameManager.Stage3BossTransform.position;
        }
    }

    public class Transition3 : GameStateObject
    {
        //This stage should be called by playerHealth or the boss instance,
        //after player's death or boss taken too much damage.
        public Transition3(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            gameManager.endGameTimeline.Play();
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.endGameTimeline.Stop();
        }
    }

    public class ScoreCount : GameStateObject
    {
        public ScoreCount(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameManager.mainCanvas.SetActive(false);
            //gameManager.showScore.showScore(gameManager.scoreQueue.getScore());
            gameManager.UIcam.setDepth(0);
            UIManager.Instance.PushPanel(UIPanelType.ScoreMenu);
            ShowScore showScore = UIManager.Instance.GetPanel(UIPanelType.ScoreMenu).GetComponent<ShowScore>();
            showScore.showScore(gameManager.scoreQueue.getScore());
            gameManager.camera2.Priority = 0;
            gameManager.stage1Timeline.Stop();
            gameManager.stage1Timeline.time = 0;
            gameManager.stage2Timeline.Stop();
            gameManager.stage2Timeline.time = 0;
            gameManager.stage2Speaker.Stop();
            gameManager.endGameTimeline.Stop();
            gameManager.endGameTimeline.time = 0;
            gameManager.bossAttackController.resetAttack();
            gameManager.playerInstance.SetActive(false);
            gameManager.playerInstance.transform.localPosition = gameManager.SpawnPlayerTransform.localPosition;
            SpawnController spawnController = gameManager.spawnController.GetComponent<SpawnController>();
            spawnController.stopSpawn();
            gameManager.centralPackMovement.setMoving(false);
        }
        public override void OnStateUpdate()
        {

        }
        public override void OnStateExit()
        {
            gameManager.UIcam.setDepth(-2);
            UIManager.Instance.PopPanel();
            ShowScore showScore = UIManager.Instance.GetPanel(UIPanelType.ScoreMenu).GetComponent<ShowScore>();
            showScore.resetScore();
            //gameManager.showScore.resetScore();
        }
    }

    public class GameOver : GameStateObject
    {
        //This stage should be called by playerHealth or the boss instance,
        //after player's death or boss taken too much damage.
        public GameOver(GameManager manager) : base(manager) { }
        public override void OnStateEnter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //gameManager.stage1Director.StopDirect();
            //gameManager.stage2Director.stopDirect();
            gameManager.mainCanvas.SetActive(false);
            gameManager.UIcam.setDepth(0);
            UIManager.Instance.PushPanel(UIPanelType.OverMenu);
            gameManager.camera2.Priority = 0;
            gameManager.stage1Timeline.Stop();
            gameManager.stage1Timeline.time = 0;
            gameManager.stage2Timeline.Stop();
            gameManager.stage2Timeline.time = 0;
            gameManager.stage2Speaker.Stop();
            gameManager.bossAttackController.resetAttack();
            gameManager.playerInstance.SetActive(false);
            gameManager.playerInstance.transform.localPosition = gameManager.SpawnPlayerTransform.localPosition;
            SpawnController spawnController = gameManager.spawnController.GetComponent<SpawnController>();
            spawnController.stopSpawn(); 
            gameManager.centralPackMovement.setMoving(false);
        }
        public override void OnStateUpdate()
        {
            if (!gameManager.gameStarted)
            {
                return;
            }
            else
            {
                gameManager.gameRestartController.restartGame();
                gameManager.SetCurrentState(GameState.Stage1);
            }
        }
        public override void OnStateExit()
        {
            gameManager.gameStarted = false;
            gameManager.UIcam.setDepth(-2);
            UIManager.Instance.PopPanel();
        }
    }

    Dictionary<GameState, GameStateObject> stateMap;
    


    private void Awake()
    {
        //Instantiate the game state classes and populate stateMap dictionary
        stateMap = new Dictionary<GameState, GameStateObject>();
        Instance = this;
        gameStart = new GameStartStage(this);
        opening = new OpeningStage(this);
        stage1 = new GameStage1(this);
        stage2 = new GameStage2(this);
        stage3 = new GameStage3(this);
        transition1 = new Transition1(this);
        transition2 = new Transition2(this);
        transition3 = new Transition3(this);
        scoreCount = new ScoreCount(this);
        gameOver = new GameOver(this);
        stateMap.Add(GameState.GameStart, gameStart);
        stateMap.Add(GameState.Opening, opening);
        stateMap.Add(GameState.Stage1, stage1);
        stateMap.Add(GameState.Stage2, stage2);
        stateMap.Add(GameState.Stage3, stage3);
        stateMap.Add(GameState.Transition1, transition1);
        stateMap.Add(GameState.Transition2, transition2);
        stateMap.Add(GameState.Transition3, transition3);
        stateMap.Add(GameState.ScoreCount, scoreCount);
        stateMap.Add(GameState.GameOver, gameOver);
    }

    public void SetCurrentState(GameState state)
    {
        if (state != GameState.GameStart)
        {
            GameStateObject lastStateObject = stateMap[currentState];
            lastStateObject.OnStateExit();
        }

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

    public void StartGame()
    {
        gameStarted = true;
    }
}