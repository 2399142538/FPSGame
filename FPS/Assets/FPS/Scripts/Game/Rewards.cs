using System.Collections.Generic;

namespace Unity.FPS.Game
{
        public enum AttributeRewards
        {
                /// <summary>
                /// [0]默认移速DMS
                /// </summary>
                PDMS = 1,

                /// <summary>
                /// [1]默认血量DHP
                /// </summary>
                PDHP = 2,

                /// <summary>
                /// //[2]默认护盾DHPSd
                /// </summary>
                PDHPSd = 3,

                /// <summary>
                /// //[3]默认护盾恢复速度DHPSdRS
                /// </summary>
                PDHPSdRS = 4,

                
                /// <summary>
                /// //[4]增伤
                /// </summary>
                PZS = 5,

                
                /// <summary>
                /// //[5]增加从枪口子弹发射次数ShootCount
                /// </summary>
                PShootCount ,
                /// <summary>
                /// //[6]本枪枪口是否具有硬直
                /// </summary>
                PShootHardStraight ,
                /// <summary>
                /// //[7]本身是否免疫硬直
                /// </summary>
                PImmuneHardStraight ,

                //冲锋枪1 CF
                /// <summary>
                ///  //[0]攻击力系数
                /// </summary>
                CFATK,

                /// <summary>
                /// //[2]每秒重新装填的弹药量DEYRS
                /// </summary>
                CFDEYRS,

                /// <summary>
                /// 弹夹里子弹数量
                /// </summary>
                CFZDC,


                //散弹枪2 SD
                /// <summary>
                ///  //[0]攻击力系数
                /// </summary>
                SDATK,

                /// <summary>
                /// //[2]每秒重新装填的弹药量DEYRS
                /// </summary>
                SDDEYRS,

                /// <summary>
                /// 弹夹里子弹数量
                /// </summary>
                SDZDC,

                /// <summary>
                /// 散淡枪减速
                /// </summary>
                SDSlowDown,


                //炮枪3 DP
                /// <summary>
                ///  //[0]攻击力系数
                /// </summary>
                DPATK,

                /// <summary>
                /// //[2]每秒重新装填的弹药量DEYRS
                /// </summary>
                DPDEYRS,

                /// <summary>
                /// 弹夹里子弹数量
                /// </summary>
                DPZDC,


                //炮塔PT
                /// <summary>
                /// 炮塔血量DHP
                /// </summary>
                PTHP,

                /// <summary>
                /// 炮塔护盾DHPSd
                /// </summary>
                PTHPSd,

                /// <summary>
                /// 炮塔默认护盾恢复速度DHPSdRS
                /// </summary>
                PTPSdRS,

                /// <summary>
                /// 炮塔ATK攻击力
                /// </summary>
                PTATK,

                /// <summary>
                /// 炮塔每秒重新装填的弹药量DEYRS
                /// </summary>
                PTDEYRS,

                /// <summary>
                /// 炮塔]弹夹里子弹数量
                /// </summary>
                PTDPZDC,


                //小怪XG
                /// <summary>
                /// 小怪默认移速DMS
                /// </summary>
                XGDMS,

                /// <summary>
                /// 小怪血量DHP
                /// </summary>
                XGHP,

                /// <summary>
                /// 小怪护盾DHPSd
                /// </summary>
                XGHPSd,

                /// <summary>
                /// 小怪默认护盾恢复速度DHPSdRS
                /// </summary>
                XGPSdRS,

                /// <summary>
                /// 小怪ATK攻击力
                /// </summary>
                XGATK,

                /// <summary>
                /// 小怪每秒重新装填的弹药量DEYRS
                /// </summary>
                XGDEYRS,

                /// <summary>
                /// 小怪弹夹里子弹数量
                /// </summary>
                XGDPZDC,
                /// <summary>
                ///  [0]附加概率
                /// </summary>
    
                LightSFJGL,

                /// <summary>
                ///  [7]是否附加闪电阵
                /// </summary>
                LightFZhen,

