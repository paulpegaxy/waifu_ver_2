using System;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupConfirmIdleEarnUpgrade : MonoBehaviour
{
    [SerializeField] private Image imgCard;
    [SerializeField] private Image imgIconInCard;
    [SerializeField] private Image holderLevel;

    [SerializeField] private List<ConfirmIdleEarnPanel> listPanel;


    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtLevel;

    private DataIdleEarnUpgradeItem _data;

    private void OnEnable()
    {
        ConfirmIdleEarnPanelUnlock.OnUnlockIdleEarn += OnDoneUnlockCard;
    }

    private void OnDisable()
    {
        ModelApiUpgradeInfo.OnChanged -= ModelApiUpgradeInfoChanged;
        ConfirmIdleEarnPanelUnlock.OnUnlockIdleEarn -= OnDoneUnlockCard;
    }

    private void ModelApiUpgradeInfoChanged(ModelApiUpgradeInfo obj)
    {
        var data = FactoryApi.Get<ApiUpgrade>().Data.upgrade;
        var item = data.Find(x => x.current.id == _data.id);
        Reload(new DataIdleEarnUpgradeItem()
        {
            id = item.current.id,
            level = item.current.level,
            canUnlock = item.current.unlocked,
            cost = item.next.CostParse,
            typeCurrency = TypeResource.HeartPoint,
            profitPerHour = item.CurrentPointPerHour,
            profitAfter = item.NextPointPerHour,
            type = item.current.unlocked ? TypeIdleEarnCard.CAN_INTERACT : TypeIdleEarnCard.REQUIRE_CONDITION,
            conditionData = item.current.ConditionData
        });
    }

    public void InitShow(DataIdleEarnUpgradeItem data)
    {
        Reload(data);
        ModelApiUpgradeInfo.OnChanged += ModelApiUpgradeInfoChanged;
    }

    private void Reload(DataIdleEarnUpgradeItem data)
    {
        _data = data;
        var isUnlocked = data.level > 0;
        TypeConfirmIdleEarnPanel typePanel;
        if (isUnlocked)
            typePanel = TypeConfirmIdleEarnPanel.LevelUp;
        else
        {
            if ((data.canUnlock || data.conditionData == null) ||
                data.conditionData.typeCondition == TypeConditionIdleEarnUnlock.INVITE_FRIEND)
                typePanel = TypeConfirmIdleEarnPanel.Unlock;
            else
                typePanel = TypeConfirmIdleEarnPanel.UnlockSpecial;

        }

        listPanel.ForEach(x =>
        {
            x.refData.gameObject.SetActive(x.type == typePanel);
            if (x.refData.gameObject.activeSelf)
                x.refData.SetData(data);
        });

        SetContentUnlocked(isUnlocked);
        SetBasicInfo(data);
    }

    private void SetContentUnlocked(bool isUnlocked)
    {
        int id = int.Parse(_data.id);
        // imgIconInCard.sprite = ControllerSprite.Instance.GetIdleEarnIcon(id);
        imgIconInCard.LoadSpriteAutoParseAsync("idle_earn_" + id);
        var matDisable = DBM.Config.visualConfig.materialConfig.matDisableObject;

        imgCard.material = isUnlocked ? null : matDisable;
        holderLevel.material = isUnlocked ? null : matDisable;
        imgIconInCard.material = isUnlocked ? null : matDisable;
    }

    private void SetBasicInfo(DataIdleEarnUpgradeItem data)
    {
        string idleName = ExtensionEnum.ToIdleEarnName(data.id);
        txtTitle.text = idleName;
        txtLevel.text = $"Lv.{data.level}";
    }

    private void OnDoneUnlockCard()
    {
        if (SpecialExtensionTutorial.IsInTutorial(TutorialCategory.Upgrade, TutorialState.UpgradeActionFirstSkill))
        {
            GetComponent<UIPopup>().Hide();
        }
    }
}
