using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터 리스트를 관리하는 ScriptableObject
    /// 속성 : 이펙트 데이터 리스트
    /// 기능 : 데이터 저장하기, 불러오기 
    /// </summary>
    public class EffectData : BaseData
    {
        #region Variables
        //툴에서 사용하는 이펙트 데이터 리스트
        public List<EffectClip> clips;      

        //파일 (xml, json)
        //리소스 폴더 이하 경로 - Resources.Load 경로
        public const string dataPath = "Data/EffectData";
        public const string fileName = "EffectData.json";
        #endregion

        //생성자
        public EffectData() { }

        //데이터(이펙트 데이터 리스트) 저장하기 
        public void SaveData()
        {
            //json
            //툴 이름 목록 리스트를 읽어서 클립리스트에 있는 이름에 저장
            int length = GetDataCount();
            for (int i = 0; i < length; i++)
            {
                clips[i].id = i;
                clips[i].name = names[i];
            }

            //파일에 저장할 데이터 준비
            EffectDatabase database = new EffectDatabase();
            database.clips = clips;
            //저장할 데이터를 json 타입의 텍스트로 변경
            string jsonOutput = JsonUtility.ToJson(database, true);
            
            //파일 저장
            string filePath = Application.dataPath + dataPath_Asset + fileName;
            File.WriteAllText(filePath, jsonOutput);
        }

        //데이터(이펙트 데이터 리스트) 불러오기
        public void LoadData()
        {
            TextAsset asset = ResourcesManager.Load<TextAsset>(dataPath);
            if(asset == null || asset.text == null)
            {
                //새로운 빈데이터를 하나 추가
                AddData("New Effect");
                return;
            }

            //json
            EffectDatabase database = JsonUtility.FromJson<EffectDatabase>(asset.text);
            clips = database.clips;

            //클립리스트에 있는 이름을 읽어서 툴 이름 목록 리스트에 저장
            int length = clips.Count;
            names = new List<string>();
            for (int i = 0; i < length; i++)
            {
                names.Add(clips[i].name);
            }
        }

        //데이터 추가하기 - 추가 후 데이터 목록 갯수 반환
        public override int AddData(string newName)
        {
            //데이터가 하나도 없을때
            if(names == null)
            {
                //리스트 새로 생성하고 데이터 추가
                names = new List<string>() { newName };
                clips = new List<EffectClip>() { new EffectClip() };
            }
            else
            {
                names.Add(newName); //이름 목록에 새로운 이름 추가
                clips.Add(new EffectClip());    //이펙트 데이터 리스트 추가
            }

            return GetDataCount();
        }

        //데이터 복사하기
        public override void CopyData(int index)
        {
            names.Add(names[index]);    //이름 목록에 선택한 목록 이름 추가
            clips.Add(CopyClip(index)); //선택한 이펙트 데이터를 리스트 추가
        }

        //데이터 제거하기
        public override void RemoveData(int index)
        {
            //이름 목록에 선택한 목록 이름 제거
            names.Remove(names[index]); 
            if (names.Count == 0)
                names = null;

            //선택한 이펙트 데이터를 리스트 제거
            clips.Remove(clips[index]);
            if(clips.Count == 0)
            {
                clips = null;
            }
        }

        //매개변수로 들어온 데이터를 복사해서 반환하기
        public EffectClip CopyClip(int index)
        {
            //인덱스 체크
            if(index < 0 || index >= clips.Count)
                return null;

            EffectClip originClip = clips[index];

            EffectClip newClip = new EffectClip();            
            newClip.effectType = originClip.effectType;
            newClip.effectPath = originClip.effectPath;
            newClip.effectName = originClip.effectName;
            return newClip;
        }

        //매개변수로 들어온 데이터를 반환하기
        public EffectClip GetClip(int index)
        {
            //인덱스 체크
            if (index < 0 || index >= clips.Count)
                return null;

            //프리팹 로드
            clips[index].PreLoad();

            return clips[index];
        }

        //모드 데이터 해제
        public void ClearData()
        {
            foreach(EffectClip clip in clips)
            {
                //클립의 프리팹 해제
                clip.Release();
            }
            clips = null;
            names = null;
        }
    }
}