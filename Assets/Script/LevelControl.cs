using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// 게임 레벨을 받아오기 위한 클래스
/// </summary>
public class LevelData
{
    public struct Range
    {
        public float min;
        public float max;
    }
    public float end_time;// 어느정도 시간 간격으로 변화시킬지에 대한 변수

    public Range spawnTime;

    public LevelData()
    {
        this.end_time = 15.0f;
        this.spawnTime.min = 0.0f;
        this.spawnTime.max = 0.0f;
    }
}

/// <summary>
/// 레벨 생성 정보를 담을 구조체
/// </summary>
public struct CreationInfo
{
    public struct Range
    {
        public float min;
        public float max;
    }

    public Range spawnTime;
};

public class LevelControl : MonoBehaviour {
    private List<LevelData> level_datas = new List<LevelData>();


    public CreationInfo cur_enemy;

    public int level = 0; // 난이도 초기화

    /// <summary>
    /// 레벨 텍스트 파일 입출력
    /// </summary>
    /// <param name="level_data_text"></param>
    public void loadLevelData(TextAsset level_data_text)
    {
        //텍스트 데이터를 문자열(스트링)로 가져온다.
        string level_texts = level_data_text.text;

        // 개행 코드 '\'마다 분할해서 배열에 넣는다.
        string[] lines = level_texts.Split('\n');

        // lines 내의 각 행에 대해서 차례로 처리하는 루프
        foreach (var line in lines)
        {
            if (line == "")//행이 빈 줄이면
            {
                continue;
            }
            //Debug.Log(line);
            string[] words = line.Split();//행 내의 워드를 배열에 저장한다.
            int n = 0;
            
            

            //LevelData형 변수를 생성한다.
            //현재 처리하는 행의 데이터를 넣어 간다.
            LevelData level_data = new LevelData();

            foreach (var word in words)
            {
                
                if (word.StartsWith("#"))
                    break;
                if (word == "")
                    continue;


                //n 값을 0,1, ~ ,3로 변화시켜 감으로써 4항목을 처리한다.
                //각 워드를 플롯값으로 변환하고 level_data에 저장한다.
                switch (n)
                {
                    case 0:
                        level_data.end_time = float.Parse(word);
                        break;

                    case 1:
                        level_data.spawnTime.min = float.Parse(word);
                        break;
                    case 2:
                        level_data.spawnTime.max = float.Parse(word);
                        break;
                }
                n++;
            }

            // 3항목 이상이 제대로 처리되었다면,
            if (n >= 3)
            {
                this.level_datas.Add(level_data);
                
            }
            else
            {
                if (n == 0)//n이 0이라면 = 주석을 처리한 경우이므로 아무것도 처리하지 않는다.
                { }
                else// 그 이외면 오류
                {
                    Debug.LogError("[LevelData] Out of parameter.\n");
                }
            }
        }

        // level_datas에 데이터가 하나도 없으면,
        if (this.level_datas.Count == 0)
        {
            // 데이터가 하나도 없으면 오류 메시지 출력
            Debug.LogError("[LevelData] has no data.\n");
            // level_datas에 기본 LevelData를 하나 추가해 둔다.
            this.level_datas.Add(new LevelData());
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void initialize()
    {

        // 이전, 현재, 다음 블록을 각각
        // clear_next_block()에 넘겨서 초기화한다.
        this.clear_enemy(ref this.cur_enemy);
       
    }

    /// <summary>
    /// 초기화
    /// </summary>
    /// <param name="enemy">구조체 초기화</param>
    private void clear_enemy(ref CreationInfo enemy)
    {
        enemy.spawnTime.min = 0;
        enemy.spawnTime.max = 0;
    }

    /// <summary>
    /// 실행 시간에 따른 레벨 데이터 삽입
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="passage_time"></param>
    public void update_level(ref CreationInfo enemy, float passage_time)
    {
        //float local_time = Mathf.Repeat(0, this.level_datas[this.level_datas.Count - 1].end_time);

        //float local_time = Mathf.Clamp(passage_time, 0, this.level_datas[this.level_datas.Count - 1].end_time);
        float local_time = Mathf.Repeat(passage_time, this.level_datas[this.level_datas.Count - 1].end_time);

        // 현재 레벨을 구한다.
        int i;
        for (i = 0; i < this.level_datas.Count - 1; i++)
        {
            if (local_time <= this.level_datas[i].end_time)
            {
                break;
            }
        }

        this.level = i;// 현재 레벨
       
       
        LevelData level_data;
        level_data = this.level_datas[this.level];
   
        // 현재 레벨에 해당하는 값들을 enemy 인스턴스에 대입

        enemy.spawnTime.min = level_data.spawnTime.min;
        enemy.spawnTime.max = level_data.spawnTime.max;
    }
    /// <summary>
    /// update_level 호출
    /// </summary>
    /// <param name="passage_time">게임 실행 시간</param>
    public void update(float passage_time)
    {
        this.update_level(ref this.cur_enemy, passage_time);
    }
}
