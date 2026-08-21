using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace MySample
{
    /// <summary>
    /// 대화창 그리기를 관리하는 클래스
    /// 매개 변수로 들어온 Dialog 데이터를 UI에 적용합니다.
    /// - character == 0 이면 npcImage 비활성
    /// - next == -1 이면 nextButton 비활성
    /// - sentenceText는 타이핑 연출 (연출 중에는 nextButton 비활성)
    /// - npc 이미지는 Resources/Npc 폴더에서 Dialog.character와 파일이름을 매칭하여 로드합니다.
    /// </summary>
    public class DrawDialog : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI sentenceText;
        // npcImage 는 캐릭터 이미지를 표시하는 GameObject (Image 컴포넌트 포함 권장)
        public GameObject npcImage;
        // next 버튼 GameObject
        public GameObject nextButton;

        [Tooltip("타이핑 문자 간 간격(초)")]
        public float typingSpeed = 0.03f;

        [Tooltip("Resources 폴더 내 캐릭터 이미지가 들어있는 폴더 이름(예: NpcImage)")]
        public string npcImageFolder = "Npc";

        Image _npcImageComponent;
        Coroutine _typingCoroutine;
        #endregion

        void Awake()
        {
            if (npcImage != null)
                _npcImageComponent = npcImage.GetComponent<Image>();
        }

        /// <summary>
        /// 전달된 Dialog 데이터를 UI에 적용하고 타이핑 연출을 시작합니다.
        /// 이미지 로드는 Resources/{npcImageFolder}/{파일이름} 형태로 시도합니다.
        /// 시도 순서: "{character}", "npc{character}", "character{character}"
        /// </summary>
        public void Draw(Dialog dlg)
        {
            if (dlg == null)
                return;

            // 이름 표시
            if (nameText != null)
                nameText.text = dlg.name ?? string.Empty;

            // 캐릭터 이미지 처리
            if (dlg.character <= 0)
            {
                if (npcImage != null)
                    npcImage.SetActive(false);
            }
            else
            {
                if (npcImage != null)
                    npcImage.SetActive(true);

                if (_npcImageComponent != null)
                {
                    Sprite s = LoadNpcSpriteForCharacter(dlg.character);
                    if (s != null)
                        _npcImageComponent.sprite = s;
                    else
                        Debug.LogWarning($"DrawDialog: Resources/{npcImageFolder}에서 캐릭터 이미지({dlg.character})를 찾을 수 없습니다.");
                }
            }

            // 버튼 초기 비활성 (타이핑 중일 수 있으므로)
            if (nextButton != null)
                nextButton.SetActive(false);

            // 타이핑 시작
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeSentence(dlg.sentence ?? string.Empty, dlg.next != -1));
        }

        Sprite LoadNpcSpriteForCharacter(int characterIndex)
        {
            if (string.IsNullOrEmpty(npcImageFolder))
                npcImageFolder = "Npc";

            string[] candidates = new string[] {
                characterIndex.ToString(),
                $"npc{characterIndex}",
                $"character{characterIndex}"
            };

            foreach (var name in candidates)
            {
                var path = $"{npcImageFolder}/{name}";
                var sprite = Resources.Load<Sprite>(path);
                if (sprite != null)
                    return sprite;
            }

            return null;
        }

        IEnumerator TypeSentence(string sentence, bool hasNext)
        {
            if (sentenceText == null)
                yield break;

            sentenceText.text = string.Empty;
            // 타이핑 연출 중에는 nextButton 비활성
            if (nextButton != null)
                nextButton.SetActive(false);

            for (int i = 0; i < sentence.Length; i++)
            {
                sentenceText.text += sentence[i];
                yield return new WaitForSeconds(typingSpeed);
            }

            // 연출 끝나면 다음 버튼 활성화 (다음 대화가 있을 때만)
            if (nextButton != null)
                nextButton.SetActive(hasNext);

            _typingCoroutine = null;
        }

        /// <summary>
        /// 타이핑 중이면 즉시 전체 문장을 표시합니다.
        /// (예: 사용자가 빠르게 넘기기 원할 때 호출)
        /// </summary>
        public void FinishTypingNow(string fullSentence, bool hasNext)
        {
            if (sentenceText == null)
                return;

            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            sentenceText.text = fullSentence ?? string.Empty;
            if (nextButton != null)
                nextButton.SetActive(hasNext);
        }
    }
}