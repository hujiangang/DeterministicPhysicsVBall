using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameLayers
{
    Table = 6,
    Rails = 7,
    Pockets = 8,
    TableBounds = 9,
}


public class GameController : MonoBehaviour
{
    private Camera mainCam;

    /// <summary>
    ///  球杆组件.
    /// </summary>
    private Cuestick cuestick;

    /// <summary>
    ///  母球组件.
    /// </summary>
    private Cueball cueball;


    // 是否点击了桌子.
    private bool tableClicked;

    private bool clickAboveBall, clickRightOfBall;
    private bool cuestickRotated;

    private Vector3 prevMPos;



    private delegate void UserInput();
    private UserInput userInput;

    void Awake()
    {
        FindCuestickAndCueball();
        mainCam = Camera.main;

        if (Input.mousePresent){
            userInput = MouseControl;
        }else{
            userInput = TouchControl;
        }
    }

    public void Init(){
        FindCuestickAndCueball();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        userInput();
    }

    private void TouchControl(){

        
    }

    private void MouseControl(){

        if (Input.GetMouseButtonDown(0)){
            HandlePointerDown(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0)){
            HandlePointerUp(Input.mousePosition);
        }

        if (Input.GetMouseButton(0)){
            if (cuestickRotated){
                RotateCuestick(Input.mousePosition);
            }
            prevMPos = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GameEvents.InvokeBasicEvent(GameBasicEvent.PullCuestick);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameEvents.InvokeBasicEvent(GameBasicEvent.ReleaseCuestick);
        }
        
    }

    public void RotateCuestick(Vector3 pos)
    {
        Vector3 diff = pos - prevMPos;

        if (diff == Vector3.zero) return;

        float angle;
        if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
        {

            angle = diff.y;
            if (clickRightOfBall) angle *= -1;
        }
        else
        {
            angle = diff.x;
            if (!clickAboveBall) angle *= -1;
        }

        cuestickRotated = true;
        cuestick.Rotate(angle * Mathf.Abs(angle) * Time.deltaTime * 10);
    }


    /// <summary>
    /// 处理按下事件.
    /// </summary>
    /// <param name="pointerPos"></param>
    private void HandlePointerDown(Vector3 pointerPos){
        prevMPos = Input.mousePosition;
        if (Physics.Raycast(mainCam.ScreenPointToRay(pointerPos), out RaycastHit hit))
        {
            if (hit.collider.gameObject.layer == (int)GameLayers.TableBounds){
                tableClicked = true;
            }
        }

        cuestickRotated = true;
        Vector3 ballScreenPos = mainCam.WorldToScreenPoint(cueball.transform.position);
        clickAboveBall = pointerPos.y > ballScreenPos.y;
        clickRightOfBall = pointerPos.x > ballScreenPos.x;
    }

    /// <summary>
    /// 处理抬起事件.
    /// </summary>
    /// <param name="pointerPos"></param>
    private void HandlePointerUp(Vector3 pointerPos){

        if (!cuestickRotated && tableClicked){
            pointerPos.z = mainCam.transform.position.y - cueball.transform.position.y;
            cuestick.AimAt(pointerPos);
        }

        tableClicked = false;
        cuestickRotated = false;
    }

    /// <summary>
    /// 自动查找球杆和白球
    /// </summary>
    private void FindCuestickAndCueball()
    {
        GameObject _cueball = null;
        GameObject _cuestick = null;

        // 查找球杆
        if (_cuestick == null)
        {
            _cuestick = GameObject.FindWithTag(GameTags.GetTagName(GameTag.Cuestick));
            if (_cuestick == null)
            {
                // 尝试通过名称查找
                _cuestick = GameObject.Find(GameTags.GetTagName(GameTag.Cuestick));
            }
        }
        
        // 查找母球.
        if (_cueball == null)  
        {
            _cueball = GameObject.FindWithTag(GameTags.GetTagName(GameTag.Cueball));
            if (_cueball == null)
            {
                // 尝试通过名称查找
                _cueball = GameObject.Find(GameTags.GetTagName(GameTag.Cueball));
            }
        }

        if (_cuestick != null){
            if (!_cuestick.TryGetComponent<Cuestick>(out cuestick))
            {
                cuestick = _cuestick.AddComponent<Cuestick>();
            }
        }

       if (_cueball != null){
            if (!_cueball.TryGetComponent<Cueball>(out cueball)){
               cueball = _cueball.AddComponent<Cueball>();
            }
        }
        Debug.Log("CuestickManager: Found cuestick=" + (_cuestick != null) + ", cueball=" + (_cueball != null));
    }
}