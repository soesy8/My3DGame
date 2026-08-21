using UnityEngine;

namespace MySample
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Sample/Card")]
    public class CardSO : ScriptableObject
    {
        new public string name; //카드 이름
        public string description; //카드 내용

        public int mana;
        public int attck;
        public int health;

        public Sprite artImage;     //카드 텍스쳐    
    }
}