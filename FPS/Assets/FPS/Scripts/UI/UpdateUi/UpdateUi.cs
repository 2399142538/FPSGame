using System;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

class UpdateUiRewardsItem
{
    public GameObject _Item;
    public Text Describe;
    public Text Value;
    private Button _button;
    private Image _buttonImg;
    private bool _isBuy;
    private UpdateUiRewardsItem _updateUiRewardsItem;
    public void Init(GameObject item,AttributeRewards re,float Count,int listIndex,int index)
    {
        _Item = item;
        Describe = item.GetChild<Text>("Describe");
        Value = item.GetChild<Text>("Value");
        _button = item.GetChild<Button>("Button");
        _buttonImg = item.GetChild<Image>("Button");
        Describe.text = AllRewards.RewardsDuiZhao[re];
        Value.text = Count.ToString();
        _isBuy =  AllRewards.GetRewardsListIndex(listIndex, index);
        SetColor();
        _button.onClick.AddListener(() =>
        {
            _button.onClick.RemoveAllListeners();
            AllRewards.SetRewardsListIndex(listIndex, index);
            _isBuy =  AllRewards.GetRewardsListIndex(listIndex, index);
            AllRewards.GiveRewardsList(listIndex,index);
            SetColor();
        });
    }

    void SetColor()
    {

        _buttonImg.color=_isBuy?Color.green: Color.white;
    }
}
public class UpdateUi : MonoBehaviour
{
    private Transform PlayerContent;
    private Transform Enemy1Content;
    private Transform Enemy2Content;
    private Transform Gun1Content;
    private Transform Gun2Content;
    private Transform Gun3Content;
    private Transform LightSContent;
    private Transform _updateUiRewardsItem;

    private void Start()
    {
        _updateUiRewardsItem = transform.GetChild("UpdateUiRewardsItem");
        
        PlayerContent = transform.GetChild("PlayerContent");
        
        Enemy1Content = transform.GetChild("Enemy1Content");
        Enemy2Content = transform.GetChild("Enemy2Content");
        
        Gun1Content = transform.GetChild("Gun1Content");
        Gun2Content = transform.GetChild("Gun2Content");
        Gun3Content = transform.GetChild("Gun3Content");
        
        LightSContent = transform.GetChild("LightSContent");
        
 
        Init(AllRewards.PlayersRewards,AllRewards.PlayersRewardsValue,PlayerContent,1);
        
        Init(AllRewards.Gun1Rewards,AllRewards.Gun1RewardsValue,Gun1Content,2);
        Init(AllRewards.Gun2Rewards,AllRewards.Gun2RewardsValue,Gun2Content,3);
        Init(AllRewards.Gun3Rewards,AllRewards.Gun3RewardsValue,Gun3Content,4);
        Init(AllRewards.Enemy1Rewards,AllRewards.Enemy1RewardsValue,Enemy1Content,5);
        Init(AllRewards.Enemy2Rewards,AllRewards.Enemy2RewardsValue,Enemy2Content,6);
        Init(AllRewards.LightSRewards,AllRewards.LightSRewardsValue,Enemy2Content,7);

    }
    
    
    void Init(List<AttributeRewards> lists,List<float> Rewards,Transform PlayerContent,int index)
    {
        for (int i = 0; i <lists.Count; i++)
        {
            Transform g= Instantiate(_updateUiRewardsItem, PlayerContent);
            g.gameObject.SetActive(true);
            UpdateUiRewardsItem item = new UpdateUiRewardsItem();
            item.Init(g.gameObject,lists[i],Rewards[i],index,i);
        }
    }
    
}
