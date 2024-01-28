using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControl2 : MonoBehaviour
{
    public ScoreControl scoreControl;

    GameObject token;
    List<int> faceIndexes = new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 };
    public static System.Random rnd = new System.Random();
    public int shuffleNum = 0;
    int[] visibleFaces = { -1, -2 };

    public float gameTime = 60.0f; // 초기 시간 설정
    bool gameFailed = false;
    public int totalMatches = 4;
    private int currentMatches = 0;
    public Text resetCountText; // 카운트 점수 표시 Text
    public TMP_Text ClearText;
    public TMP_Text ClearScore;
    private int resetCount = 0; // 카운트 세기

    void Start()
    {
        // faceIndexes 리스트의 초기 길이를 저장
        int originalLength = faceIndexes.Count;

        // 토큰의 초기 y 위치
        float yPosition = 2.3f;

        // 토큰의 초기 x 위치
        float xPosition = -2.2f;

        // 3번 반복하여 토큰을 생성하고 배치
        for (int i = 0; i < 7; i++)
        {
            // faceIndexes 리스트에서 랜덤한 인덱스를 얻기 위한 난수 생성
            shuffleNum = rnd.Next(0, (faceIndexes.Count));

            // 새로운 토큰을 생성하고 위치를 설정
            var temp = Instantiate(token, new Vector3(xPosition, yPosition, 0), Quaternion.identity);

            // MainFront2 스크립트의 faceIndex 속성을 설정
            temp.GetComponent<MainFront2>().faceIndex = faceIndexes[shuffleNum];

            // 이미 사용된 faceIndex를 제거하여 중복 사용을 방지
            faceIndexes.Remove(faceIndexes[shuffleNum]);

            // xPosition 값을 증가하여 토큰을 가로로 이동
            xPosition = xPosition + 4;

            // originalLength의 절반까지 반복하면 y 위치를 변경하여 세로로 이동
            if (i == (originalLength / 2 - 2))
            {
                yPosition = -2.3f;
                xPosition = -6.2f;
            }
        }

        // 마지막 토큰에 남은 faceIndex를 할당
        token.GetComponent<MainFront2>().faceIndex = faceIndexes[0];
        ClearText.gameObject.SetActive(false); // 게임 오버 텍스트 숨기기
        ClearScore.gameObject.SetActive(false); // 최종 점수 텍스트 숨기기
    }


    public bool TwoCardsUp()
    {
        bool cardsUp = false;
        if (visibleFaces[0] >= 0 && visibleFaces[1] >= 0)
        {
            cardsUp = true;
        }
        return cardsUp;
    }

    public void AddVisibleFace(int index)
    {
        if (visibleFaces[0] == -1)
        {
            visibleFaces[0] = index;
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
        }
    }

    public void RemoveVisibleFace(int index)
    {
        if (visibleFaces[0] == index)
        {
            visibleFaces[0] = -1;
        }
        else if (visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
        }

        if (scoreControl != null)
        {
            scoreControl.IncrementResetCount(); // scoreControl 코드에 점수를 추가
            scoreControl.IncrementFinalCount(); // final 점수도 추가
            scoreControl.IncrementClearCount(); // 클리어 점수 추가
        }
    }

    public bool CheckMatch()
    {
        bool success = false;
        if (visibleFaces[0] == visibleFaces[1])
        {
            visibleFaces[0] = -1;
            visibleFaces[1] = -2;
            success = true;
            currentMatches++;

            // 모든 조합이 일치하였는지 확인
            if (currentMatches == totalMatches)
            {
                ShowClearText();
            }
        }
        return success;
    }

    /*private void MoveToNextStage()
    {
        SceneManager.LoadScene("Stage3");
    }*/

    private void ShowClearText() // 게임 완료시 클리어 문구 출력
    {
        ClearText.gameObject.SetActive(true); // 게임 오버 텍스트를 출력
        ClearScore.gameObject.SetActive(true); // 최종 점수 출력
        Debug.Log("Game Ended");
        Application.Quit();
    }


    private void Awake()
    {
        token = GameObject.Find("Token2");
    }
}
