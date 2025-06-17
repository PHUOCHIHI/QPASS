using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Unity.Jobs;

public class ASM_MN : Singleton<ASM_MN>
{
    public List<Region> listRegion = new List<Region>();
    public List<Players> listPlayer = new List<Players>();

    private void Start()
    {
        createRegion();
        YC1(); // thêm 1 người chơi test
        YC2(); // in ra danh sách
    }

    public void createRegion()
    {
        listRegion.Add(new Region(0, "VN"));
        listRegion.Add(new Region(1, "VN1"));
        listRegion.Add(new Region(2, "VN2"));
        listRegion.Add(new Region(3, "JS"));
        listRegion.Add(new Region(4, "VS"));
    }

    public string calculate_rank(int score)
    {
        if (score < 100)
        {
            return "Đồng";
        }
        else if (score < 500)
        {
            return "Bạc";
        }
        else if (score < 1000)
        {
            return "Vàng";
        }
        else
        {
            return "Kim cương";
        }
    }

    public void YC1()
    {
        if (ScoreKeeper.Instance == null)
        {
            Debug.LogError("ktt");
            return;
        }

        try
        {
            int id = ScoreKeeper.Instance.GetID();
            string name = ScoreKeeper.Instance.GetUserName();
            int score = ScoreKeeper.Instance.GetScore();
            int regionId = ScoreKeeper.Instance.GetIDregion();

            Region region = listRegion.Find(r => r.ID == regionId);
            if (region == null) 
            {
                region = listRegion[0]; 
            }

            Players newPlayer = new Players(id, name, score, region);
            listPlayer.Add(newPlayer);

            Debug.Log($"ADD: ID = {id}, Name = {name}, Score = {score}, Region = {region.Name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }

    public void YC2()
    {
        Debug.Log("Danh sach nguoi choi");
        foreach (Players p in listPlayer)
        {
            string rank = calculate_rank(p.Score);
            Debug.Log($"ID: {p.Id} | Name: {p.Name} | Score: {p.Score} | Region: {p.Region.Name} | Rank: {rank}");
        }
    }

    public void YC3()
    {
        if (ScoreKeeper.Instance == null) return;

        int currentScore = ScoreKeeper.Instance.GetScore();
        Debug.Log("Danh sách người chơi có điểm thấp hơn người chơi hiện tại:");
        
        var lowerScorePlayers = listPlayer.Where(p => p.Score < currentScore);
        foreach (var player in lowerScorePlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log($"ID: {player.Id} | Name: {player.Name} | Score: {player.Score} | Region: {player.Region.Name} | Rank: {rank}");
        }
    }

    public void YC4()
    {
        if (ScoreKeeper.Instance == null) return;

        int currentId = ScoreKeeper.Instance.GetID();
        var player = listPlayer.FirstOrDefault(p => p.Id == currentId);
        
        if (player != null)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log("Thông tin người chơi sau khi kết thúc màn chơi:");
            Debug.Log($"ID: {player.Id} | Name: {player.Name} | Score: {player.Score} | Region: {player.Region.Name} | Rank: {rank}");
        }
        else
        {
            Debug.Log("Không tìm thấy thông tin người chơi!");
        }
    }

    public void YC5()
    {
        Debug.Log("Danh sách người chơi theo thứ tự điểm giảm dần:");
        var sortedPlayers = listPlayer.OrderByDescending(p => p.Score);
        
        foreach (var player in sortedPlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log($"ID: {player.Id} | Name: {player.Name} | Score: {player.Score} | Region: {player.Region.Name} | Rank: {rank}");
        }
    }

    public void YC6()
    {
        Debug.Log("Top 5 người chơi có điểm thấp nhất:");
        var lowestScorePlayers = listPlayer.OrderBy(p => p.Score).Take(5);
        
        foreach (var player in lowestScorePlayers)
        {
            string rank = calculate_rank(player.Score);
            Debug.Log($"ID: {player.Id} | Name: {player.Name} | Score: {player.Score} | Region: {player.Region.Name} | Rank: {rank}");
        }
    }

    public void YC7()
    {
        Thread bxhThread = new Thread(() =>
        {
            try
            {
                Dictionary<int, (int totalScore, int playerCount)> regionScores = new Dictionary<int, (int, int)>();

                foreach (Players player in listPlayer)
                {
                    int regionId = player.Region.ID;
                    if (!regionScores.ContainsKey(regionId))
                    {
                        regionScores[regionId] = (0, 0);
                    }
                    var current = regionScores[regionId];
                    regionScores[regionId] = (current.totalScore + player.Score, current.playerCount + 1);
                }

                string content = "Bang Xep Hang Region\n";
                content += "------------------------\n";

                foreach (var region in listRegion)
                {
                    if (regionScores.ContainsKey(region.ID))
                    {
                        var (totalScore, playerCount) = regionScores[region.ID];
                        double averageScore = (double)totalScore / playerCount;
                        content += $"Region: {region.Name}\n";
                        content += $"Số người chơi: {playerCount}\n";
                        content += $"Điểm trung bình: {averageScore:F2}\n";
                        content += "------------------------\n";
                    }
                }

                string filePath = Path.Combine(Application.dataPath, "bxhRegion.txt");
                File.WriteAllText(filePath, content);
                Debug.Log("Da Luu bxhRegion.txt");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Loi Khi Tinh Diem Trung Binh: {e.Message}");
            }
        });

        bxhThread.Name = "BXH";
        bxhThread.Start();
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        //try
        //{
        //    Dictionary<int, (int totalScore, int playerCount)> regionScores = new Dictionary<int, (int, int)>();

        //    foreach (Players player in listPlayer)
        //    {
        //        int regionId = player.Region.ID;
        //        if (!regionScores.ContainsKey(regionId))
        //        {
        //            regionScores[regionId] = (0, 0);
        //        }
        //        var current = regionScores[regionId];
        //        regionScores[regionId] = (current.totalScore + player.Score, current.playerCount + 1);
        //    }

        //    string content = "Bang Xep Hang Region\n";
        //    content += "------------------------\n";

        //    foreach (var region in listRegion)
        //    {
        //        if (regionScores.ContainsKey(region.ID))
        //        {
        //            var (totalScore, playerCount) = regionScores[region.ID];
        //            double averageScore = (double)totalScore / playerCount;
        //            content += $"Region: {region.Name}\n";
        //            content += $"So Nguoi Choi: {playerCount}\n";
        //            content += $"Diem Trung Binh: {averageScore:F2}\n";
        //            content += "------------------------\n";
        //        }
        //    }

        //    string filePath = Path.Combine(Application.dataPath, "bxhRegion.txt");
        //    File.WriteAllText(filePath, content);
        //    Debug.Log("Da Luu bxhRegion.txt");
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError($"Loi Khi Tinh Diem Trung Binh: {e.Message}");
        //}
    }
}

[SerializeField]
public class Region
{
    public int ID;
    public string Name;
    public Region(int ID, string Name)
    {
        this.ID = ID;
        this.Name = Name;
    }
}

[SerializeField]
public class Players
{
    public int Id;
    public string Name;
    public int Score;
    public Region Region;
    public Players(int id, string name, int score, Region region)
    {
        this.Id = id;
        this.Name = name;
        this.Score = score;
        this.Region = region;
    }
}