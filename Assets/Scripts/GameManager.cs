using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Ray ray;
    private Vector3 origin;
    private bool leftHoleCreated;
    private bool rightHoleCreated;
    private bool startGameReset;
    private float secondHoleTimer;

    [SerializeField] private GameObject vrRig;
    [SerializeField] private GameObject menuArea;
    [SerializeField] private GameObject playArea;
    [SerializeField] private GameObject holePrefab;
    [SerializeField] private GameObject startRayPos;
    [SerializeField] private GameObject startRayPosTwo;
    [SerializeField] private GameObject endRayPos;
    [SerializeField] private GameObject endRayPosTwo;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private float secondsToActivate = 2;
    [SerializeField] private int secondHoleTime = 10;

    public static GameManager instance;
    public int score = 0;
    public int timeToStart = 5;
    public int holesCreated = 0;
    public float remainedTimeToStart;
    public bool isPlaying;
    public bool hasGameEnded = false;
    public bool hasGameStarted = false;
    public bool showCredits = false;
    public bool showStartMenu = false;
    public bool showHoleCreated = false;

    [HideInInspector] public float sparedBeer;
    [HideInInspector]public float maxSparedBeer;
    [HideInInspector] public float playFillAmount;
    [HideInInspector] public float creditsFillAmount;
    [HideInInspector] public float resumeFillAmount;
    [HideInInspector] public float newGameFillAmount;
    [HideInInspector] public float startQuitFillAmount;
    [HideInInspector] public float pauseQuitFillAmount;
    [HideInInspector] public float gameOverQuitFillAmount;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        AudioManager.instance.PlaySound("Theme");
        showStartMenu = true;
        isPlaying = false;
        startGameReset = true;

    }


    private void Update()
    {
        ray.origin = Vector3.Lerp(ray.origin, ray.direction, .1f);
        if (!rightHoleCreated || !leftHoleCreated)
        {
            if (hasGameStarted)
            {
                secondHoleTimer += Time.deltaTime;
                if (secondHoleTimer >= secondHoleTime)
                {
                    secondHoleTimer = 0;
                    CreateHole();
                }
            }
        }

        if (hasGameStarted)
        {
            if (startGameReset)
            {
                startGameReset = false;
                StartCoroutine("CreateHoleCoroutine");
                remainedTimeToStart = timeToStart;
            }
            if (RenderSettings.fogDensity >= 0)
            {
                RenderSettings.fogDensity -= 0.5f * Time.deltaTime;
            }
            remainedTimeToStart -= Time.deltaTime;
        }

        UIActions();

        if (sparedBeer >= maxSparedBeer)
        {
            isPlaying = false;
            hasGameEnded = true;
        }
    }

    private void UIActions()
    {
        if (playFillAmount >= 1)
        {
            hasGameStarted = true;
            isPlaying = true;
            playFillAmount = 0;
        }
        if (resumeFillAmount >= 1)
        {
            ResumeGame();
            resumeFillAmount = 0;
        }
        if (startQuitFillAmount >= 1)
        {
            Application.Quit();
        }
        if (pauseQuitFillAmount >= 1)
        {
            Application.Quit();
        }
        if (gameOverQuitFillAmount >= 1)
        {
            Application.Quit();
        }
        if (newGameFillAmount >= 1)
        {
            SceneManager.LoadScene("Main");

        }
        if (creditsFillAmount >= 1)
        {
            showCredits = !showCredits;
            creditsFillAmount = 0;
        }
    }

    private void CreateHole()
    {
        if (startRayPos == null || startRayPosTwo == null)
        {
            return;
        }
        int choice = 0;
        if (!leftHoleCreated && !rightHoleCreated)
        {
            choice = Random.Range(0, 2);
        }
        else if (leftHoleCreated)
        {
            choice = 1;
        }
        else
        {
            choice = 0;
        }
        if (choice == 0)
        {
            leftHoleCreated = true;
            BoxCollider startCollider = startRayPos.GetComponent<BoxCollider>();
            MeshCollider endCollider = endRayPos.GetComponent<MeshCollider>();

            origin = startRayPos.transform.position;
            float xOffset = Random.Range(-startCollider.bounds.extents.x, startCollider.bounds.extents.x);
            float yOffset = Random.Range(-startCollider.bounds.extents.y, startCollider.bounds.extents.y);
            float zOffset = Random.Range(-startCollider.bounds.extents.z, startCollider.bounds.extents.z);
            origin += new Vector3(xOffset, yOffset, zOffset);
            ray.origin = origin;

            xOffset = Random.Range(-endCollider.bounds.extents.x, endCollider.bounds.extents.x);
            yOffset = Random.Range(-endCollider.bounds.extents.y, endCollider.bounds.extents.y);
            zOffset = Random.Range(-endCollider.bounds.extents.z, endCollider.bounds.extents.z);
            Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
            ray.direction = (endRayPos.transform.position) - startRayPos.transform.position;
        }
        else
        {
            rightHoleCreated = true;
            BoxCollider startCollider = startRayPosTwo.GetComponent<BoxCollider>();
            MeshCollider endCollider = endRayPosTwo.GetComponent<MeshCollider>();

            origin = startRayPosTwo.transform.position;
            float xOffset = Random.Range(-startCollider.bounds.extents.x, startCollider.bounds.extents.x);
            float yOffset = Random.Range(-startCollider.bounds.extents.y, startCollider.bounds.extents.y);
            float zOffset = Random.Range(-startCollider.bounds.extents.z, startCollider.bounds.extents.z);
            origin += new Vector3(xOffset, yOffset, zOffset);
            ray.origin = origin;

            xOffset = Random.Range(-endCollider.bounds.extents.x, endCollider.bounds.extents.x);
            yOffset = Random.Range(-endCollider.bounds.extents.y, endCollider.bounds.extents.y);
            zOffset = Random.Range(-endCollider.bounds.extents.z, endCollider.bounds.extents.z);
            Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
            ray.direction = (endRayPosTwo.transform.position) - startRayPosTwo.transform.position;
        }
        if (Physics.Raycast(ray, out RaycastHit hit, 40f, ~ignoreLayer))
        {
            print("preso");

            GameObject holeInstance = Instantiate(holePrefab, hit.point, Quaternion.identity, hit.transform);

            holeInstance.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * holeInstance.transform.rotation;
        }
        else
        {
            print("mancato");
        }

        holesCreated++;
        showHoleCreated = true;
        StartCoroutine("TurnOffHoleUICoroutine");
    }


    public void AddSparedBeer()
    {
        sparedBeer += 1;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void PrymaryButtonPress()
    {
        if (isPlaying)
        {
            return;
        }
        if (!hasGameStarted)
        {
            playFillAmount += Time.deltaTime / secondsToActivate;
            playFillAmount = Mathf.Clamp(playFillAmount, 0, 1);
        }
        else
        {
            if (hasGameEnded)
            {
                newGameFillAmount += Time.deltaTime / secondsToActivate;
                newGameFillAmount = Mathf.Clamp(newGameFillAmount, 0, 1);
            }
            else
            {
                resumeFillAmount += Time.deltaTime / secondsToActivate;
                resumeFillAmount = Mathf.Clamp(resumeFillAmount, 0, 1);
            }
        }
        startQuitFillAmount = 0;
        pauseQuitFillAmount = 0;
        gameOverQuitFillAmount = 0;
        creditsFillAmount = 0;
    }

    public void PrymaryButtonRelease()
    {
        if (isPlaying)
        {
            return;
        }
        if (!hasGameStarted && !hasGameEnded)
        {
            playFillAmount -= Time.deltaTime / secondsToActivate;
            playFillAmount = Mathf.Clamp(playFillAmount, 0, 1);
        }
        else
        {
            if (hasGameEnded)
            {
                newGameFillAmount -= Time.deltaTime / secondsToActivate;
                newGameFillAmount = Mathf.Clamp(newGameFillAmount, 0, 1);
            }
            else
            {
                resumeFillAmount -= Time.deltaTime / secondsToActivate;
                resumeFillAmount = Mathf.Clamp(resumeFillAmount, 0, 1);
            }
        }

    }

    public void SecondaryButtonPress()
    {
        if (isPlaying)
        {
            return;
        }
        if (!hasGameStarted && !hasGameEnded)
        {
            startQuitFillAmount += Time.deltaTime / secondsToActivate;
            startQuitFillAmount = Mathf.Clamp(startQuitFillAmount, 0, 1);
        }
        else
        {
            if (hasGameEnded)
            {
                gameOverQuitFillAmount += Time.deltaTime / secondsToActivate;
                gameOverQuitFillAmount = Mathf.Clamp(gameOverQuitFillAmount, 0, 1);
            }
            else
            {
                pauseQuitFillAmount += Time.deltaTime / secondsToActivate;
                pauseQuitFillAmount = Mathf.Clamp(pauseQuitFillAmount, 0, 1);
            }
        }
        newGameFillAmount = 0;
        playFillAmount = 0;
        creditsFillAmount = 0;
        resumeFillAmount = 0;
    }

    public void SecondaryButtonRelease()
    {
        if (isPlaying)
        {
            return;
        }
        if (!hasGameStarted && !hasGameEnded)
        {
            startQuitFillAmount -= Time.deltaTime / secondsToActivate;
            startQuitFillAmount = Mathf.Clamp(startQuitFillAmount, 0, 1);
        }
        else
        {
            if (hasGameEnded)
            {
                gameOverQuitFillAmount -= Time.deltaTime / secondsToActivate;
                gameOverQuitFillAmount = Mathf.Clamp(gameOverQuitFillAmount, 0, 1);
            }
            else
            {
                pauseQuitFillAmount -= Time.deltaTime / secondsToActivate;
                pauseQuitFillAmount = Mathf.Clamp(pauseQuitFillAmount, 0, 1);
            }
        }

    }

    public void PrymaryAxisPress()
    {
        if (isPlaying)
        {
            return;
        }
        if (!hasGameStarted && !hasGameEnded)
        {
            creditsFillAmount += Time.deltaTime / secondsToActivate;
            creditsFillAmount = Mathf.Clamp(creditsFillAmount, 0, 1);
        }
        pauseQuitFillAmount = 0;
        gameOverQuitFillAmount = 0;
        startQuitFillAmount = 0;
        playFillAmount = 0;
    }
    public void PrymaryAxisRelease()
    {
        if (isPlaying)
        {
            return;
        }
        creditsFillAmount -= Time.deltaTime / secondsToActivate;
        creditsFillAmount = Mathf.Clamp(creditsFillAmount, 0, 1);
    }

    public void MenuButtonPress()
    {
        if (hasGameEnded || !hasGameStarted)
        {
            print("1");
            return;
        }
        if (remainedTimeToStart > 0)
        {
            return;
        }
        if (isPlaying)
        {
            print("2");
            isPlaying = false;
            playArea.transform.position = vrRig.transform.position;
            vrRig.transform.position = menuArea.transform.position;
        }
        else
        {
            print("3");
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        if (playArea == null)
        {
            return;
        }
        isPlaying = true;
        vrRig.transform.position = playArea.transform.position;
    }

    private IEnumerator CreateHoleCoroutine()
    {
        yield return new WaitForSeconds(timeToStart);
        CreateHole();
    }

    private IEnumerator TurnOffHoleUICoroutine()
    {
        yield return new WaitForSeconds(7f);
        showHoleCreated = false;
    }
}