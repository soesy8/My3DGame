using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using UnityObject = UnityEngine.Object;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 툴 고유의 기능 구현
    /// Editor창 만들기
    /// </summary>
    public class EffectTool : EditorWindow
    {
        #region Variables
        //이펙트 데이터
        private static EffectData effectData;
        //이펙트 클립 오브젝트
        private GameObject effectSource = null;

        //툴 UI
        public int uiWidthLarge = 300;
        public int uiWidthMiddle = 200;

        private int selection = 0;              //리스트 선택 인덱스
        private Vector2 sp1 = Vector2.zero;
        private Vector2 sp2 = Vector2.zero;
        #endregion

        [MenuItem("Tools/Effect Tool")] //유니티 에디터 메뉴 추가
        static void Init()
        {
            //이펙트 데이터 객체 생성 및 데이터 로드
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();

            //툴 윈도우창 열기
            EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");
            window.Show();
        }

        //window UI 그리기
        private void OnGUI()
        {
            //데이터 체크
            if (effectData == null)
                return;

            EditorGUILayout.BeginVertical();
            {
                UnityObject source = effectSource;

                //툴 상단 레이어 그리기
                EditorHelper.EditorToolTopLayer(effectData, ref selection,
                    ref source, uiWidthMiddle);
                effectSource = (GameObject)source;

                //데이터 레이어 그리기
                EditorGUILayout.BeginHorizontal();
                {
                    //데이터 이름 목록 그리기
                    EditorHelper.EditorToolListLayer(effectData, ref selection,
                        ref source, uiWidthLarge, ref sp1);
                    effectSource = (GameObject)source;

                    //선택된 데이터 클립 속성 그리기
                    EditorGUILayout.BeginVertical();
                    {
                        sp2 = EditorGUILayout.BeginScrollView(sp2);
                        {
                            //데이터 체크
                            if(effectData.GetDataCount() > 0)
                            {
                                //속성 나열
                                EditorGUILayout.Separator();    //빈줄 넣기
                                //id
                                EditorGUILayout.LabelField("id", selection.ToString(),
                                    GUILayout.Width(uiWidthLarge));
                                //데이터 이름
                                effectData.names[selection] = EditorGUILayout.TextField("이름", effectData.names[selection],
                                    GUILayout.Width(uiWidthLarge * 1.5f));
                                //이펙트 종류
                                effectData.clips[selection].effectType = (EffectType)EditorGUILayout.EnumPopup("이펙트 종류", 
                                    effectData.clips[selection].effectType, GUILayout.Width(uiWidthLarge));

                                EditorGUILayout.Separator();    //빈줄 넣기
                                if(effectSource == null && effectData.clips[selection].effectName != "")
                                {
                                    effectData.clips[selection].PreLoad();
                                    effectSource = Resources.Load<GameObject>(effectData.clips[selection].effectPath
                                        + effectData.clips[selection].effectName);
                                }
                                effectSource = (GameObject)EditorGUILayout.ObjectField("이펙트 오브젝트", effectSource,
                                    typeof(GameObject), false, GUILayout.Width(uiWidthLarge * 1.5f));
                                if(effectSource != null)
                                {
                                    effectData.clips[selection].effectPath = EditorHelper.GetPath(source);
                                    effectData.clips[selection].effectName = effectSource.name;
                                }
                                else //null 이면
                                {
                                    effectData.clips[selection].effectPath = "";
                                    effectData.clips[selection].effectName = "";
                                }
                                EditorGUILayout.Separator();    //빈줄 넣기
                            }
                        }
                        EditorGUILayout.EndScrollView();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();    //빈줄 넣기
            //하단 레이어
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Reload Settings"))
                {
                    effectData = ScriptableObject.CreateInstance<EffectData>();
                    effectData.LoadData();
                    selection = 0;
                    effectSource = null;
                }
                if (GUILayout.Button("Save"))
                {
                    effectData.SaveData();
                    //이름 목록을 enum 만들기
                    CreateEnumFile();

                    //새로운 내용 에디터 프로젝트에 적요
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);  
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //이름 목록을 enum 만들기
        public void CreateEnumFile()
        {
            string enumName = "EffectList";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();
            for (int i = 0; i < effectData.names.Count; i++)
            {
                if (effectData.names[i] != "")
                {
                    builder.AppendLine("            " + effectData.names[i] + "=" + i + ",");
                }
            }
            EditorHelper.CreateEnumStructure(enumName, builder);
        }

    }
}