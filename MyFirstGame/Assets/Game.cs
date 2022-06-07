using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Game : OneInstance
{
    public static Game instance { get; set; }

    public PlayerUI playerUI;

    public static Game GameInstance;
    public static GameObject MainGameObject;
    public static CoroutineHandler coroutineHandler;

    public Material DaySkybox;
    public Material NightSkybox;

    public DayNightCycle dayNightCycle = new DayNightCycle();

    private Dictionary<string, waitForOperation> runningWaitForOperations = new Dictionary<string, waitForOperation>();


    void Update()
    {
        waitForLoop();
    }

    void waitForLoop()
    {
        List<string> finishedOp = new List<string>();
        foreach (string key in runningWaitForOperations.Keys){
            
            waitForOperation val = runningWaitForOperations[key];
            val.current += Time.deltaTime;
            if (val.current >= val.end) {
                val.callback();
                finishedOp.Add(key);
            }
        }

        foreach (string key in finishedOp) {
            runningWaitForOperations.Remove(key);
        }
    }

    public class waitForOperation
    {
        public string name;
        public float current;
        public float end;
        public waitForDone callback;

        public waitForOperation(string name, float current, float end)
        {
            this.name = name;
            this.current = current;
            this.end = end;
        }
    }

    public delegate void waitForDone();

    public void WaitFor(string name, float time, waitForDone callback)
    {
        waitForOperation waitOperation = new waitForOperation(name, 0, time);
        waitOperation.callback = callback;
        runningWaitForOperations.Add(name, waitOperation);
    }

    void Awake()
    {

        if (instance)
        {
            Debug.LogWarning("Did you mean to make second instance of type. Type extends OneInstance");
        }
        else
        {
            instance = this;
        }

        OneInstance.AddInstance(this.ToString(), this);

        GameInstance = this;
        MainGameObject = gameObject;
        coroutineHandler = MainGameObject.GetComponent<CoroutineHandler>();

        dayNightCycle.changed += DayNightToggle;
        dayNightCycle.Start();
    }

    override public void ResetInstance()
    {
        OneInstance.AllInstances[this.ToString()] = new Game();
    }


    void DayNightToggle()
    {
        if (dayNightCycle.dayNight == DayNightCycle.DayNight.Day)
        {
            RenderSettings.skybox = DaySkybox;
        }
        else
        {
            RenderSettings.skybox = NightSkybox;
        }
    }


    public void Restart()
    {
        WaitFor("Time", 10, () => {
            OneInstance.resetAllInstances();
            Enemy.navMeshData = null;
            SceneManager.LoadScene("Scenes/"+SceneManager.GetActiveScene().name);   
        });
    }
}
