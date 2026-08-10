using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MySample
{   
    //대화창 그리기를 관리하는 클래스
    //매개변수로 들어온 Dialog 데이터를 UI에 적용하기
    public class DrawDialog : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI sentenceText;
        public GameObject npcImage;
        public GameObject nextButton;
        
        private float typingSpeed = 0.05f;
        private Coroutine typingCoroutine;

        /// <summary>
        /// Dialog 데이터를 UI에 적용
        /// </summary>
        public void DrawDialogData(Dialog dialog)
        {
            if (dialog == null)
            {
                return;
            }

            // 캐릭터 이름 표시
            nameText.text = dialog.name;

            // 캐릭터 이미지 처리 (0: 캐릭터 이미지 없음)
            if (dialog.character == 0)
            {
                npcImage.SetActive(false);
            }
            else
            {
                npcImage.SetActive(true);
                SetNpcImage(dialog.character);
            }

            // 다음 대화 여부에 따라 nextButton 활성화/비활성화
            if (dialog.next == -1)
            {
                nextButton.SetActive(false);
            }

            // 타이핑 연출로 문장 표시
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypingEffect(dialog.sentence));
        }

        /// <summary>
        /// 리소스 폴더의 Npc 폴더에서 캐릭터 인덱스에 맞는 이미지 로드 및 설정
        /// npc01.png, npc02.png, ... 등의 파일명과 character 인덱스를 매칭
        /// </summary>
        private void SetNpcImage(int characterIndex)
        {
            string imagePath = $"Npc/npc{characterIndex:D2}";
            Sprite npcSprite = Resources.Load<Sprite>(imagePath);

            if (npcSprite != null)
            {
                Image imageComponent = npcImage.GetComponent<Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = npcSprite;
                }
            }
            else
            {
                Debug.LogWarning($"NPC 이미지를 찾을 수 없습니다: {imagePath}");
            }
        }

        /// <summary>
        /// 타이핑 연출 구현
        /// 타이핑 연출하는 동안 nextButton 비활성화
        /// 타이핑 완료 후 nextButton 활성화
        /// </summary>
        private IEnumerator TypingEffect(string text)
        {
            sentenceText.text = "";
            nextButton.SetActive(false);

            foreach (char character in text)
            {
                sentenceText.text += character;
                yield return new WaitForSeconds(typingSpeed);
            }

            nextButton.SetActive(true);
        }

        /// <summary>
        /// 타이핑 연출 스킵 (전체 텍스트 즉시 표시)
        /// </summary>
        public void SkipTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            nextButton.SetActive(true);
        }
    }
}