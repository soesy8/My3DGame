using UnityEngine;
using UnityObject = UnityEngine.Object;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;

namespace My3DGame
{
    /// <summary>
    /// 리소스를 관리하는 클래스
    /// 폴더에 있는 리소스를 로드
    /// 리소스 로드한 오브젝트를 게임오브젝트로 Instantiate
    /// 1. Resources.Load, 2. Addressable
    /// </summary>
    public class ResourcesManager : MonoBehaviour
    {
        //매개변수로 받은 경로에 있는 에쎗을 UnityObject로 가져오기
        public static T Load<T> (string path) where T : UnityObject
        {
            return Resources.Load<T> (path);
            //return Addressables.LoadAssetAsync<T> (path).WaitForCompletion();
        }

        //리소스 로드한 에쎗을 UnityObject로 가져온 후 Instantiate
        public static GameObject LoadAndInstantiate(string path)
        {
            UnityObject source = Load<UnityObject>(path);

            //source 널 체크
            if (source == null) return null;

            return GameObject.Instantiate(source) as GameObject;
        }
    }
}