using System;

namespace MySample
{
    /// <summary>
    /// 대화 속성을 관리하는 직렬화된 클래스
    /// </summary>
    [Serializable]
    public class Dialog
    {
        public int number;              //대화 인덱스
        public int character;           //대화 캐릭터 인덱스 (0:캐릭터 이미지 없음)
        public string name;             //대화 캐릭터 이름
        public string sentence;         //대화 내용
        public int next;                //다음 대화 인덱스 (-1:다음 대화가 없다)
    }
}