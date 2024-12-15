using System.Collections;
using System.Collections.Generic;
using Game.Defines;
using Template.Defines;
using UnityEngine;
using UnityEngine.U2D;

public class ControllerSprite : MonoBehaviour
{
    [SerializeField] private List<SpriteAtlas> AtlasSprites;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitAfterSceneLoad()
    {
        Instance.Init();
    }

    private static ControllerSprite _instance;

    public static ControllerSprite Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = Instantiate(Resources.Load("Managers/ControllerSprite") as GameObject);
            _instance = obj.GetComponent<ControllerSprite>();
            DontDestroyOnLoad(obj);
            return _instance;
        }
    }


    public enum AtlasType
    {
        Icon,
        ResourceIcon,
    }

    private void Init()
    {

    }

    private Sprite Get(string name, AtlasType type)
    {
        return AtlasSprites[(int)type].GetSprite(name);
    }

    public Sprite GetResourceIcon(TypeResource type)
    {
        return Get(((int)type).ToString(), AtlasType.ResourceIcon);
    }
    
    public Sprite GetResourceIcon(int id)
    {
        return Get(id.ToString(), AtlasType.ResourceIcon);
    }

    public Sprite GetZodiacIcon(TypeZodiac type)
    {
        return Get("icon_zodiac_" + ((int)type + 1), AtlasType.Icon);
    }
    
    public Sprite GetStoryGenresIcon(TypeStoryGenres type)
    {
        return Get("icon_story_genres_" + ((int)type + 1), AtlasType.Icon);
    }

    public Sprite GetHobbyIcon(int index)
    {
        return Get("icon_hobby_" + (index + 1), AtlasType.Icon);
    }

    public Sprite GetSubPack(int index)
    {
        return Get("sub_" + (index + 1), AtlasType.Icon);
    }
    
    public Sprite GetSubTag(int index)
    {
        return Get("sub_tag_" + (index + 1), AtlasType.Icon);
    }
}