                /// <summary>
                ///  //[1]闪电伤害
                /// </summary>
                LightFATK,

                /// <summary>
                ///  //[2]闪电区域半径
                /// </summary>
                LightFBJ,
                /// <summary>
                /// [3]闪电链伤害
                /// </summary>

                LightFLATK,
                /// <summary>
                /// [4]闪电链个数
                /// </summary>

                LightFLC,
                /// <summary>
                /// [5]是否携带斩杀
                /// </summary>

                LightFCKill,

                /// <summary>
                /// [6]斩杀线
                /// </summary>
                LightFKillCount,
                

        }
        public class AllRewards
        {

                
                
                public static List<AttributeRewards> PlayersRewards = new List<AttributeRewards>()
                { AttributeRewards.PDMS, AttributeRewards.PDHP    , AttributeRewards.PDHPSd  , AttributeRewards.PDHPSdRS, AttributeRewards.PZS ,AttributeRewards.PShootCount ,AttributeRewards.PShootCount,AttributeRewards.PShootHardStraight,AttributeRewards.PImmuneHardStraight };
                public static List<float> PlayersRewardsValue = new List<float>(){0.5f,0.5f,0.5f,0.5f,0.5f,1,1,1,1};
                public static List<bool> PlayersRewardsBuy = new List<bool>(){false,false,false,false,false,false,false,false,false};
                
                public static List<AttributeRewards> Gun1Rewards = new List<AttributeRewards>()
                { AttributeRewards.CFATK, AttributeRewards.CFDEYRS    , AttributeRewards.CFZDC, AttributeRewards.CFATK,AttributeRewards.CFZDC, AttributeRewards.CFATK,AttributeRewards.CFZDC};
                public static List<float> Gun1RewardsValue = new List<float>(){0.5f,0.5f,0.5f,0.5f,0.5f,0.5f,0.5f};
                public static List<bool> Gun1RewardsBuy = new List<bool>(){false,false,false,false,false,false,false};
                
                public static List<AttributeRewards> Gun2Rewards = new List<AttributeRewards>()
                { AttributeRewards.SDATK, AttributeRewards.SDDEYRS    , AttributeRewards.SDZDC,AttributeRewards.SDSlowDown };
                public static List<float> Gun2RewardsValue = new List<float>(){0.5f,0.5f,0.5f,1};
                public static List<bool> Gun2RewardsBuy = new List<bool>(){false,false,false,false};
                
                public static List<AttributeRewards> Gun3Rewards = new List<AttributeRewards>()
                { AttributeRewards.DPATK, AttributeRewards.DPDEYRS    , AttributeRewards.DPZDC };
                public static List<float> Gun3RewardsValue = new List<float>(){0.5f,0.5f,0.5f};
                public static List<bool> Gun3RewardsBuy = new List<bool>(){false,false,false};
                
                public static List<AttributeRewards> Enemy1Rewards = new List<AttributeRewards>() { AttributeRewards.PTHP, AttributeRewards.PTHPSd,AttributeRewards.PTPSdRS,AttributeRewards.PTATK,AttributeRewards.PTDEYRS,AttributeRewards.PTDPZDC, };
                public static List<float> Enemy1RewardsValue = new List<float>() { -0.5f,-0.5f,-0.5f,-0.5f,-0.5f,-0.5f };
                public static List<bool> Enemy1RewardsBuy = new List<bool>() { false,false,false,false,false,false, };
                
