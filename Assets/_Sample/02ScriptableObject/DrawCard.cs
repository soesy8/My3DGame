using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MySample
{
    /// <summary>
    /// CardSO 데이터를 읽어 와서 카드 UI에 적용합니다.
    /// Inspector에 필요한 UI 컴포넌트를 연결하고
    /// Draw(card) 또는 DrawFromResource(path)로 갱신하세요.
    /// </summary>
    public class DrawCard : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI healthText;
        public Image artImage;

        [Header("Optional")]
        // 인스펙터에서 미리 할당해두면 시작 시 자동으로 그립니다.
        public CardSO cardToDraw;

        void Start()
        {
            if (cardToDraw != null)
                Draw(cardToDraw);
        }

        /// <summary>
        /// 전달된 CardSO 데이터를 UI에 적용합니다.
        /// </summary>
        public void Draw(CardSO card)
        {
            if (card == null)
            {
                Debug.LogWarning("DrawCard: card is null");
                Clear();
                return;
            }

            if (nameText != null)
                nameText.text = card.name ?? string.Empty;

            if (descriptionText != null)
                descriptionText.text = card.description ?? string.Empty;

            if (manaText != null)
                manaText.text = card.mana.ToString();

            if (attackText != null)
                attackText.text = card.attck.ToString();

            if (healthText != null)
                healthText.text = card.health.ToString();

            if (artImage != null)
            {
                if (card.artImage != null)
                {
                    artImage.sprite = card.artImage;
                    artImage.gameObject.SetActive(true);
                }
                else
                {
                    artImage.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Resources에서 CardSO 를 로드해 그림. 경로는 Resources 폴더 기준입니다.
        /// 예: DrawFromResource("Cards/Edwin")
        /// </summary>
        public void DrawFromResource(string resourcePath)
        {
            var so = Resources.Load<CardSO>(resourcePath);
            if (so == null)
            {
                Debug.LogWarning($"DrawCard: Resources에서 CardSO를 찾을 수 없음 - {resourcePath}");
                return;
            }

            Draw(so);
        }

        void Clear()
        {
            if (nameText != null) nameText.text = string.Empty;
            if (descriptionText != null) descriptionText.text = string.Empty;
            if (manaText != null) manaText.text = string.Empty;
            if (attackText != null) attackText.text = string.Empty;
            if (healthText != null) healthText.text = string.Empty;
            if (artImage != null) artImage.gameObject.SetActive(false);
        }
    }
}

