using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    //Data 틀에서 생산되는 Data 기본(부모) 클래스
    //공통 속성 : 이름 목록(List)
    //공통 기능 : 데이터 갯수 가져오기, 이름 리스트 가져오기, 데이터 추가, 복사, 제거
    public class BaseData : ScriptableObject
    {
        #region Variables
        
        public List<string> names;      //이름 목록(List)
        public const string dataPath = "/My3DGame/Resources/Data";      //데이터 파일 경로
        
        #endregion
        
        //생성자
        public BaseData() { }
        
        //데이터 갯수 가져오기
        public int GetDataCount()
        {
            //names null check
            if (this.names == null) return 0;

            return names.Count;
        }
        
        //이름 리스트 가져오기 - 
        public string[] GetNameList(bool showId, string filterWord = "")
        {
            int length = GetDataCount();
            string[] returnNames = new string[length];

            for (int i = 0; i < length; i++)
            {
                //필터링
                if (filterWord != "")
                {
                    if (names[i].ToLower().Contains(filterWord.ToLower()) == false)
                    {
                        continue;
                    }
                }

                if (showId)
                {
                    returnNames[i] = i.ToString() + " : " + names[i];
                }
                else
                {
                    returnNames[i] = names[i];
                }
            }
            
            return returnNames;
        }
        
        //데이터 추가하기 - 추가 후 데이터 목록 갯수 반환
        public virtual int AddData(string newName)
        {
            return GetDataCount();
        }
        
        //데이터 복사하기
        public virtual void CopyData(int index)
        {
            if (index < 0 || index >= GetDataCount()) return;
            
            string copyName = names[index] + "_Copy";
            names.Add(copyName);
        }
        
        //데이터 제거하기
        public virtual void RemoveData(int index)
        {
            if (index < 0 || index >= GetDataCount()) return;
            
            names.RemoveAt(index);
        }
    }
}
