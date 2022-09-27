using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : MonoBehaviour
{
    public Slider Slider;

    public void SetSliderPrecess(System.Object[] objs)
    {
        Slider.value = (float)objs[0];
    }

    private void OnEnable()
    {
        Slider.value = 0;
    }

    private void Start()
    {
        MsgSystem.instance.RegistMsgAction(MsgSystem.getted_level_message,SetSliderPrecess);
        MsgSystem.instance.RegistMsgAction(MsgSystem.getted_all_assets,LoadAllAbOver);
    }


    public void LoadAllAbOver(System.Object[] objs)
    {
        Slider.value = 0;
    }
    
}
