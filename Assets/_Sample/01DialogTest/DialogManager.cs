using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace MySample
{
    //대화를 관리하는 클래스
    //리소스 폴더의 Dialog.xml 파일 읽어 "List<Dialog>"
    //대화 인덱스로 현재 대화 구성하기 - "Queue<Dialog>"
    //현재 대화에서 대화를 하나씩 꺼내서 보여준다
    
    [XmlRoot("ArrayOfDialog")]
    public class DialogList
    {
        [XmlElement("Dialog")]
        public List<Dialog> dialogs = new List<Dialog>();
    }

    public class DialogManager : MonoBehaviour
    {
        private List<Dialog> allDialogs = new List<Dialog>();
        private Queue<Dialog> currentDialogQueue = new Queue<Dialog>();
        private Dialog currentDialog;
        private DrawDialog drawDialog;

        private void Start()
        {
            LoadDialogs();
            drawDialog = GetComponent<DrawDialog>();
        }

        /// <summary>
        /// 리소스 폴더의 Dialog.xml 파일을 읽어와서 대화 리스트 로드
        /// </summary>
        private void LoadDialogs()
        {
            TextAsset xmlFile = Resources.Load<TextAsset>("Dialog/Dialog");
            if (xmlFile == null)
            {
                Debug.LogError("Dialog.xml 파일을 찾을 수 없습니다!");
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DialogList));
                using (StringReader reader = new StringReader(xmlFile.text))
                {
                    DialogList dialogList = (DialogList)serializer.Deserialize(reader);
                    allDialogs = dialogList.dialogs;
                    Debug.Log($"총 {allDialogs.Count}개의 대화를 로드했습니다.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Dialog.xml 파일 파싱 중 오류가 발생했습니다: {e.Message}");
            }
        }

        /// <summary>
        /// 대화 인덱스로 대화 시작
        /// </summary>
        public void StartDialog(int dialogIndex)
        {
            currentDialogQueue.Clear();
            
            // 입력된 인덱스에서 시작하는 대화를 찾기
            AddDialogToQueue(dialogIndex);

            if (currentDialogQueue.Count > 0)
            {
                ShowNextDialog();
            }
            else
            {
                Debug.LogWarning($"인덱스 {dialogIndex}에서 시작하는 대화를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// 대화를 큐에 추가 (재귀적으로 다음 대화까지 연결)
        /// </summary>
        private void AddDialogToQueue(int dialogIndex)
        {
            Dialog dialog = allDialogs.Find(d => d.number == dialogIndex);
            if (dialog == null)
            {
                return;
            }

            currentDialogQueue.Enqueue(dialog);

            // next가 -1이 아니면 다음 대화도 큐에 추가
            if (dialog.next != -1)
            {
                AddDialogToQueue(dialog.next);
            }
        }

        /// <summary>
        /// 다음 대화를 보여줌
        /// </summary>
        public void ShowNextDialog()
        {
            if (currentDialogQueue.Count > 0)
            {
                currentDialog = currentDialogQueue.Dequeue();
                DisplayDialog(currentDialog);
            }
            else
            {
                Debug.Log("더 이상의 대화가 없습니다.");
                currentDialog = null;
            }
        }

        /// <summary>
        /// 현재 대화를 화면에 표시 (UI 연동)
        /// </summary>
        private void DisplayDialog(Dialog dialog)
        {
            Debug.Log($"[{dialog.name}({dialog.character})]: {dialog.sentence}");
            
            // DrawDialog를 통해 UI에 표시
            if (drawDialog != null)
            {
                drawDialog.DrawDialogData(dialog);
            }
        }

        /// <summary>
        /// 버튼 클릭 핸들러 - 지정된 인덱스의 대화 시작
        /// Inspector에서 버튼의 onClick 이벤트에 연결
        /// </summary>
        public void OnDialogButtonClicked(int dialogIndex)
        {
            StartDialog(dialogIndex);
        }

        /// <summary>
        /// 다음 버튼 클릭 시 호출되는 함수
        /// 다음 대화를 표시
        /// </summary>
        public void OnNextButtonClicked()
        {
            if (currentDialogQueue.Count > 0)
            {
                ShowNextDialog();
            }
            else
            {
                Debug.Log("더 이상의 대화가 없습니다.");
            }
        }

        /// <summary>
        /// 현재 대화 반환
        /// </summary>
        public Dialog GetCurrentDialog()
        {
            return currentDialog;
        }

        /// <summary>
        /// 남은 대화 개수 반환
        /// </summary>
        public int GetRemainingDialogCount()
        {
            return currentDialogQueue.Count;
        }

        /// <summary>
        /// 모든 대화 반환
        /// </summary>
        public List<Dialog> GetAllDialogs()
        {
            return new List<Dialog>(allDialogs);
        }
    }

}
