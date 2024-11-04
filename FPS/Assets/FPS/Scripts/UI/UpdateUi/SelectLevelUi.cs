using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.FPS.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelUi : MonoBehaviour
{
    private Transform _levelItem;
    private Transform Content;
    class LevelItem
    {
        private TextMeshProUGUI LevelText;
        private Text DescribeText ;
        private LoadSceneButton StartButton;
        private Button StartButton2;

        public void Init(Transform obj,LevelCfg cfg)
        {
            LevelText = obj.GetChild<TextMeshProUGUI>("LevelText");
            DescribeText = obj.GetChild<Text>("DescribeText");
            StartButton = obj.GetChild<LoadSceneButton>("Button");
            StartButton2 = obj.GetChild<Button>("Button");
            LevelText.SetText("level "+cfg.LevelId);
            string c = "";
            for (int i = 0; i < cfg.WaveCount.Count; i++)
            {
                c += cfg.WaveCount[i] + " ";
            }
            DescribeText.text=("波次: "+cfg.WaveCount.Count+"\n波: "+c);
            StartButton.SceneName = "MainSceneWuJin";
            StartButton2.onClick.AddListener(() =>
            {
                GameDataLevelData.instance.NowLevel = cfg.LevelId;
                StartButton.LoadTargetScene();
            });

        }
    }

    private List<LevelItem> _levelItems = new List<LevelItem>();

    private void Start()
    {
        Content = transform.GetChild("Content");
        _levelItem = transform.GetChild("LevelItem");
        for (int i = 0; i < GameDataLevelData.instance.LevelCfgs.Count; i++)
        {
            GameObject g= Instantiate(_levelItem.gameObject, Content);
            g.SetActive(true);
            LevelItem LevelItem1 = new LevelItem();
            LevelItem1.Init(g.transform,GameDataLevelData.instance.LevelCfgs[i]);
        }
    }
}
