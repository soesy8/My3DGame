using UnityEngine;
using System.Collections.Generic;
using System;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터 리스트 정의 - json 파일 불러오기,저장하기에서 이용
    /// </summary>
    [Serializable]
    public class EffectDatabase
    {
        public List<EffectClip> clips;
    }

    /// <summary>
    /// 이펙트 데이터 정의
    /// </summary>
    [Serializable]
    public class EffectClip
    {
        #region Variable
        public int id;                  //id
        public string name;             //데이터 이름
        public EffectType effectType;   //이펙트 종류
        public string effectPath;       //이펙트 파일 경로
        public string effectName;       //이펙트 파일 이름

        private GameObject effectPrefab = null; //이펙트 프리팹
        #endregion

        //생성자
        public EffectClip() { }

        //프리팹 사전 로딩
        public void PreLoad()
        {
            //경로 체크
            if (effectPath == null || effectName == null)
                return;

            var fullPath = effectPath + effectName;
            if(effectPrefab == null && fullPath != "")
            {
                effectPrefab = ResourcesManager.Load<GameObject>(fullPath);
            }
        }

        //프리팹 해제
        public void Release()
        {
            if (effectPrefab != null)
            {
                effectPrefab = null;
            }
        }

        //이펙트 오브젝트 생성
        public GameObject InstantiateEffect(Vector3 position)
        {
            if (effectPrefab == null)
                PreLoad();

            if (effectPrefab != null)
            {
                GameObject effectGo = GameObject.Instantiate(effectPrefab, position, Quaternion.identity);
                return effectGo;
            }

            return null;
        }
    }
}