                public static List<AttributeRewards> Enemy2Rewards = new List<AttributeRewards>() {AttributeRewards.XGDMS,AttributeRewards.XGHP, AttributeRewards.XGHPSd,AttributeRewards.XGPSdRS,AttributeRewards.XGATK,AttributeRewards.XGDEYRS,AttributeRewards.XGDPZDC };
                public static List<float> Enemy2RewardsValue = new List<float>() { -0.5f,-0.5f,-0.5f,-0.5f,-0.5f,-0.5f,-0.5f };
                public static List<bool> Enemy2RewardsBuy = new List<bool>() { false,false,false,false,false,false,false };

                
                public static List<AttributeRewards> LightSRewards = new List<AttributeRewards>() {AttributeRewards.LightSFJGL,AttributeRewards.LightFZhen,AttributeRewards.LightFATK, AttributeRewards.LightFBJ,AttributeRewards.LightFLATK,AttributeRewards.LightFLC,AttributeRewards.LightFCKill,AttributeRewards.LightFKillCount };
                public static List<float> LightSRewardsValue = new List<float>() { 0.2f,1,0.5f,0.5f,0.5f,0.5f,1,0.5f };
                public static List<bool> LightSRewardsBuy = new List<bool>() { false,false,false,false,false,false,false,false };
                
                

                public static bool GetRewardsListIndex(int listIndex,int index)
                {
                        switch (listIndex)
                        {
                                case 1: return PlayersRewardsBuy[index];break;
                                case 2: return Gun1RewardsBuy[index];break;
                                case 3: return Gun2RewardsBuy[index];break;
                                case 4: return Gun3RewardsBuy[index];break;
                                case 5: return Enemy1RewardsBuy[index];break;
                                case 6: return Enemy2RewardsBuy[index];break;
                                case 7: return LightSRewardsBuy[index];break;
                                default: return false;
                        }
                }
                public static bool GiveRewardsListRewards(int listIndex,int index)
                {
                        switch (listIndex)
                        {
                                case 1: return PlayersRewardsBuy[index];break;
                                case 2: return Gun1RewardsBuy[index];break;
                                case 3: return Gun2RewardsBuy[index];break;
                                case 4: return Gun3RewardsBuy[index];break;
                                case 5: return Enemy1RewardsBuy[index];break;
                                case 6: return Enemy2RewardsBuy[index];break;
                                case 7: return LightSRewardsBuy[index];break;
                                default: return false;
                        }
                }
           
                public static void SetRewardsListIndex(int listIndex,int index)
                {
                        switch (listIndex)
                        {
                                case 1:  PlayersRewardsBuy[index]=true;break;
                                case 2:  Gun1RewardsBuy[index]   =true;break;
                                case 3:  Gun2RewardsBuy[index]   =true;break;
                                case 4:  Gun3RewardsBuy[index]   =true;break;
                                case 5:  Enemy1RewardsBuy[index] =true;break;
                                case 6:  Enemy2RewardsBuy[index] =true;break;
                                case 7:  LightSRewardsBuy[index] =true;break;
                        }
                }
                
