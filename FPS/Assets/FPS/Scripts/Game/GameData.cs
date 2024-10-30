using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Game
{

   public enum EnemyType
   {
      /// <summary>
      /// enemy1
      /// </summary>
      Turret,
      /// <summary>
      /// enemy2
      /// </summary>
      Monile
   }

   public enum GunType
   {
      /// <summary>
      /// 冲锋枪
      /// </summary>
      Other,
      /// <summary>
      /// 冲锋枪
      /// </summary>
      ChongFeng,
      /// <summary>
      /// 散淡枪
      /// </summary>
      SanDan,
      /// <summary>
      /// 炮
      /// </summary>
      PaoQiang,
      /// <summary>
      /// Enemy1小怪
      /// </summary>
      EnemyX,
      /// <summary>
      /// Enemy2炮塔
      /// </summary>
      EnemyT
   }


   public class GameData : MonoBehaviour
   {
      public static GameData instance;

      private void Awake()
      {
         DontDestroyOnLoad(this);
         instance = this;
      }
      
      [Header("攻速")] public float PlayerATS = 1;



      /// <summary>
      /// 玩家默认数据
      /// </summary>
      public List<float> PlayerDefData = new List<float>
      {
         1, //[0]默认移速DMS
         200, //[1]默认血量DHP
         200, //[2]默认护盾DHPSd
         3, //[3]默认护盾恢复速度DHPSdRS
         0, //[4]默认增伤
         1, //[5]增加从枪口子弹发射次数ShootCount
      };
      
      /// <summary>
      /// 玩家系数
      /// </summary>
      public List<float> PlayerData = new List<float>
      {
         0, //[0]默认移速DMS
         0, //[1]默认血量DHP
         0, //[2]默认护盾DHPSd
         0, //[3]默认护盾恢复速度DHPSdRS
         0, //[4]增伤ZS
         0, //[5]增加从枪口子弹发射次数
         0, //[6]本枪枪口是否具有硬直
         0, //[7]本身是否免疫硬直
      };

      /// <summary>
      /// [0]默认移速DMS
      /// [1]默认血量DHP
      /// [2]默认护盾DHPSd
      /// [3]默认护盾恢复速度DHPSdRS
      /// 
      /// [5]增加从枪口子弹发射次数
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public float GetPlayerMaxData(int index)
      {
         switch (index)
         {
            case 5:
               return PlayerDefData[index] + PlayerData[index];
         }
         return PlayerDefData[index] + PlayerData[index] * PlayerDefData[index];
      }


      //枪支属性
      
      //突击枪
      public List<float> Gun1DefData = new List<float>
      {
         10, //[0]ATK攻击力
         100,//[1]默认能量DEY
         16, //[2]每秒重新装填的弹药量DEYRS
         16, //[3]弹夹里子弹数量
      };
      //突击枪系数
      public List<float> Gun1Data = new List<float>
      {
         0, //[0]ATK攻击力
         0,//[1]默认能量DEY
         0, //[2]每秒重新装填的弹药量DEYRS
         0, //[3]弹夹里子弹数量
      };
         
      //散蛋枪
      public List<float> Gun2DefData = new List<float>
      {
         7, //[0]ATK攻击力
         100,//[1]默认能量DEY
         1.1f, //[2]每秒重新装填的弹药量DEYRS
         2, //[3]弹夹里子弹数量
      };
      //散蛋枪系数
      public List<float> Gun2Data = new List<float>
      {
         0, //[0]ATK攻击力
         0,//[1]默认能量DEY
         0, //[2]每秒重新装填的弹药量DEYRS
         0, //[3]弹夹里子弹数量
         0, //[4]散淡枪是否具有减速效果，不为0具有
      };  
      //炮枪
      public List<float> Gun3DefData = new List<float>
      {
         40, //[0]ATK攻击力
         100,//[1]默认能量DEY
         1, //[2]每秒重新装填的弹药量DEYRS
         4, //[3]弹夹里子弹数量
      };
      //炮枪系数
      public List<float> Gun3Data = new List<float>
      {
         0, //[0]ATK攻击力
         0,//[1]默认能量DEY
         0, //[2]每秒重新装填的弹药量DEYRS
         0, //[3]弹夹里子弹数量
      };
         
      
      
      [Header("攻击力1")] public float PlayerDefATK1 = 1;
      [Header("攻击力2")] public float PlayerDefATK2 = 1;
      [Header("攻击力3")] public float PlayerDefATK3 = 1;

      //3种枪攻击力

      
      /// <summary>
      /// Enemy1默认数据 炮塔
      /// </summary>
      public List<float> Enemy1DefData = new List<float>
      {
         1, //[0]默认移速DMS
         400, //[1]默认血量DHP
         400, //[2]默认护盾DHPSd
         3, //[3]默认护盾恢复速度DHPSdRS
         3, //[4]ATK攻击力
         5, //[5]每秒重新装填的弹药量DEYRS
         100, //[6]弹夹里子弹数量
      };
      
      /// <summary>
      /// Enemy1系数 炮塔
      /// </summary>
      public List<float> Enemy1Data = new List<float>
      {
         0, //[0]默认移速DMS
         0, //[1]默认血量DHP
         0, //[2]默认护盾DHPSd
         0, //[3]默认护盾恢复速度DHPSdRS
         0, //[4]ATK攻击力
         0, //[5]每秒重新装填的弹药量DEYRS
         0, //[6]弹夹里子弹数量
      };

      /// <summary>
      /// [0]默认移速DMS
      /// [1]默认血量DHP
      /// [2]默认护盾DHPSd
      /// [3]默认护盾恢复速度DHPSdRS
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public float GetEnemy1MaxData(int index) => Enemy1DefData[index] + Enemy1Data[index] * Enemy1DefData[index];
      
      /// <summary>
      /// Enemy2 小怪默认数据
      /// </summary>
      public List<float> Enemy2DefData = new List<float>
      {
         3.5f, //[0]默认移速DMS
         100, //[1]默认血量DHP
         100, //[2]默认护盾DHPSd
         3, //[3]默认护盾恢复速度DHPSdRS
         10, //[4]ATK攻击力
         0.5f, //[5]每秒重新装填的弹药量DEYRS
         15, //[6]弹夹里子弹数量
      };
      
      /// <summary>
      /// Enemy2
      /// </summary>
      public List<float> Enemy2Data = new List<float>
      {
         0, //[0]默认移速DMS
         0, //[1]默认血量DHP
         0, //[2]默认护盾DHPSd
         0, //[3]默认护盾恢复速度DHPSdRS
         0, //[4]ATK攻击力
         0, //[5]每秒重新装填的弹药量DEYRS
         0, //[6]弹夹里子弹数量
      };

      /// <summary>
      /// [0]默认移速DMS
      /// [1]默认血量DHP
      /// [2]默认护盾DHPSd
      /// [3]默认护盾恢复速度DHPSdRS
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public float GetEnemy2MaxData(int index) => Enemy2DefData[index] + Enemy2Data[index] * Enemy2DefData[index];


      public float GetATK(GunType type)
      {
         switch (type)
         {
            case GunType.Other :
               return 0;
               break;
            case GunType.ChongFeng :
               return Gun1DefData[0] + Gun1DefData[0] * (Gun1Data[0]+PlayerData[4]);
               break;
            case GunType.SanDan :
               return Gun2DefData[0] + Gun2DefData[0] * (Gun2Data[0]+PlayerData[4]);
               break;
            case GunType.PaoQiang :
               return Gun3DefData[0] + Gun3DefData[0] * (Gun3Data[0]+PlayerData[4]);
               break;
            case GunType.EnemyX :
               return Enemy2DefData[4] + Enemy2DefData[4] * Enemy2Data[4];
            case GunType.EnemyT :
               return Enemy1DefData[4] + Enemy1DefData[4] * Enemy1Data[4];
            default:
               return 0;
         }
      }
      /// <summary>
      /// 每秒重新装填的弹药量DEYRS  2
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      public float GetDEYRS(GunType type)
      {
         switch (type)
         {
            case GunType.Other :
               return 0;
               break;
            case GunType.ChongFeng :
               return Gun1DefData[2] + Gun1DefData[2]* Gun1Data[2];
               break;
            case GunType.SanDan :
               return Gun2DefData[2] + Gun2DefData[2]* Gun2Data[2];
               break;
            case GunType.PaoQiang :
               return Gun3DefData[2] + Gun3DefData[2]*Gun3Data[2];
               break;
            case GunType.EnemyX :
               return Enemy2DefData[5] + Enemy2DefData[5]*Enemy2Data[5];
            case GunType.EnemyT :
               return Enemy1DefData[5] + Enemy1DefData[5]*Enemy1Data[5];
            default:
               return 0;
         }
      }
      /// <summary>
      /// 获取弹夹数
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      public float GetDJC(GunType type)
      {
         switch (type)
         {
            case GunType.Other :
               return 0;
               break;
            case GunType.ChongFeng :
               return Gun1DefData[3] +  Gun1Data[3]*Gun1DefData[3];
               break;
            case GunType.SanDan :
               return Gun2DefData[3] +  Gun2Data[3]*Gun2DefData[3];
               break;
            case GunType.PaoQiang :
               return Gun3DefData[3] +  Gun3Data[3]*Gun3DefData[3];
               break;
            case GunType.EnemyX :
               return Enemy2DefData[6] +Enemy2Data[6]*Enemy2DefData[6];
            case GunType.EnemyT :
               return Enemy1DefData[6] + Enemy1Data[6]*Enemy1DefData[6];
            default:
               return 0;
         }
      }
      

      
   }
}