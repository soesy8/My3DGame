using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 게임에서 사용하는 데이터들을 관리하는 클래스
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        #region Variables
        private static EffectData effectData = null;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //데이터 사전 로딩
            GetEffectData();

        }
        #endregion

        #region Custom Method
        public static EffectData GetEffectData()
        {
            if(effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }

            return effectData;
        }
        #endregion
    }
}