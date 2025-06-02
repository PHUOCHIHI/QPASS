using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button addPlayerButton;
    [SerializeField] private Button displayPlayersButton;
    [SerializeField] private TextMeshProUGUI displayText;
    
    private List<PlayerInfo> players = new List<PlayerInfo>();
    private PlayerInfo currentPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (addPlayerButton != null)
            addPlayerButton.onClick.AddListener(AddNewPlayer);
        if (displayPlayersButton != null)
            displayPlayersButton.onClick.AddListener(DisplayAllPlayers);
    }

    public void AddNewPlayer()
    {
        if (string.IsNullOrEmpty(nameInput.text)) return;

        string playerName = nameInput.text;
        PlayerInfo newPlayer = new PlayerInfo(playerName);
        players.Add(newPlayer);
        currentPlayer = newPlayer;
        
        // Cập nhật thông tin trong ScoreKeeper
        ScoreKeeper.Instance.ResetScore(playerName, Random.Range(1, 10));
        
        nameInput.text = "";
        Debug.Log($"Đã thêm người chơi mới: {playerName}");
    }

    public void DisplayAllPlayers()
    {
        if (displayText != null)
        {
            string display = "Danh sách người chơi:\n\n";
            foreach (var player in players)
            {
                display += player.ToString() + "\n\n";
            }
            displayText.text = display;
        }
    }

    public PlayerInfo GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void UpdateCurrentPlayerScore(int score)
    {
        if (currentPlayer != null)
        {
            currentPlayer.UpdateScore(score);
            currentPlayer.UpdateLastPlayed();
            // Cập nhật điểm trong ScoreKeeper
            ScoreKeeper.Instance.ModifyScore(score);
        }
    }

    public void UpdateCurrentPlayerLevel(int level)
    {
        if (currentPlayer != null)
        {
            currentPlayer.UpdateLevel(level);
            currentPlayer.UpdateLastPlayed();
        }
    }

    public void UpdateCurrentPlayerHealth(float health)
    {
        if (currentPlayer != null)
        {
            currentPlayer.UpdateHealth(health);
            currentPlayer.UpdateLastPlayed();
        }
    }

    // Phương thức mới để lấy thông tin từ ScoreKeeper
    public void SyncWithScoreKeeper()
    {
        if (currentPlayer != null)
        {
            // Cập nhật điểm số từ ScoreKeeper
            currentPlayer.UpdateScore(ScoreKeeper.Instance.GetScore());
            // Cập nhật tên người chơi nếu có thay đổi
            if (currentPlayer.playerName != ScoreKeeper.Instance.GetUserName())
            {
                currentPlayer.playerName = ScoreKeeper.Instance.GetUserName();
            }
            currentPlayer.UpdateLastPlayed();
        }
    }

    // Phương thức để lấy thông tin người chơi hiện tại từ ScoreKeeper
    public void LoadCurrentPlayerFromScoreKeeper()
    {
        string userName = ScoreKeeper.Instance.GetUserName();
        if (!string.IsNullOrEmpty(userName))
        {
            // Tìm người chơi trong danh sách
            PlayerInfo existingPlayer = players.Find(p => p.playerName == userName);
            if (existingPlayer != null)
            {
                currentPlayer = existingPlayer;
            }
            else
            {
                // Tạo người chơi mới nếu chưa tồn tại
                currentPlayer = new PlayerInfo(userName);
                players.Add(currentPlayer);
            }
            // Cập nhật điểm số
            currentPlayer.UpdateScore(ScoreKeeper.Instance.GetScore());
        }
    }
} 