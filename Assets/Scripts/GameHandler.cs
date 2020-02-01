using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public int numberOfStages;

    public GameObject textHandlerReference;

    public static GameHandler instance;

    private Highscore highscore;
    private StageHandler stageHandler;
    private GameObject textHandler;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        stageHandler = new StageHandler(numberOfStages);
        highscore = new Highscore();
        textHandler = null;
    }

    private void Update()
    {
        CheckToSpawnTextHandler();

        if (Input.GetKeyDown(KeyCode.N))
        {
            NewGame();
            CheckToSpawnTextHandler();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            stageHandler.NextStage();
            CheckToSpawnTextHandler();
        }
    }

    private void CheckToSpawnTextHandler()
    {
        if (textHandler == null)
        {
            if (stageHandler.GetStage() != "Main Menu")
            {
                textHandler = Instantiate(textHandlerReference, Vector3.zero, Quaternion.identity);
            }
        }
    }

    public void SetNewTextHandler(GameObject textH)
    {
        textHandler = textH;
    }

    public void NewGame()
    {
        highscore.NewScore();
        stageHandler.StartNewGame();
    }

    private class StageHandler
    {
        private List<string> stages;
        private int stageAmount;
        private int currentStage;

        public StageHandler(int stageCount)
        {
            stages = new List<string>();
            AddStages(stageCount);
            stageAmount = stages.Count;
            currentStage = 0;
        }

        private void AddStages(int stageCount)
        {
            for (int i = 0; i < stageCount; i++)
            {
                stages.Add("Level " + (i + 1).ToString());
            }
        }

        public bool NextStage()
        {
            if (currentStage + 1 < stageAmount)
            {
                currentStage += 1;
                SceneManager.LoadScene(stages[currentStage]);
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetStage()
        {
            Scene currentS = SceneManager.GetActiveScene();
            return currentS.name;
        }

        public void StartNewGame()
        {
            currentStage = 0;
            SceneManager.LoadScene("Level 1");
        }
    }

    private class Score
    {
        private int currentSwings;
        private int totalSwings;
        private string name;

        public Score()
        {
            currentSwings = 0;
            totalSwings = 0;
            name = "";
        }

        private void AddSwing()
        {
            currentSwings++;
        }

        private void ResetSwings()
        {
            currentSwings = 0;
        }

        private void StoreSwings()
        {
            totalSwings += currentSwings;
            currentSwings = 0;
        }

        public int GetCurrentSwings()
        {
            return currentSwings;
        }

        public int GetTotalSwings()
        {
            return totalSwings;
        }

        public string GetName()
        {
            return name;
        }
    }

    class Highscore
    {
        private List<Score> scoreList;
        private Score currentScore;

        public Highscore()
        {
            scoreList = new List<Score>();
            currentScore = new Score();
        }

        public void NewScore()
        {
            currentScore = new Score();
        }

        public Score GetCurrentScore()
        {
            return currentScore;
        }

        public void AddScore()
        {
            scoreList.Add(new Score());
        }

        public void DiscardScore()
        {
            currentScore = null;
        }
    }
}