                public static void GiveRewardsList(int listIndex,int index)
                {
                        switch (listIndex)
                        {
                        
                                case 1: GiveRewardsManager(PlayersRewards[index],PlayersRewardsValue[index]);break;
                                case 2: GiveRewardsManager(Gun1Rewards[index],Gun1RewardsValue[index]);break;
                                case 3: GiveRewardsManager(Gun2Rewards[index],Gun2RewardsValue[index]);break;
                                case 4: GiveRewardsManager(Gun3Rewards[index],Gun3RewardsValue[index]);break;
                                case 5: GiveRewardsManager(Enemy1Rewards[index],Enemy1RewardsValue[index]);break;
                                case 6: GiveRewardsManager(Enemy2Rewards[index],Enemy2RewardsValue[index]);break;
                                case 7: GiveRewardsManager(LightSRewards[index],LightSRewardsValue[index]);break;
                        }
                }
                public static Dictionary<AttributeRewards, string> RewardsDuiZhao = new Dictionary<AttributeRewards, string>()
            {                    
                    [AttributeRewards.PDMS]="移速",
                    [AttributeRewards.PDHP]="血量",
                    [AttributeRewards.PDHPSd]="护盾",
                    [AttributeRewards.PDHPSdRS]="护盾恢复速度",
                    [AttributeRewards.PZS]="增伤",
                    [AttributeRewards.PShootCount]="增加枪口射出的子弹数量",
                    [AttributeRewards.PShootHardStraight]=" 枪口具有硬直",
                    [AttributeRewards.PImmuneHardStraight]="免疫硬直",
                    [AttributeRewards.CFATK]="冲锋枪攻击力",
                    [AttributeRewards.CFDEYRS]="冲锋枪每秒装填弹药量",
                    [AttributeRewards.CFZDC]="冲锋枪弹夹里子弹数量",
                    [AttributeRewards.SDATK]="散弹枪攻击力",
                    [AttributeRewards.SDDEYRS]="散弹枪2每秒装填弹药量",
                    [AttributeRewards.SDZDC]="散弹枪2弹夹里子弹数量",
                    [AttributeRewards.SDSlowDown]="散淡枪减速效果",
                    [AttributeRewards.DPATK]="大炮攻击力",
                    [AttributeRewards.DPDEYRS]="大炮每秒装填弹药量",
                    [AttributeRewards.DPZDC]="大炮弹夹里子弹数量",
                    [AttributeRewards.PTHP]="炮塔血量",
                    [AttributeRewards.PTHPSd]="炮塔护盾",
                    [AttributeRewards.PTPSdRS]="炮塔护盾恢复速度",
                    [AttributeRewards.PTATK]="炮塔攻击力",
                    [AttributeRewards.PTDEYRS]="炮塔每秒装填弹药",
                    [AttributeRewards.PTDPZDC]="炮塔弹夹里子弹数量",
                    [AttributeRewards.XGDMS]="小怪移速",
                    [AttributeRewards.XGHP]="小怪血量",
                    [AttributeRewards.XGHPSd]="小怪护盾",
                    [AttributeRewards.XGPSdRS]="小怪护盾恢复速度",
                    [AttributeRewards.XGATK]="小怪攻击力",
                    [AttributeRewards.XGDEYRS]="小怪每秒装填弹药",
                    [AttributeRewards.XGDPZDC]="小怪弹夹里子弹数量",
                    
                    [AttributeRewards.LightSFJGL]="附加概率",
                    [AttributeRewards.LightFATK]="闪电伤害",
                    [AttributeRewards.LightFBJ]="闪电区域半径",
                    [AttributeRewards.LightFLATK]="闪电链伤害",
                    [AttributeRewards.LightFLC]="闪电链个数",
                    [AttributeRewards.LightFCKill]="是否携带斩杀",
                    [AttributeRewards.LightFKillCount]="斩杀线",
                    [AttributeRewards.LightFZhen]="附带闪电阵",
            };
            //AttributeRewards.PDMS]    
            //AttributeRewards.PDHP]    
            //AttributeRewards.PDHPSd]  
            //AttributeRewards.PDHPSdRS]
            //AttributeRewards.PZS]     
            //AttributeRewards.PShootCount]     
            //AttributeRewards.CFATK]   
            //AttributeRewards.CFDEYRS] 
            //AttributeRewards.CFZDC]   
            //AttributeRewards.SDATK]   
            //AttributeRewards.SDDEYRS] 
            //AttributeRewards.SDZDC]   
            //AttributeRewards.DPATK]   
            //AttributeRewards.DPDEYRS] 
            //AttributeRewards.DPZDC]   
            //AttributeRewards.PTHP]    
            //AttributeRewards.PTHPSd]  
            //AttributeRewards.PTPSdRS] 
            //AttributeRewards.PTATK]   
            //AttributeRewards.PTDEYRS] 
            //AttributeRewards.PTDPZDC] 
            //AttributeRewards.XGDMS]   
            //AttributeRewards.XGHP]    
            //AttributeRewards.XGHPSd]  
            //AttributeRewards.XGPSdRS] 
            //AttributeRewards.XGATK]   
            //AttributeRewards.XGDEYRS] 
            //AttributeRewards.XGDPZDC] 
            public static void GiveRewardsManager(AttributeRewards rewards,float Count)
            {
                    GameData g = GameData.instance;
                    switch (rewards)
                    {
                                  case  AttributeRewards.PDMS:
                                          g.PlayerData[0] += Count;  break;
                                   case AttributeRewards.PDHP    :     g.PlayerData[1] += Count;        break;               
                                   case AttributeRewards.PDHPSd  :     g.PlayerData[2] += Count;        break;               
                                   case AttributeRewards.PDHPSdRS:     g.PlayerData[3] += Count;        break;               
                                   case AttributeRewards.PZS     :     g.PlayerData[4] += Count;        break;               
                                   case AttributeRewards.PShootCount     :     g.PlayerData[5] += Count;        break;               
                                   case AttributeRewards.PShootHardStraight     :     g.PlayerData[6] += Count;        break;               
                                   case AttributeRewards.PImmuneHardStraight     :     g.PlayerData[7] += Count;        break;               
                                   case AttributeRewards.CFATK       :     g.Gun1Data[0] += Count;           break;          
                                   case AttributeRewards.CFDEYRS     :     g.Gun1Data[2] += Count;          break;           
                                   case AttributeRewards.CFZDC       :     g.Gun1Data[3] += Count;        break;             
                                   case AttributeRewards.SDATK          :     g.Gun2Data[0] += Count;         break;         
                                   case AttributeRewards.SDDEYRS        :     g.Gun2Data[2] += Count;         break;         
                                   case AttributeRewards.SDZDC          :     g.Gun2Data[3] += Count;         break;         
                                   case AttributeRewards.SDSlowDown          :     g.Gun2Data[4] += Count;         break;         
                                   case AttributeRewards.DPATK         :     g.Gun3Data[0] += Count;          break;         
                                   case AttributeRewards.DPDEYRS       :     g.Gun3Data[2] += Count;          break;         
                                   case AttributeRewards.DPZDC         :     g.Gun3Data[3] += Count;          break;         
                                   case AttributeRewards.PTHP           :     g.Enemy1Data[1] += Count;             break;   
                                   case AttributeRewards.PTHPSd         :     g.Enemy1Data[2] += Count;             break;   
                                   case AttributeRewards.PTPSdRS        :     g.Enemy1Data[3] += Count;             break;   
                                   case AttributeRewards.PTATK          :     g.Enemy1Data[4] += Count;             break;   
                                   case AttributeRewards.PTDEYRS        :     g.Enemy1Data[5] += Count;             break;   
                                   case AttributeRewards.PTDPZDC        :     g.Enemy1Data[6] += Count;             break;   
                                   case AttributeRewards.XGDMS           :     g.Enemy2Data[0] += Count;     break;          
                                   case AttributeRewards.XGHP            :     g.Enemy2Data[1] += Count;          break;     
                                   case AttributeRewards.XGHPSd          :     g.Enemy2Data[2] += Count;          break;     
                                   case AttributeRewards.XGPSdRS         :     g.Enemy2Data[3] += Count;          break;     
                                   case AttributeRewards.XGATK           :     g.Enemy2Data[4] += Count;          break;     
                                   case AttributeRewards.XGDEYRS         :     g.Enemy2Data[5] += Count;          break;     
                                   case AttributeRewards.XGDPZDC         :     g.Enemy2Data[6] += Count;          break;      
                                   
                                   case AttributeRewards.LightSFJGL      :     g.LightningBulletsData[0] += Count;     break;          
                                   case AttributeRewards.LightFATK       :     g.LightningBulletsData[1] += Count;          break;     
                                   case AttributeRewards.LightFBJ        :     g.LightningBulletsData[2] += Count;          break;     
                                   case AttributeRewards.LightFLATK      :     g.LightningBulletsData[3] += Count;          break;     
                                   case AttributeRewards.LightFLC        :     g.LightningBulletsData[4] += Count;          break;     
                                   case AttributeRewards.LightFCKill     :     g.LightningBulletsData[5] += Count;          break;     
                                   case AttributeRewards.LightFKillCount :     g.LightningBulletsData[6] += Count;          break;      
                                   case AttributeRewards.LightFZhen :     g.LightningBulletsData[7] += Count;          break;      
                    }
            }
    }
}