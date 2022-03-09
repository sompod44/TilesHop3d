using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PLayerController : MonoBehaviour
{
    //public GameObject goTrail;

    [Header("Control Variables Start")]
    [SerializeField] private int touchYthreshold;
    [SerializeField] private int touchXthreshold;
    [SerializeField] private float objectSmoothSpeed;
    [SerializeField] private float timeForEveryMove;
    [SerializeField] private float ballJumpingSpeed;
    [SerializeField] private float BallFallingSpeed;

    [Header("Animation controlling variable")]
    [SerializeField] private float TileDownAnimValue;
    [SerializeField] private float TileDownAnimSec;
    [SerializeField] private float TileScaleAnimValue;
    [SerializeField] private float TileScaleAnimsec;

    [Header("Public UI components")]
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtDimondScore;
    public GameObject goGameOverPanel;
    public GameObject godimonCount;
    public GameObject goScoreCount;
    public Button btnContinue;

    private bool isDraging;
    private Vector2 vecStartTouchPos;
    private Vector2 vecMovingTouchPos;
    private Vector2 vecMovingTouchDeltaPos;
    private Vector2 vecRealScreenSize;
    private bool triggerBallJump;
    private int score;
    private int dimondscore;
    private bool isStartingJump;
    private bool isStartingFall;
    private bool BlockJumpAndFall;
    private Transform currentTile;
    GameObject goCurrentTileZoomEffect;
    private bool isMovingTweenComplete;
    public static PLayerController instance;


    // Start is called before the first frame update
    void Start()
    {
        isDraging = false;
        triggerBallJump = false;
        isStartingJump = false;
        isStartingFall = false;
        isMovingTweenComplete = true;
        BlockJumpAndFall = true;
        score = 0;
        vecRealScreenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        btnContinue.onClick.AddListener(()=> { ContinueButtonCallBack(); });

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance = null;
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DDOL.instance.gameStart)
        {
            BallMovement(); //This method for controlling ball left and right movement.
            if(!triggerBallJump)
            {
                triggerBallJump = true;
                BallJump();
            }

            if(isStartingJump && BlockJumpAndFall)
            {
                if(transform.position.y < 4f)
                {
                    //goTrail.SetActive(true);
                    transform.position = new Vector3(transform.position.x, transform.position.y + ballJumpingSpeed, transform.position.z);
                }
                
            }

            if(isStartingFall && BlockJumpAndFall)
            {
                if(transform.position.y > 0.79f)
                {
                    //goTrail.SetActive(false);
                    transform.position = new Vector3(transform.position.x, transform.position.y - ballJumpingSpeed, transform.position.z);
                }
                else
                {
                    if(transform.position.x > currentTile.position.x - 1.6f && transform.position.x < currentTile.position.x + 1.6f)
                    {
                        goCurrentTileZoomEffect = currentTile.GetComponent<TileController>().goZoomEffect;
                        goCurrentTileZoomEffect.SetActive(true);
                        goCurrentTileZoomEffect.transform.DOScale(new Vector3(1.9f, 1.9f, 1.9f), 0.4f)
                            .OnComplete(()=> { goCurrentTileZoomEffect.SetActive(false); })
                            ;
                        currentTile.DOScale(new Vector3(currentTile.localScale.x - TileScaleAnimValue, currentTile.localScale.y, currentTile.localScale.z - TileScaleAnimValue), TileScaleAnimsec);
                        currentTile.DOMoveY(currentTile.position.y - TileDownAnimValue, TileDownAnimSec)
                            .OnComplete(() => {
                                if(currentTile.GetComponent<TileController>().isDimond3dOpen)
                                {
                                    currentTile.GetComponent<TileController>().isDimond3dOpen = false;
                                    currentTile.GetComponent<TileController>().goDimond3d.SetActive(false);
                                    dimondscore++;
                                    txtDimondScore.text = dimondscore.ToString();
                                }
                                currentTile.DOScale(new Vector3(currentTile.localScale.x + TileScaleAnimValue, currentTile.localScale.y, currentTile.localScale.z + TileScaleAnimValue), TileScaleAnimsec);
                                currentTile.DOMoveY(currentTile.position.y + TileDownAnimValue, TileDownAnimSec)
                                    .OnComplete(() => {
                                        score++;
                                        txtScore.text = score.ToString();
                                    })
                                    ;
                            })
                            ;
                        isStartingFall = false;
                        isStartingJump = true;
                    }
                    else
                    {
                        isStartingJump = false;
                        isStartingFall = false;
                        DDOL.instance.gamePause = true;
                        transform.position += new Vector3(0f, -20f, 0f);
                        Time.timeScale = 0;
                        StopAllCoroutines();
                        godimonCount.SetActive(false);
                        goScoreCount.SetActive(false);
                        goGameOverPanel.SetActive(true);
                    }

                }
            }
            
        }
        


    }

    #region Custom_Methods
    private void BallMovement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                vecStartTouchPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                vecMovingTouchPos = touch.position;
                vecMovingTouchDeltaPos = touch.deltaPosition;
                if (vecMovingTouchPos.y - vecStartTouchPos.y < touchYthreshold)
                {
                    int direction = vecMovingTouchDeltaPos.x > 0 ? 1 : -1;
                    //transform.position = new Vector3(transform.position.x + (objectSmoothSpeed * direction), transform.position.y, transform.position.z);
                    if (direction > 0)
                    {
                        DOTween.Kill(transform);
                        //goTrail.SetActive(false);
                        //BlockJumpAndFall = false;
                        isMovingTweenComplete = false;
                        transform.DOMoveX(transform.position.x + vecMovingTouchDeltaPos.x * Time.deltaTime, timeForEveryMove)
                            .OnComplete(() =>
                            {
                                Vector3 newPos = transform.position;
                                newPos.x = Mathf.Clamp(transform.position.x, vecRealScreenSize.x, vecRealScreenSize.x * -1);
                                transform.position = newPos;
                                isMovingTweenComplete = true;
                                //BlockJumpAndFall = true;
                            });
                        //transform.Translate(Vector3.right * objectSmoothSpeed, Space.World);
                    }
                    else
                    {
                        DOTween.Kill(transform);
                        //goTrail.SetActive(false);
                        //BlockJumpAndFall = false;
                        isMovingTweenComplete = false;
                        transform.DOMoveX(transform.position.x + vecMovingTouchDeltaPos.x * Time.deltaTime, timeForEveryMove)
                            .OnComplete(() =>
                            {
                                Vector3 newPos = transform.position;
                                newPos.x = Mathf.Clamp(transform.position.x, vecRealScreenSize.x, vecRealScreenSize.x * -1);
                                transform.position = newPos;
                                isMovingTweenComplete = true;
                                //BlockJumpAndFall = true;
                            });
                        //transform.Translate(Vector3.left * objectSmoothSpeed, Space.World);
                    }
                }
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                Debug.Log("Touch End");
            }
        }
    }
    public void BallJump()
    {
        isStartingJump = true;
    }
    public void BallFall(float tileSpeed, Transform tile)
    {
        isStartingJump = false;
        isStartingFall = true;
        currentTile = tile;
    }
    private void ContinueButtonCallBack() //this button call when user press into continue button
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    #endregion
}
