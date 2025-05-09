using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표 : 다음 씬을 '비동기 방식'으로 로드하고 싶다.
    //        또한 로딩 진행률을 시각적으로 표현하고 싶다.
    //                              ㄴ % 프로그래스 바와 % 별 텍스트

    // 속성:
    // - 다음 씬 번호(인덱스)
    public int             NextSceneIndex = 2;

    // - 프로그래스 슬라이더바
    public Slider          ProgressSlider;

    // - 프로그래스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정된 씬을 비동기로 로드한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false;    // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while(ao.isDone == false)
        {
            // 비동기로 실행할 코드들
            Debug.Log(ao.progress);     // 0~1
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"({ao.progress * 100f}%) ";
            AddText(ao.progress);

            Debug.Log(ProgressText);
            // 서버와 통신해서 유저 데이터나 기획 데이터를 받아오면 된다.

            if (ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;      // 1프레임 대기
        }
    }

    private void AddText(float progress)
    {
        if (progress <= 0.1f)
        {
            ProgressText.text += "이럴 줄 알고 트윈테일 고정핀은 군용으로 했다.";
        }
        else if (progress <= 0.3f)
        {
            ProgressText.text += "총알보다 중요한 건... 머리 쪽 정확도야.";
        }
        else if (progress <= 0.5f)
        {
            ProgressText.text += "좀비들은 머리가 없으면 아무 생각도 못 하지. 나랑 똑같네.";
        }
        else if (progress <= 0.7f)
        {
            ProgressText.text += "생존 팁: 트윈테일은 좀비를 유인하는 데도 쓸 수 있다. (확실하진 않음)";
        }
        else
        {
            ProgressText.text += "문이 열린다. 트윈테일을 묶고, 총을 들고… 나가자.";
        }
    }
}
