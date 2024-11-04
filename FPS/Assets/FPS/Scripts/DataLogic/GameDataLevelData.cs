using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  LevelCfg
{

   /// <summary>
   /// 等级
   /// </summary>
   public int LevelId;
   /// <summary>
   /// 波次
   /// </summary>
   public int WaveTimes=1;
   /// <summary>
   /// 一波多少个
   /// </summary>
   public List<int> WaveCount;
}
   public partial class GameDataLevelData :MonoBehaviour
   {
      public static GameDataLevelData instance;
      
      
      public int NowLevel=1;
      public int AllLevels=10;
      public List<LevelCfg> LevelCfgs = new List<LevelCfg>();

      private void Awake()
      {
         instance = this;
         DontDestroyOnLoad(this);
         for (int i = 0; i < AllLevels; i++)
         {
            List<int> a = new List<int>(){};
            for (int j = 0; j < i+5; j++)
            {
               a.Add((j+1)*3);
            }

            LevelCfgs.Add(new LevelCfg()
            {
               LevelId=i,WaveTimes=i+5,WaveCount=a
            });
         }
      }

      public LevelCfg GetLevelConfig()
      {
         return LevelCfgs[NowLevel];
      }
      
   }
