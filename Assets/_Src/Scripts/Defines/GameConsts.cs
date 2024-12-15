using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedScrollerDemos.CellEvents;
using UnityEngine;

public static class GameConsts
{
    public static TimeSpan TIME_OUT_REQUEST = TimeSpan.FromMilliseconds(10000);

    public const int MAX_AVATAR_USER = 12;
    public const int MAX_SWIPE_COUNT = 40;
    public const int MAX_COUNT_CHAR_CHAT = 40;
    public const int MAX_WAIFU_PICTURE = 15;
    public const int MAX_LENGTH_NEXT_PREPARE_WAIFU_BANNER = 3;

#if PRODUCTION_BUILD
    public const int GAME_INFO_REFRESH_TIME = 30;
#else
    public const int GAME_INFO_REFRESH_TIME = 10;
#endif

    public const int GAME_INFO_REFRESH_TOKEN = 24 * 60 * 60;
    public const int SERVER_TIME_REFRESH_TIME = 5 * 60;
    public const int INIT_POINT = 1990;
    public const int INIT_FIRST_UNDRESS = 20;
    public const int MAX_TICKET = 3;
    public const int MAX_PICK_STORY_GENRES = 3;

    public const int BOT_TRIAL_AUTO_TAP = 5;
    public const int BOT_PREMIUM_AUTO_TAP = 10;
    public const int BOT_TRIAL_HARD_CHECK_TIME = 5;

    public const int MAX_NUMBER_BG_EVENT_YUKI = 50000;

    public const int MAX_LEVEL_PER_CHAR = 10;
    public const int MAX_LENGTH_NICK = 12;
    public const int MAX_LENGHT_SHORT_MESSAGE = 25;

    public const int MAX_LOG_TAP_FOR_FIRST_TIME = 3;
    public const int MAX_LOG_TAP_FOR_BOOSTER_TUT = 5;
    public const int MAX_LOG_TAP_FOR_UPGRADE_TUT = 1;
    public const int MAX_LOG_TAP_FOR_GAME_FEATURE_TUT = 10;


    public const int MAX_LENGHT_PER_LINE_MESSAGE = 30;
    public const int MAX_WORD_PER_LINE = 7;
    public const int MAX_WORD_PER_LINE_DATING = 7;

    public const int MAX_LENGTH_PRE_POOL_FLOAT_ITEM_EFF = 50;
    public const int MAX_LENGTH_PRE_POOL_SELECTION_EFF = 15;

    public const float ANIM_NORMAL_DURATION_TIME = 0.25f;

    public const string DEFAULT_KEY_GGSHEET = "1HI8dYZDxQQOjpFlK8cbejAdO9O-pgPlgptknocSxshY";

    public const string PATH_AUTO_GEN_UI = "Assets/_Src/Scripts/Defines/AutoGenerate";

    public const int DELAY_REQUEST_BUY_ONCHAIN = 300;
}
