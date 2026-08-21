using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MySample
{
    /// <summary>
    /// 대화를 관리하는 클래스
    /// 리소스 폴더에 있는 Dialog.xml 파일을 읽어 List<Dialog> 로 보관합니다.
    /// SetDialog(index) 로 현재 대화(Queue<Dialog>) 를 구성하고
    /// GetNext / TryGetNext 로 한 문장씩 꺼내어 사용할 수 있습니다.
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        // 전체 대화 목록 (Dialog.xml 에서 읽음)
        public List<Dialog> Dialogs { get; private set; }

        // 현재 재생 중인 대화 큐
        public Queue<Dialog> CurrentQueue { get; private set; }

        // DrawDialog 연결 (Inspector에 드래그)
        public DrawDialog drawDialog;

        void Awake()
        {
            LoadDialogs();
        }

        /// <summary>
        /// Resources/Dialog/Dialog.xml 을 읽어 Dialogs 를 초기화합니다.
        /// </summary>
        public void LoadDialogs()
        {
            try
            {
                var ta = Resources.Load<TextAsset>("Dialog/Dialog");
                if (ta == null)
                {
                    Debug.LogError("DialogManager: Resources/Dialog/Dialog.xml을 찾을 수 없습니다.");
                    Dialogs = new List<Dialog>();
                    return;
                }

                var serializer = new XmlSerializer(typeof(List<Dialog>));
                using (var reader = new StringReader(ta.text))
                {
                    Dialogs = (List<Dialog>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"DialogManager: 대화 로드 중 오류 발생 - {ex.Message}");
                Dialogs = new List<Dialog>();
            }
        }

        /// <summary>
        /// 지정한 대화 인덱스를 현재 대화로 설정합니다.
        /// </summary>
        /// <param name="index">대화 번호</param>
        public void SetDialog(int index)
        {
            if (Dialogs == null)
                LoadDialogs();

            var list = Dialogs.Where(d => d.number == index).ToList();
            if (list.Count == 0)
            {
                Debug.LogWarning($"DialogManager: 인덱스 {index} 에 해당하는 대화가 없습니다.");
            }

            CurrentQueue = new Queue<Dialog>(list);
        }

        /// <summary>
        /// 다음 대화를 꺼냅니다. 없으면 null 반환.
        /// </summary>
        public Dialog GetNext()
        {
            if (CurrentQueue == null || CurrentQueue.Count == 0)
                return null;
            return CurrentQueue.Dequeue();
        }

        /// <summary>
        /// 다음 대화를 시도해서 꺼냅니다.
        /// </summary>
        public bool TryGetNext(out Dialog dialog)
        {
            if (CurrentQueue == null || CurrentQueue.Count == 0)
            {
                dialog = null;
                return false;
            }

            dialog = CurrentQueue.Dequeue();
            return true;
        }

        /// <summary>
        /// 현재 대화가 남아있는지 여부
        /// </summary>
        public bool HasNext => CurrentQueue != null && CurrentQueue.Count > 0;

        /// <summary>
        /// 0번 버튼이 눌렸을 때 호출할 함수.
        /// 인덱스 0의 대화를 로드하고 첫 문장을 DrawDialog에 전달해 표시합니다.
        /// </summary>
        public void OnPressButton0(int dialogNumber)
        {
            SetDialog(dialogNumber);

            if (TryGetNext(out Dialog dlg))
            {
                if (drawDialog != null)
                    drawDialog.Draw(dlg);
                else
                    Debug.LogWarning("DialogManager: drawDialog가 연결되어 있지 않습니다.");
            }
            else
            {
                Debug.LogWarning("DialogManager: 인덱스 0에 대한 대화를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 다음 문장을 표시합니다. UI의 Next 버튼에 연결해서 사용하세요.
        /// </summary>
        public void OnPressNext()
        {
            if (TryGetNext(out Dialog dlg))
            {
                if (drawDialog != null)
                    drawDialog.Draw(dlg);
            }
            else
            {
                Debug.Log("DialogManager: 더 이상 대화가 없습니다.");
            }
        }
    }
}

/*
DialogManager를 아래 내용에 따라 구현해줘
0번 버튼을 누르면 0번 인덱스의 대화를 보여주는 함수 구현해줘
*/