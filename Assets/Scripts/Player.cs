using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float ForceMoveDelay;
    public GameObject MainCamera;
    public GameObject MiniMapCamera;
    public GameObject Plant;
    public GameObject GameOverText;
    public GameObject ResetText;
    public GameObject RootPrefab;
    public GameObject TauntText;
    public GameObject Tree;

    private bool cameraResetting;
    private bool gameOver;
    private bool lockInput;
    private float timeUntilMove;
    private float finalSize;
    private int lives;
    private int score;
    private string[] taunts;
    private Text scoreText;
    private Vector3 currentPosition;
    private Vector3 nextPosition;

    [HideInInspector]
    public enum Directions { Null, Right, Left, Up, Down };
    public Directions MovementDirection;
    public int RootParts;

    private void Start()
    {
        lives = 4;
        scoreText = GameObject.Find("/Canvas/Score Text").GetComponent<Text>();
        currentPosition = Vector3.zero;
        timeUntilMove = ForceMoveDelay;

        taunts = new string[] { "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                                "'Success comes when people act together; failure tends to happen alone' - Deepak Chopra",
                                "'Mother F***er' - Ltn. Rautanen",
                                "'We're out of pancakes.' - Vesa Ahopelto",
                                "'If at first you don't succeed, git gud you f***ing noob!' - Olli Uikkanen, as he assaulted me",
                                "'Failure is the condiment that gives success its flavor.' - Truman Capote",
                                "'Failure means a stripping away of the inessential' - J. K. Rowling",
                                "'I will f***ing stamp 'em!' - Kimmo Nikkanen",
                                "'Mistakes are the portals of discovery.' - James Joyce",
                                "'There is no such thing as failure. There are only results.' - Tony Robbins",
                                "'Don't quote me on this' - Olli Uikkanen",
                                "'Täs lopus tulee quoteja niinku call of dutyssä' - Elias Massa",
                                "'Kun ne nachot loppu, niin me syötiin näitä omenoita.' - Antton Jokinen",
                                "'You cannot climb the ladder of success dressed in the costume of failure.' - Zig Ziglar",
                                "'Failure is not fatal, but failure to change might be.' - John Wooden",
                                "'Tails on vähän niin kuin sininen Sonic' - Riikka Kilpeläinen",
                                "'There is a reason horses speak to horses and not to cows. It is because the are horses.' - Pothenar",
                                "'Our house in the middle of our house in the middle of our house...' - Olli Uikkanen",
                                "'Jet fuel cannot melt steel beams!' - Olli Uikkanen",
                                "'A failure is not always a mistake, it may simply be the circumstances. The real mistake is to stop trying' - B. F. Skinner",
                                "'Failure doesn't mean you are a failure it just means you haven't succeeded yet.' - Robert H. Schuller",
                                "'The only real failure in life is not to be true to the best one know.' - Buddha",
                                "'You have to be able to accept failure to get better.' - LeBron James",
                                "'For you Mason, not for me!' - Victor Reznov, as he f***ing died",
                                "'Success is not a good teacher, failure makes you humble.' - Shah Rukh Khan",
                                "'Failure is simply the opportunity to begin again, this time more intelligently.' - Henry Ford",
                                "'There are no secrets to success. It is the result of preparation, hard work, and learning from failure.' - Colin Powell",
                                "'Success is the result of perfection, hard work, learning from failure, loyalty, and persistence.' - Colin Powell",
                                "'Kehoni on temppeli.' - Antton Jokinen",
                                "'Päiväni on mittaamaton ja pettymykseni on pilalla!' - Anssi Remes",
                                "Failure precedes success.",
                                "'Nitkutusnuppini nousahtelee!' - Elias Massa",
                                "'Sabreurs - A Noble Duel is now out on Steam for $8,99.' - Olli Uikkanen",
                                "'Täähän se ois!' - Antton Jokinen",
                                "'What is not saved will be lost!' - Nintendo",
                                "'Mun pitää kirjotaa tää ylös - pääni ei tomi oikein hyvin.' - Anssi Remes, 23.44 on a Saturday night",
                                "'Tää on se parempi matopeli.' - Anssi Remes",
                                "'Kuoleppa nopee!' - Kristian Kallio",
                                "'Lapamatohan siellä tykittää menemään.' - Kristian Kallio",
                                "'Verinen tamppooni paksusuolessa.' - Sipi Raussi",
                                "'Missä post-processing efektit?' - Janne Viitala",
                                "'Quote!' - Sipi Raussi",
                                "'En tarvitse kynää enää!' - Anssi Remes",
                                "'Onsks se enää vai enään?' - Anssi Remes",
                                "'Unityn default outline classi on ihan p****.' - Anssi Remes",
                                "'Äh, mä en tiiä mitä mää laitan tähän.' - Elias Massa"};
    }

    private void Update()
    {
        if (!lockInput)
        {
            PlayerControls();
            ForceMove();
        }
        else if (lockInput && cameraResetting)
        {
            if (((MainCamera.GetComponent<CameraController>().TargetPosition
                + MainCamera.GetComponent<CameraController>().PositionSetting)
                - MainCamera.transform.position).magnitude
                <= MainCamera.GetComponent<CameraController>().PositionMargin)
            {
                lockInput = false;
                cameraResetting = false;
            }
        }

        if (gameOver)
        {
            if (Tree.transform.localScale.x < finalSize * 0.03f)
            {
                Tree.transform.localScale += Tree.transform.localScale * 1.1f * Time.deltaTime;
            }
        }

        //Restart
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void Die()
    {
        --lives;
        finalSize += RootParts;
        RootParts = 0;
        MovementDirection = Directions.Null;
        if (lives <= 0)
        {
            GameOver();
            MainCamera.GetComponent<CameraController>().TargetPosition = Vector3.zero + new Vector3(0, 10, -10);
        }
        else
        {
            currentPosition = Vector3.zero;
            MainCamera.GetComponent<CameraController>().TargetPosition = Vector3.zero;
            cameraResetting = true;
        }
    }

    public void Ready()
    {
        ++RootParts;
        timeUntilMove = ForceMoveDelay;
        lockInput = false;
        currentPosition = nextPosition;
    }

    private void ForceMove()
    {
        if (!lockInput)
        {
            if (timeUntilMove < 0 && MovementDirection != Directions.Null)
            {
                if (MovementDirection == Directions.Right)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(2, 0, 0);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Right;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, -90)));
                }
                else if (MovementDirection == Directions.Left)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(-2, 0, 0);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Left;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
                }
                else if (MovementDirection == Directions.Up)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(0, 0, 2);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Up;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(90, 0, 0)));
                }
                else if (MovementDirection == Directions.Down)
                {
                    lockInput = true;
                    nextPosition = currentPosition + new Vector3(0, 0, -2);
                    MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                    MovementDirection = Directions.Down;
                    Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
                }
            }
            else if (timeUntilMove > 0)
            {
                timeUntilMove -= Time.deltaTime;
            }
        }
    }

    private void GameOver()
    {
        gameOver = true;
        MiniMapCamera.SetActive(false);
        GameOverText.SetActive(true);
        TauntText.GetComponent<Text>().text = taunts[((int)Random.Range(0, taunts.Length) - 1)];
        ResetText.SetActive(true);
        MainCamera.GetComponent<CameraController>().GameOver();

        foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (gameObj.name == "RootNode(Clone)")
            {
                Instantiate(Plant, gameObj.transform.localPosition, transform.rotation);
            }
        }
    }

    private void PlayerControls()
    {
        //Horizontal movement
        if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") == 0)
        {
            //Right
            if (Input.GetAxis("Horizontal") > 0 && MovementDirection != Directions.Left)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(2, 0, 0);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Right;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, -90)));
            }
            //Left
            else if (Input.GetAxis("Horizontal") < 0 && MovementDirection != Directions.Right)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(-2, 0, 0);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Left;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(0, 0, 90)));
            }
        }
        //Vertical movement
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") != 0)
        {
            //Up
            if (Input.GetAxis("Vertical") > 0 && MovementDirection != Directions.Down)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(0, 0, 2);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Up;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(90, 0, 0)));
            }
            //Down
            else if (Input.GetAxis("Vertical") < 0 && MovementDirection != Directions.Up)
            {
                lockInput = true;
                nextPosition = currentPosition + new Vector3(0, 0, -2);
                MainCamera.GetComponent<CameraController>().TargetPosition = nextPosition;
                MovementDirection = Directions.Down;
                Instantiate(RootPrefab, currentPosition, Quaternion.Euler(new Vector3(-90, 0, 0)));
            }
        }
    }
}
