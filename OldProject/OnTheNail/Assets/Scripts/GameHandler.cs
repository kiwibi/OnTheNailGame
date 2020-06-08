using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public int numberOfStages;

    public GameObject textHandlerReference;

    public GameObject audioManager;

    public static GameHandler instance;

    private Highscore highscore;
    private StageHandler stageHandler;
    private GameObject textHandler;

    private bool startSequence;
    private bool transitioning;

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
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        stageHandler = new StageHandler(numberOfStages);
        highscore = new Highscore();
        textHandler = null;
        startSequence = false;
        transitioning = false;
    }

    private void Update()
    {
        CheckToSpawnTextHandler();

        if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            textHandler.GetComponent<TextHandler>().SetEndScreen(highscore.GetCurrentScore().GetParDifference());
        }
        else if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            stageHandler = new StageHandler(numberOfStages);
            highscore = new Highscore();
            textHandler = null;
            startSequence = false;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            GoToScene("StartGame");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToScene("NextLevel");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GoToScene("Menu");
        }

        if (GameObject.FindGameObjectWithTag("Hammer") != null)
        {
            if (startSequence)
            {
                GameObject.FindGameObjectWithTag("Hammer").GetComponent<RotateAround>().setIntroSwing(false);
                transitioning = true;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Hammer").GetComponent<RotateAround>().setIntroSwing(true);
                transitioning = false;
            }
        }
    }

    private void CheckToSpawnTextHandler()
    {
        if (textHandler == null)
        {
            if (stageHandler.GetStageName() == "EndScreen")
            {
                textHandler = Instantiate(textHandlerReference, Vector3.zero, Quaternion.identity);
                textHandler.GetComponent<TextHandler>().SetEndScreen(highscore.GetCurrentScore().GetParDifference());
            }
            else if (stageHandler.GetStageName() != "Menu")
            {
                textHandler = Instantiate(textHandlerReference, Vector3.zero, Quaternion.identity);
                textHandler.GetComponent<TextHandler>().SetGameHandler(gameObject);
            }
        }
    }

    public void GoToScene(string sceneType)
    {
        StartCoroutine(SceneTransition(sceneType));
    }

    private IEnumerator SceneTransition(string sceneType)
    {
        startSequence = true;
        transitioning = true;

        if (textHandler != null)
        {
            textHandler.GetComponent<TextHandler>().PlayTransition();
            audioManager.GetComponent<AudioManager>().Play("Transition");
        }
        else if (GameObject.FindGameObjectWithTag("Planks") != null)
        {
            GameObject.FindGameObjectWithTag("Planks").GetComponent<Animator>().SetTrigger("Start");
            audioManager.GetComponent<AudioManager>().Play("Transition");
        }

        yield return new WaitForSeconds(2f);

        switch (sceneType)
        {
            case "Menu":
                MainMenu();
                break;

            case "End":
                EndScreen();
                break;

            case "NextLevel":
                NextLevel();
                break;

            case "StartGame":
                NewGame();
                break;
        }
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            highscore.GetCurrentScore().StoreSwings();
            highscore.GetCurrentScore().AddPar(GetPar());
            if (stageHandler.NextStage())
            {
                EndScreen();
                CheckToSpawnTextHandler();
            }
            else
            {
                CheckToSpawnTextHandler();
                startSequence = true;
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
        audioManager.GetComponent<AudioManager>().PlayMusic(1);
        startSequence = true;
    }

    public void MainMenu()
    {
        highscore.DiscardScore();
        stageHandler.GoToMainMenu();
        audioManager.GetComponent<AudioManager>().PlayMusic(0);
    }

    public void EndScreen()
    {
        stageHandler.GoToEndScreen();
        audioManager.GetComponent<AudioManager>().PlayMusic(0);
    }

    public void SetStartSequence(bool b)
    {
        startSequence = b;
    }

    public void AddSwing()
    {
        if (!transitioning)
        {
            highscore.GetCurrentScore().AddSwing();
        }
    }

    public int GetSwings()
    {
        return highscore.GetCurrentScore().GetCurrentSwings();
    }

    public int GetPar()
    {
        Scene s = SceneManager.GetActiveScene();
        return GetComponent<LevelPars>().GetPar(s.name);
    }

    public int GetStage()
    {
        return stageHandler.GetStageInt();
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

        public string GetStageName()
        {
            Scene currentS = SceneManager.GetActiveScene();
            return currentS.name;
        }

        public int GetStageInt()
        {
            return currentStage + 1;
        }

        public void StartNewGame()
        {
            currentStage = 0;
            SceneManager.LoadScene("Level 1");
        }

        public void GoToMainMenu()
        {
            currentStage = 0;
            SceneManager.LoadScene("Menu");
        }

        public void GoToEndScreen()
        {
            currentStage = 0;
            SceneManager.LoadScene("EndScreen");
        }
    }

    private class Score
    {
        private int currentSwings;
        private int totalSwings;
        private string name;
        private int totalPar;

        public Score()
        {
            currentSwings = 0;
            totalSwings = 0;
            name = "";
            totalPar = 0;
        }

        public int GetParDifference()
        {
            return totalSwings - totalPar;
        }

        public void AddPar(int par)
        {
            totalPar += par;
        }

        public void AddSwing()
        {
            currentSwings++;
        }

        public void ResetSwings()
        {
            currentSwings = 0;
        }

        public void StoreSwings()
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

    private class Highscore
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
