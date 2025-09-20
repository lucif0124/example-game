using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP 사용 위함
using System.IO; // 외부의 파일 관리 위함
using UnityEngine.UI; // UI 관리
using UnityEngine.SceneManagement;

// 게임의 전반적인 흐름을 관리
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public GameObject wordPrefab; // 생성할 단어 오브젝트

    public TMP_InputField tmpInput; // 단어 입력 창
    public TextMeshProUGUI tmpTimer; // 타이머 텍스트
    public TextMeshProUGUI tmpScore; // 점수 텍스트
    public Slider sliderTimer; // 타이머 슬라이더

    public GameObject gameoverUI;
    public TextMeshProUGUI tmpLastScore;

    public int score; // 점수

    public float createInterval = 1f; // 단어 생성 간격
    private float timerCreate; // 생성 시간
    private float gameTimer = 60f; // 제한 시간

    public float horizontalLimit = 7.5f; // x축 생성 제한
    public float createPosition = 6f; // 생성 높이

    private bool isGameover; // 게임오버 관리

    private List<string> initializeWord = new List<string>(); // 단어 세팅
    public List<TextMeshPro> wordList = new List<TextMeshPro>(); // 실 사용 중인 단어 리스트

    public void LoseScore(int penalty)
    {
        score -= penalty;
        tmpScore.text = "Score : " + score.ToString();
    }

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    // 바탕화면의 Words.txt 파일을 읽어서 리스트에 추가
    private void InitializeWords()
    {
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Words.txt");

        // 해당 경로에 실제로 파일이 존재하는지?
        if (File.Exists(path))
        {
            // 파일이 존재한다면 읽는다.
            string word = File.ReadAllText(path);

            // , 단위로 구분지어놓은 문자열들을 다시 각각 하나의 단어로 분리하여 보관한다.
            string[] words = word.Split(',');

            // 구분 지어놓은 단어들을 단어 리스트에 추가한다.
            foreach(string item in words)
            {
                initializeWord.Add(item);
            }
        }
        else // Words.txt 파일이 존재하지 않는다면
        {
            Debug.Log("바탕화면에 Words.txt 파일이 존재하지 않습니다.");
        }
    }
    void Start()
    {
        InitializeWords();

        // onSubmit : 입력이 완료되었을 때를 감지한다.
        // 인풋필드에 입력된 문자열을 전달해준다.
        tmpInput.onSubmit.AddListener(GetScore);
    }

    // 점수를 얻는 기능
    private void GetScore(string text)
    {
        if (isGameover) return; // 게임오버 시 함수 실행하지 않음.

        // 입력 후 인풋 필드를 비워준다.
        tmpInput.text = string.Empty;

        foreach(TextMeshPro item in wordList)
        {
            if(item.text == text)
            {
                wordList.Remove(item);

                score += 100;
                tmpScore.text = "Score : " + score.ToString();

                Destroy(item.gameObject);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameover)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        Timer();

        // 일정 시간마다 단어를 생성
        // 시간의 흐름을 정의
        timerCreate += Time.deltaTime;

        // 흘러간 시간이 생성 주기보다 크거나 같다면
        if (timerCreate >= createInterval)
        {
            timerCreate = 0; // 다시 타이머를 0으로 초기화

            GameObject word = Instantiate(wordPrefab);
            // 제한 구역 내에서 랜덤으로 생성
            word.transform.position = new Vector3(Random.Range(-horizontalLimit, horizontalLimit), createPosition, 0);

            TextMeshPro tmp = word.GetComponent<TextMeshPro>();
            // 단어를 랜덤으로 꺼내온다.
            tmp.text = SelectWord();
            // 실 사용 리스트에 추가
            wordList.Add(tmp);
        }
    }

    // 타이머 기능
    private void Timer()
    {
        // 최초 60초에서부터 시간을 점차 감소
        gameTimer -= Time.deltaTime;

        // 감소된 시간을 UI에 표현
        sliderTimer.value = gameTimer / 60;
        tmpTimer.text = gameTimer.ToString("F2");

        // 시간이 남아있지 않다면
        if (gameTimer <= 0)
        {
            GameOver(); // 게임 오버
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0; // 시간을 정지하여 게임이 일시정지되도록 한다.
        isGameover = true;

        gameoverUI.SetActive(true);
        tmpLastScore.text = "당신의 점수는 ? " + score + "점!!";
    }

    // 단어를 랜덤으로 선택하여 꺼내오는 기능
    private string SelectWord()
    {
        // 만약 단어리스트에 단어가 하나도 없다면, 다시 단어를 추가한다.
        if (initializeWord.Count == 0) InitializeWords();

        // 랜덤의 단어를 선택하기 위한 인덱스 뽑기
        int randomIndex = Random.Range(0, initializeWord.Count);

        string word = initializeWord[randomIndex];

        // 단어풀에서 방금 뽑은 단어를 제거하여 중복단어를 방지
        initializeWord.RemoveAt(randomIndex);

        return word; // 뽑은 랜덤단어를 반환
    }
}
