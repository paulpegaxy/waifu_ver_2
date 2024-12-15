// Author: 
// Created Date: 22/07/2024
// Update Time: 22/07

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Extensions;
using Game.Model;
using Game.Runtime;
using Game.UI;
using Template.Defines;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControllerClick : AItemCheckStartGame, IPointerClickHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ParticleSystem effActivePoint;
    [SerializeField] private ParticleSystem[] arrEffContainScPoint;
    [SerializeField] private GenericDictionary<TypeGirlReact, List<Transform>> dictRandomTap;

    public static Action<TypeGirlReact> OnClick;
    public static Action OnLogTap;

    private Vector3 _startPosition;
    private ModelApiGameInfo _data;

    private GraphicRaycaster _raycaster;
    private GraphicRaycaster Raycaster => _raycaster ??= GetComponent<GraphicRaycaster>();

    private bool _isDragging;
    private Vector2 _startDragPosition;


    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent(TypeGameEvent.BotSendAutoTap, ProcessBotAutoTap);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.RemoveEvent(TypeGameEvent.BotSendAutoTap, ProcessBotAutoTap);
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        ModelApiGameInfo.OnChanged += OnGameInfoChanged;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        ModelApiGameInfo.OnChanged -= OnGameInfoChanged;
    }

    protected override void OnInit()
    {
        effActivePoint.gameObject.SetActive(true);
        ServiceLocator.GetService<IServiceGirlVfx>().Init(effActivePoint, arrEffContainScPoint);
        _data = FactoryApi.Get<ApiGame>().Data.Info;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ControllerPopup.IsAnyPopupVisible())
            return;

        if (AnR.GetKey(AnR.CommonKey.VfxFloatingItem) == null)
            return;

        if (!IsGameStart)
            return;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        Raycaster.Raycast(eventData, results);

        if (results.Count <= 0) return;

        for (int i = 0; i < results.Count; i++)
        {
            var resultTag = results[i].gameObject.tag;
            var findEle = Enum.GetValues(typeof(TypeGirlReact)).Cast<TypeGirlReact>().FirstOrDefault(x => x.ToString().Equals(resultTag));
            if (findEle != TypeGirlReact.None)
            {
                if (!ServiceLocator.GetService<IServiceValidate>().ValidateClick())
                    return;


                _startPosition = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) /
                                 canvas.scaleFactor;



                ControllerAudio.Instance.PlaySfx(AnR.AudioKey.TapGirl);

                PlayVisualClick(findEle, _startPosition);
                OnLogTap?.Invoke();
                OnClick?.Invoke(findEle);
            }
        }
    }

    private void PlayVisualClick(TypeGirlReact type, Vector3 position)
    {
        ControllerResource.Add(TypeResource.HeartPoint, _data.GetFinalPointPerTap());
        ControllerResource.Subtract(TypeResource.ExpWaifu, _data.PointPerTapParse);
        var serviceGirlVfx = ServiceLocator.GetService<IServiceGirlVfx>();
        serviceGirlVfx.ActiveTapGeneral(type, position, transform);
    }

    private int _tapOfBot;
    private async void ProcessBotAutoTap(object eventData)
    {
        var isMainNode = SpecialExtensionUI.GetCurrentNode() == UIId.UIViewName.Main.ToString();
        if (!isMainNode)
            return;

        int tapCount = (int)eventData;
        if (_tapOfBot > 0)
        {
#if !PRODUCTION_BUILD
            ControllerPopup.ShowToastError("Bot dang tap rồi");
#endif
            return;
        }


        _tapOfBot = tapCount;

        // Debug.LogError("vao day ProcessBotAutoTap: "+tapCount);
        // var additiveValue = tapCount * FactoryApi.Get<ApiGame>().Data.Info.PointPerTapParse;
        int i = 0;
        float timeDelay = 1f / tapCount;
        do
        {
            _tapOfBot--;
            var randomType = (TypeGirlReact)Random.Range(1, Enum.GetValues(typeof(TypeGirlReact)).Length);
            var dict = dictRandomTap[randomType];
            var randomPos = dict[Random.Range(0, dict.Count)].localPosition;
            PlayVisualClick(randomType, randomPos);
            i++;
            await UniTask.Delay((int)timeDelay * 1000);
        } while (i < tapCount);

        _tapOfBot = 0;
    }

    private void OnGameInfoChanged(ModelApiGameInfo gameInfo)
    {
        _data = gameInfo;
    }
}