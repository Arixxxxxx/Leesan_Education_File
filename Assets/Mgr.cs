using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Mgr : MonoBehaviour
{

    [SerializeField] Animator cuttonAnim;
    [SerializeField] GameObject[] chapter;
    [SerializeField] Image NextBar;
    [SerializeField] Image Fillbar;


    bool nextOk;

    Action funtionAction;

    private void Awake()
    {
        //Screen.SetResolution(1920, 1080, false);

        Application.targetFrameRate = 60;
    }
    void Start()
    {
        Camera.main.aspect = 16f / 9f;
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        Chapter_1();
    }

    [SerializeField]
    float timer = 0;
    float time = 1.5f;
    void Update()
    {
        if (nextOk)
        {
            if (!NextBar.gameObject.activeSelf)
            {
                NextBar.gameObject.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                timer += Time.deltaTime;
                Fillbar.fillAmount = timer / time;

                if (timer > time)
                {
                    // 다음씬 연결
                    timer = 0;
                    Fillbar.fillAmount = 0;
                    nextStepPlay();
                }
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                Fillbar.fillAmount = 0;
                timer = 0;
            }
        }
        else
        {
            if (NextBar.gameObject.activeSelf)
            {
                NextBar.gameObject.SetActive(false);
            }
        }

    }

    private void Chapter_1()
    {
        cuttonAnim.GetComponent<SpriteRenderer>().color = Color.black;
        cuttonAnim.gameObject.SetActive(true);
        StartFade(2, () => chapter[0].SetActive(true));
    }

    private void nextStepPlay()
    {
        nextOk = false;

        int SceneNumber = 0;
        
        //찾아 현재 켜져잇는 스텝 인덱스
        for(int index =0;  index < chapter.Length; index++)
        {
            if (chapter[index].gameObject.activeSelf && index < chapter.Length - 1) 
            {
                SceneNumber = index + 1;
            }
            else if(chapter[index].gameObject.activeSelf && index == chapter.Length - 1)
            {
                SceneNumber = -1; //-1 이면 종료
            }
        }

        // 다음씬 번호 찾앗음 켜주러가야함
        if(SceneNumber != -1)
        {
            FadeScrrenWithAction(() => {
                chapter[SceneNumber - 1].SetActive(false);
                chapter[SceneNumber].SetActive(true);
            });
        }
        else // 종료
        {
            cuttonAnim.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            cuttonAnim.gameObject.SetActive(true);
            cuttonAnim.SetTrigger("End");
        }
    }

    public void FadeScrrenWithAction(Action funtion)
    {
        funtionAction = null;
        funtionAction = funtion;
        StartCoroutine(OnOff());
    }

    IEnumerator OnOff()
    {
        cuttonAnim.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        cuttonAnim.gameObject.SetActive(true);

        yield return null;

        cuttonAnim.SetTrigger("NextFade");

        AnimatorStateInfo animationState = cuttonAnim.GetCurrentAnimatorStateInfo(0);
        int targetStateHash = Animator.StringToHash("Base Layer.FadeOnOff");

        // while 루프에서 애니메이션 상태가 목표 상태로 변경될 때까지 대기
        while (animationState.fullPathHash != targetStateHash)
        {
            yield return null;
            animationState = cuttonAnim.GetCurrentAnimatorStateInfo(0);
        }

        while (cuttonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.35f)
        {
            yield return null;
        }

        funtionAction?.Invoke();


        while (cuttonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        cuttonAnim.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        cuttonAnim.gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        nextOk = true;
    }


    public void StartFade(float Time, Action funtion)
    {
        funtionAction = null;
        funtionAction = funtion;

        StartCoroutine(startFade(Time));
    }

    IEnumerator startFade(float Time)
    {
        funtionAction?.Invoke();
        yield return new WaitForSeconds(Time);
        cuttonAnim.SetTrigger("StartFade");

        AnimatorStateInfo animationState = cuttonAnim.GetCurrentAnimatorStateInfo(0);
        int targetStateHash = Animator.StringToHash("Base Layer.OnFade");

        // while 루프에서 애니메이션 상태가 목표 상태로 변경될 때까지 대기
        while (animationState.fullPathHash != targetStateHash)
        {
            yield return null;
            animationState = cuttonAnim.GetCurrentAnimatorStateInfo(0);
        }

        // 애니메이션 상태가 목표 상태로 변경되었는지 확인
        while (cuttonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        cuttonAnim.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        cuttonAnim.gameObject.SetActive(false);

        
        nextOk = true;
    }

}
