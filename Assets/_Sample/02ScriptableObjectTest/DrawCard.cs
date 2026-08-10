using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MySample
{
    //CardSO 데이터를 읽어와서 카드 그리기
    public class DrawCard : MonoBehaviour
    {
        [SerializeField] private CardSO cardData;
        
        // UI Components for drawing card
        [SerializeField] private Image cardBackground;
        [SerializeField] private Image artImage;
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI atkText;
        [SerializeField] private TextMeshProUGUI healthText;

        private void Start()
        {
            // Resources 폴더에서 CardSO 데이터 로드
            if (cardData == null)
            {
                cardData = Resources.Load<CardSO>("Cards/Edwin");
            }
            
            if (cardData != null)
            {
                DrawCardUI();
            }
            else
            {
                Debug.LogError("CardSO 데이터를 찾을 수 없습니다.");
            }
        }

        /// <summary>
        /// CardSO 데이터를 바탕으로 카드 UI 그리기
        /// </summary>
        public void DrawCardUI()
        {
            if (cardData == null)
            {
                Debug.LogError("카드 데이터가 설정되지 않았습니다.");
                return;
            }

            // 카드 이름 설정
            if (cardName != null)
            {
                cardName.text = cardData.name;
            }

            // 카드 설명 설정
            if (description != null)
            {
                description.text = cardData.description;
            }

            // 비용 설정
            if (costText != null)
            {
                costText.text = cardData.cost.ToString();
            }

            // 공격력 설정
            if (atkText != null)
            {
                atkText.text = cardData.atk.ToString();
            }

            // 체력 설정
            if (healthText != null)
            {
                healthText.text = cardData.health.ToString();
            }

            // 카드 아트 이미지 설정
            if (artImage != null && cardData.artImage != null)
            {
                artImage.sprite = cardData.artImage;
            }
        }

        /// <summary>
        /// 다른 카드 데이터로 변경해서 그리기
        /// </summary>
        public void SetAndDrawCard(CardSO newCard)
        {
            cardData = newCard;
            DrawCardUI();
        }

        /// <summary>
        /// Resources 폴더에서 이름으로 카드 데이터 로드하고 그리기
        /// </summary>
        public void LoadAndDrawCard(string cardName)
        {
            cardData = Resources.Load<CardSO>($"Cards/{cardName}");
            if (cardData != null)
            {
                DrawCardUI();
            }
            else
            {
                Debug.LogError($"카드 '{cardName}'을 찾을 수 없습니다.");
            }
        }
    }
}

