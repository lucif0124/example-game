using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TMP ��� ����
using System.IO; // �ܺ��� ���� ���� ����
using UnityEngine.UI; // UI ����
using UnityEngine.SceneManagement;

// ������ �������� �帧�� ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public GameObject wordPrefab; // ������ �ܾ� ������Ʈ

    public TMP_InputField tmpInput; // �ܾ� �Է� â
    public TextMeshProUGUI tmpTimer; // Ÿ�̸� �ؽ�Ʈ
    public TextMeshProUGUI tmpScore; // ���� �ؽ�Ʈ
    public Slider sliderTimer; // Ÿ�̸� �����̴�

    public GameObject gameoverUI;
    public TextMeshProUGUI tmpLastScore;

    public int score; // ����

    public float createInterval = 1f; // �ܾ� ���� ����
    private float timerCreate; // ���� �ð�
    private float gameTimer = 60f; // ���� �ð�

    public float horizontalLimit = 7.5f; // x�� ���� ����
    public float createPosition = 6f; // ���� ����

    private bool isGameover; // ���ӿ��� ����

    private List<string> initializeWord = new List<string>(); // �ܾ� ����
    public List<TextMeshPro> wordList = new List<TextMeshPro>(); // �� ��� ���� �ܾ� ����Ʈ

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

    // ����ȭ���� Words.txt ������ �о ����Ʈ�� �߰�
    private void InitializeWords()
    {
        string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "Words.txt");

        // �ش� ��ο� ������ ������ �����ϴ���?
        if (File.Exists(path))
        {
            // ������ �����Ѵٸ� �д´�.
            string word = File.ReadAllText(path);

            // , ������ ����������� ���ڿ����� �ٽ� ���� �ϳ��� �ܾ�� �и��Ͽ� �����Ѵ�.
            string[] words = word.Split(',');

            // ���� ������� �ܾ���� �ܾ� ����Ʈ�� �߰��Ѵ�.
            foreach(string item in words)
            {
                initializeWord.Add(item);
            }
        }
        else // Words.txt ������ �������� �ʴ´ٸ�
        {
            Debug.Log("����ȭ�鿡 Words.txt ������ �������� �ʽ��ϴ�.");
        }
    }
    void Start()
    {
        InitializeWords();

        // onSubmit : �Է��� �Ϸ�Ǿ��� ���� �����Ѵ�.
        // ��ǲ�ʵ忡 �Էµ� ���ڿ��� �������ش�.
        tmpInput.onSubmit.AddListener(GetScore);
    }

    // ������ ��� ���
    private void GetScore(string text)
    {
        if (isGameover) return; // ���ӿ��� �� �Լ� �������� ����.

        // �Է� �� ��ǲ �ʵ带 ����ش�.
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

        // ���� �ð����� �ܾ ����
        // �ð��� �帧�� ����
        timerCreate += Time.deltaTime;

        // �귯�� �ð��� ���� �ֱ⺸�� ũ�ų� ���ٸ�
        if (timerCreate >= createInterval)
        {
            timerCreate = 0; // �ٽ� Ÿ�̸Ӹ� 0���� �ʱ�ȭ

            GameObject word = Instantiate(wordPrefab);
            // ���� ���� ������ �������� ����
            word.transform.position = new Vector3(Random.Range(-horizontalLimit, horizontalLimit), createPosition, 0);

            TextMeshPro tmp = word.GetComponent<TextMeshPro>();
            // �ܾ �������� �����´�.
            tmp.text = SelectWord();
            // �� ��� ����Ʈ�� �߰�
            wordList.Add(tmp);
        }
    }

    // Ÿ�̸� ���
    private void Timer()
    {
        // ���� 60�ʿ������� �ð��� ���� ����
        gameTimer -= Time.deltaTime;

        // ���ҵ� �ð��� UI�� ǥ��
        sliderTimer.value = gameTimer / 60;
        tmpTimer.text = gameTimer.ToString("F2");

        // �ð��� �������� �ʴٸ�
        if (gameTimer <= 0)
        {
            GameOver(); // ���� ����
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0; // �ð��� �����Ͽ� ������ �Ͻ������ǵ��� �Ѵ�.
        isGameover = true;

        gameoverUI.SetActive(true);
        tmpLastScore.text = "����� ������ ? " + score + "��!!";
    }

    // �ܾ �������� �����Ͽ� �������� ���
    private string SelectWord()
    {
        // ���� �ܾ��Ʈ�� �ܾ �ϳ��� ���ٸ�, �ٽ� �ܾ �߰��Ѵ�.
        if (initializeWord.Count == 0) InitializeWords();

        // ������ �ܾ �����ϱ� ���� �ε��� �̱�
        int randomIndex = Random.Range(0, initializeWord.Count);

        string word = initializeWord[randomIndex];

        // �ܾ�Ǯ���� ��� ���� �ܾ �����Ͽ� �ߺ��ܾ ����
        initializeWord.RemoveAt(randomIndex);

        return word; // ���� �����ܾ ��ȯ
    }
}
