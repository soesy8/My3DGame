using UnityEngine;

namespace My3DGame
{
    //게임 사용하는 Enum 정의

    /// <summary>
    /// 이펙트 종류 정의
    /// </summary>
    public enum EffectType
    {
        None = -1,
        NORMAL,
    }

    /// <summary>
    /// 사운드 종류 정의
    /// </summary>
    public enum SoundType
    {
        None = -1,
        MUSIC,          //배경음
        SFX,            //효과음
        VOICE,          //음성
    }


    /*/// <summary>
    /// 툴에 있는 이펙트 리스트 enum 정의
    /// </summary>
    public enum EffectList
    {
        None = -1,

    }*/
}
