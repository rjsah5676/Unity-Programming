using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameZone1 : MonoBehaviour
{
    private bool isStart = false;

    public AudioSource mainBGM;
    public AudioSource gameBGM;
    public GameObject baseBall;
    public GameObject soccerBall;
    public GameObject basketBall;
    public GameObject golfBall;

    private int baseCnt = -1;
    private int soccCnt = -1;
    private int baskCnt = -1;
    private int golfCnt = -1;

    private int cnt = 0;
    private float rndNum;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rndNum = Random.Range(0f, 100f);
        if (isStart)
        {
            cnt++;
            if (cnt % 7 == 0)
            {
                if(rndNum < 17f)
                    Instantiate(basketBall, SetPos(), Quaternion.identity);
                if (rndNum >= 17f && rndNum < 40f)
                    Instantiate(soccerBall, SetPos(), Quaternion.identity);
                if (rndNum >= 40f && rndNum < 70)
                    Instantiate(baseBall, SetPos(), Quaternion.identity);
                if (rndNum >= 70f && rndNum < 100)
                    Instantiate(golfBall, SetPos(), Quaternion.identity);
            }
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (!isStart)
        {
            if (coll.gameObject.name == "Player")
            {
                mainBGM.Stop();
                gameBGM.Play();
                isStart = true;
            }
        }
    }
    Vector3 SetPos()
    {
        float PosZ = Random.Range(-8f, 8f);
        float PosX = Random.Range(-110f, 110f);
        Vector3 Pos = new Vector3(1000 + PosX, 0, 448);
        Pos.y = 1280;
        return Pos;
    }
}
