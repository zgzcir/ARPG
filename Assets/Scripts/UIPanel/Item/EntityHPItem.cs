using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EntityHPItem : BaseItem
{
    public Image imgHpGray;
    public Image imgHpRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hurtAni;
    public Text txtHurt;

    private int hpValue;
    private RectTransform rect;
    private Transform rootTrans;

    public void
        SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击 *" + critical + "*";
        criticalAni.Play();
    }

    public void
        SetDodge()
    {
        dodgeAni.Stop();
        dodgeAni.Play();
    }

    public void
        SetHurt(int hurt)
    {
        hurtAni.Stop();
        txtHurt.text = "-" + hurt;
        hurtAni.Play();
    }

    private float currentPrg=1;
    private float targetPrg=1;
    public void
        SetHpVal(int oldVal, int newVal)
    {
        currentPrg = oldVal * 1.0f / hpValue;
        targetPrg = newVal * 1.0f / hpValue;
        imgHpRed.fillAmount = targetPrg;
    }

    public void
        InitItemInfo(Transform
            trans, int hp)
    {
        rect = GetComponent<RectTransform>();
        rootTrans = trans;

        hpValue = hp;
        imgHpGray.fillAmount = 1;
        imgHpRed.fillAmount = 1;
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * Constans.GloableScreenRate;
        imgHpGray.fillAmount = currentPrg;
        UpdateGradient();
    }

    private void UpdateGradient()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Constans.AccelerHPSpeed*Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if(currentPrg>targetPrg)
        {
            currentPrg -= Constans.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg +=Constans.AccelerHPSpeed* Time.deltaTime;
        }

    }
        

}