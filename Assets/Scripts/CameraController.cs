using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private IUserInput pi;
    public float horizontalSpeed = 100;
    public float verticalSpeed = 80;
    public float cameraDampValue = 0.5f;
    public Image lockDot;
    public bool lockState;
    public bool isAI = false;

    private GameObject playerHandle;//改變playerHandel的 y 值
    private GameObject cameraHandle;//改變cameraHandel的 x 值
    private float tempEulerx; //因爲要clamp 所以單獨用一個變量處理
    private GameObject modle;
    private GameObject camera;

    private Vector3 cameraDampVelocity;
    public LockTarget lockTarget;//鎖定物件


    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerx = 20;

        modle = playerHandle.GetComponent<ActorController>().modle;
        pi = playerHandle.GetComponent<IUserInput>();

        if (!isAI)
        {
            camera = Camera.main.gameObject;
            lockDot.enabled = false;
            // Cursor.lockState = CursorLockMode.Locked;//鼠標隱藏
        }



        lockState = false;



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {

            Vector3 tempModleEuler = modle.transform.eulerAngles;//記錄當前model的旋轉

            //Rotate(轉動軸，轉動角度)
            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.deltaTime);//鏡頭水平移動

            tempEulerx -= pi.Jup * verticalSpeed * Time.deltaTime;//使用-=來改變水平的旋轉量
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 30);//再將數值clamp住

            //localEulerAngles 自身旋轉值  EulerAngles 該層級的旋轉值
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);//賦值到cameraHandle的x

            modle.transform.eulerAngles = tempModleEuler;//因爲在水平移動時 會順帶改變modle的旋轉 所以重新賦值記錄的旋轉
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - modle.transform.position;//取得從modle到目標的向量
            tempForward.y = 0;//y清零
            playerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        if (!isAI)
        {
            // 追尾效果
            camera.transform.position =
            Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);

            //camera.transform.eulerAngles = transform.eulerAngles;
            camera.transform.LookAt(cameraHandle.transform);

        }


    }

    private void Update()
    {
        if (lockTarget != null)
        {
            if (!isAI)
            {
                //鎖定ui 位置為target的半高
                lockDot.rectTransform.position =
                 Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            }

            if (Vector3.Distance(modle.transform.position, lockTarget.obj.transform.position) > 10.0f)
            {
                LockProcessA(null, false, false, isAI);
            }


            if (lockTarget.am != null && lockTarget.am.sm.isDie)
            {
                LockProcessA(null, false, false, isAI);
            }

        }

    }
    private void LockProcessA(LockTarget _locktarget, bool _lockDotEnable, bool _lockState, bool _isAI)
    {
        lockTarget = _locktarget;
        if (!isAI)
        {
            lockDot.enabled = _lockDotEnable;
        }

        lockState = _lockState;

    }
    public void LockUnlock()
    {

        Vector3 modelOrigin1 = modle.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + modle.transform.forward * 5.0f;


        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), modle.transform.rotation, LayerMask.GetMask(isAI ? "Player" : "Enemy"));

        if (cols.Length == 0)
        {
            LockProcessA(null, false, false, isAI);
        }
        else
        {
            foreach (var col in cols)
            {
                if (lockTarget != null && lockTarget.obj == col.gameObject)//如果鎖定是相同的
                {

                    LockProcessA(null, false, false, isAI);
                    break;
                }
                lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);//col.bounds.extents.y獲得col的半高
                LockProcessA(lockTarget, true, true, isAI);
                break;
            }


        }

    }



}
public class LockTarget
{
    public GameObject obj;
    public float halfHeight;
    public ActorManager am;

    public LockTarget(GameObject _obj, float _halfHeight)
    {
        obj = _obj;
        halfHeight = _halfHeight;
        am = obj.GetComponent<ActorManager>();
    }

}


