﻿using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/**
 * Tạo objectPooler để tối ưu khi tạo và xóa nhiều đối tượng.
 * Cơ chế: 
 *     Trong hàm Start, tạo sẵn 1 ds chứa các obj cần dùng (setActive = false)
 *  -> tạo hàm private GameObject GetFishFromPool() để mở các object cần sử dụng (setActive = true)
 *  -> cái nào không dùng nữa thì tắt nó đi (setActive = false).
 */
public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    public GameObject[] fishPrefabs;
    //public List<GameObject> droppedFishes;
    public GameObject chosenFish = null;
    public Fish fishScript;

    //Tạo ds fishPool và số lượng
    private List<GameObject> fishPool = new List<GameObject>();
    private int poolSize = 10;

    private string oldTag;
    private string newTag;
    private int newTagIndex;



    private string[] hexColors = {
        "#FF0000", // Đỏ
        "#00FF00", // Xanh lá
        "#0000FF", // Xanh dương
        "#FFFF00", // Vàng
        "#FF00FF", // Hồng
        "#00FFFF", // Xanh cyan
        "#FFA500", // Cam
        "#800080", // Tím
        "#008000", // Xanh lục đậm
        "#808080", // Xám
        "#FFC0CB"  // Hồng nhạt
    };
    private int colorIndex;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Tạo random trc 10 con cá sau đó ẩn đi để sử dụng sau này.
        for (int i = 0; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, 4);
            GameObject fish = Instantiate(fishPrefabs[randomIndex]);
            fish.SetActive(false);
            fishPool.Add(fish);
        }

        


    }

    private void Update()
    {
        if (fishScript.inWater)
        {
            chosenFish = null;
        }
    }

    public void CreateFish(Vector3 spawnPosition)
    {
        GameObject newFish = GetFishFromPool(spawnPosition);
        newFish.transform.position = spawnPosition;
        newFish.SetActive(true);

        chosenFish = newFish;
        fishScript = newFish.GetComponent<Fish>();
        fishScript.prepareToDrop();


    }

    public void PrepareFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;

    }

    public void DropFish()
    {
        fishScript.dropped();
    }

    public void MoveFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;
        fishScript.prepareToDrop();
    }

    public void MergeFish(GameObject collision1, GameObject collision2)
    {
        if (collision1.tag == collision2.tag)
        {

            if (collision1.transform.position.y > collision2.transform.position.y)
            {
                // Kiểm tra tag của fish trước khi ẩn.
                // Nếu tag > 4 thì set lại các thuộc tính của fish.
                // Radome (1, 4) cho tag_id.
                // Dùng switch case để set lại các thuộc tính cho mỗi tag_id tương ứng.
                // return newFish sau đó setActive(false).

                //Ẩn đi
                ResetFish(collision1.tag, collision1);
                collision1.SetActive(false);


                collision2.transform.localScale *= 1.2f;
                ChangeTag(collision2.tag, collision2);
                ChangeColor(collision2, newTagIndex);
            }
        }
    }

    void ResetFish(string tag, GameObject newFish)
    {
        string oldTag = tag;
        string[] parts = oldTag.Split('_');

        if (parts.Length == 2)
        {
            int tagIndex = int.Parse(parts[1]);

            if (tagIndex > 4)
            {
                Debug.Log("Old tag: " +tag);
                int randomIndex = Random.Range(1, 5); //[1 -> 4]
                switch (randomIndex)
                {
                    case 1:
                        RandomFishAgain(newFish, new Vector3(1f, 1f, 1f), parts[0], randomIndex);
                        break;
                    case 2:
                        RandomFishAgain(newFish, new Vector3(1.2f, 1.2f, 1f), parts[0], randomIndex);
                        break;
                    case 3:
                        RandomFishAgain(newFish, new Vector3(1.4f, 1.4f, 1f), parts[0], randomIndex);
                        break;
                    case 4:
                        RandomFishAgain(newFish, new Vector3(1.6f, 1.6f, 1f), parts[0], randomIndex);
                        break;
                    default:
                        Debug.Log("Không phải số 1, 2, 3, 4");
                        break;
                    
                }

                Debug.Log("New tag: " + newFish.tag);

            }
        }
    }

    void RandomFishAgain(GameObject newFish, Vector3 fishScale, string parts, int randomIndex)
    {
        newFish.transform.localScale = fishScale;
        ChangeColor(newFish, randomIndex);
        string newTag = parts + "_" + randomIndex;
        newFish.tag = newTag;
    }

    void ChangeTag(string tag, GameObject newFish)
    {
        oldTag = tag;
        string[] parts = oldTag.Split('_');

        if (parts.Length == 2)
        {
            newTagIndex = int.Parse(parts[1]);
            newTagIndex++;
            newTag = parts[0] + "_" + newTagIndex;

            newFish.tag = newTag;
        }
    }
    public void ChangeColor(GameObject newFish, int newTagIndex)
    {
        Color newColor;
        colorIndex = newTagIndex;

        if (ColorUtility.TryParseHtmlString(hexColors[colorIndex - 1], out newColor))
        {
            newFish.GetComponent<SpriteRenderer>().color = newColor;
        }

    }
    private GameObject GetFishFromPool(Vector3 spawnPosition)
    {
        foreach (GameObject fish in fishPool)
        {
            if (!fish.activeInHierarchy) //Lấy con cá nào chưa được active.
            {
                return fish;
            }
        }

        //Tạo thêm 1 con mới vào fishPool nếu tất cả đã đc sd hết.
        int randomIndex = Random.Range(0, 4); //[0 -> 3]
        GameObject newFish = Instantiate(fishPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        fishPool.Add(newFish);
        return newFish;
    }

}
