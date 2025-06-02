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
        
    }

    public void YC4()
    {
        // sinh viên viết tiếp code ở đây
    }

    public void YC5()
    {
        // sinh viên viết tiếp code ở đây
    }
    public void YC6()
    {
        // sinh viên viết tiếp code ở đây
    }
    public void YC7()
    {
        // sinh viên viết tiếp code ở đây
    }
    void CalculateAndSaveAverageScoreByRegion()
    {
        // sinh viên viết tiếp code ở đây
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