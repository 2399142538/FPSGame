using System;
using DG.Tweening;
using TMPro;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    /// <summary>
    ///  [Header("Health component to track")]血条逻辑
    /// </summary>

    public class WorldspaceHealthBar : MonoBehaviour
    {
        [Header("要跟踪的健康组件")] public Health Health;

        [Header("显示血条的图像组件")]
        public Image HealthBarImage;
        
        [Header("显示护盾条状况的图像组件")]
        public Image ShieldBarImage;
        [Header("显示护盾条背景")]
        public Image ShieldBarImageBg;

        [Header("浮动健康栏枢轴变换")]
        public Transform HealthBarPivot;

        [Header("处于完全健康状态时，健康栏是否可见")]
        public bool HideFullHealthBar = true;


        
        [Header("血量text")]
        public TextMeshPro HpPro ;
        
        [Header("护盾text")]
        public TextMeshPro SheldPro ;
        
        [Header("飞的text")]
        public TextMeshPro FlyPro ;
        
        private int LastHealth = 0;
        private int LastShield = 0;

        private void Start()
        {
            LastHealth = Health.CurrentHealth;
            LastShield = Health.CurrentShield;
            if (HideFullHealthBar)HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount <= 0.99f||ShieldBarImage.fillAmount<=0.99f);
            SheldPro.SetText(Health.CurrentShield +"/"+ Health.MaxShield);
            HpPro.SetText(Health.CurrentHealth +"/"+ Health.MaxHealth);
        }

        void Update()
        {
            if (LastHealth!=Health.CurrentHealth)
            {
                int change = Health.CurrentHealth - LastHealth;

                LastHealth = Health.CurrentHealth;
                HealthBarImage.fillAmount = (float)Health.CurrentHealth / Health.MaxHealth;
                ShieldBarImageBg.gameObject.SetActive(Health.MaxShield>0&&Health.CurrentShield>0);
                if (HideFullHealthBar) HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount <= 0.99f||ShieldBarImage.fillAmount<=0.99f);
                HpPro.SetText(Health.CurrentHealth +"/"+ Health.MaxHealth);
                HpPro.gameObject.SetActive(Health.CurrentHealth>0);
            }
            if (LastShield!=Health.CurrentShield)
            {
                int change = Health.CurrentShield - LastShield;
                
                LastShield = Health.CurrentShield;
                ShieldBarImage.fillAmount = (float)Health.CurrentShield / Health.MaxShield;
                ShieldBarImageBg.gameObject.SetActive(Health.MaxShield>0&&Health.CurrentShield>0);
                if (HideFullHealthBar) HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount <= 0.99f||ShieldBarImage.fillAmount<=0.99f);
                SheldPro.SetText(Health.CurrentShield +"/"+ Health.MaxShield);
                SheldPro.gameObject.SetActive(Health.CurrentShield>0);
            }
            Vector3 v = Camera.main.transform.position;
            HealthBarPivot.LookAt(new Vector3( v.x,v.y,v.z));
        }


        void flyCount(float count,Color color )
        {
            if (count>0)
            {
                flyCount("+" + count, color);
            }
            else
            {
                flyCount( count, color);
            }
        }
        
        void flyCount(string text,Color color )
        {
            //TextMeshPro t=Instantiate(FlyPro,FlyPro.transform.parent).GetComponent<TextMeshPro>();
            //CanvasGroup t2=t.gameObject.AddComponent<CanvasGroup>();
            //t.gameObject.SetActive(true);
            //t.SetText(text);
            //t.color = color;
            //t2.transform.DOMoveY(t2.transform.position.y + 1, 2);
            //t2.DOFade(0,0.3f).SetDelay(1.5f);
            //Destroy(t.gameObject,4);
        }
        //飞伤害字
    }
}