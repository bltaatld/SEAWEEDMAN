using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class StageButton : MonoBehaviour
{
    [SerializeField] private int chapterNum;
    [SerializeField] private int stageNum = -1;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private Sprite[] buttonSprites = new Sprite[3];

    private TextMeshProUGUI m_stageNum;
    private GameObject m_seaweedParent;
    private GameObject m_Popup;

    private void Awake()
    {
        if(stageNum == -1)
        {
            stageNum = this.transform.GetSiblingIndex();
        }

        m_stageNum = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_stageNum.text = $"{chapterNum}-{stageNum}";

        m_seaweedParent = this.transform.GetChild(1).gameObject;

        m_Popup = GameObject.FindObjectOfType<StageReadyPopup>(true).gameObject;

        if(stageNum == 1 && chapterNum == 1)
        {
            isUnlocked = true;
        }
    }

    private void Start()
    {
        UpdateButton();
    }

    public void UpdateButton()
    {
        //�����͸� Ȯ���� ��ȣ�ۿ밡�� ����, Ŭ���� ������ �ٸ� ��������Ʈ ����
        Button thisButton = this.GetComponent<Button>();
        Image thisImage = this.GetComponent<Image>();

        //can't play stage
        if (!isUnlocked)
        {
            thisButton.interactable = false;
            thisImage.sprite = buttonSprites[0];
            m_seaweedParent.SetActive(false);
            return;
        }

        //can play but its button's interactable is false
        if (!thisButton.interactable)
        {
            thisButton.interactable = true;
        }

        //int index = Array.IndexOf(SavePlayerInfo.instance.stageInfos, m_stageNum.text);
        int index = FindIndex();
        //this stage has already been cleared
        if (index != -1 && SavePlayerInfo.instance.stageInfos[index].rank > 0)
        {
            if (this.stageNum + 1 < this.transform.parent.childCount && !this.transform.parent.GetChild(stageNum + 1).GetComponent<StageButton>().isUnlocked)
            {
                StageButton nextStage = this.transform.parent.GetChild(stageNum + 1).GetComponent<StageButton>();
                nextStage.isUnlocked = true;
                nextStage.UpdateButton();
            }
            thisImage.sprite = buttonSprites[1];
            m_seaweedParent.SetActive(true);
            m_seaweedParent.GetComponent<SeaweedRecordSystem>().SetRecord(m_stageNum.text, index);
            return;
        }
        //else if (SavePlayerInfo.instance.stageInfos[index].rank == 0)
        //{
        //    m_seaweedParent.GetComponent<SeaweedRecordSystem>().SetRecord(m_stageNum.text, index);
        //}

        //can play
        thisImage.sprite = buttonSprites[2];
        m_seaweedParent.SetActive(false);
        return;
    }

    //Button-OnClick Method
    public void OpenPopup()
    {
        m_Popup.SetActive(true);
        m_Popup.GetComponent<StageReadyPopup>().SetPopup(m_stageNum.text);
    }

    private int FindIndex()
    {
        for(int i = 0; i < SavePlayerInfo.instance.stageInfos.Length; i++)
        {
            if (SavePlayerInfo.instance.stageInfos[i].clearStage == m_stageNum.text)
            {
                return i;
            }
        }
        return -1;
    }
}
