using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using UnityObject = UnityEngine.Object;

namespace My3DGame
{
    /// <summary>
    /// 데이터 툴과 관련된 공통기능 구현
    /// </summary>
    public class EditorHelper
    {
        //등록한 데이터(UnityObject)의 Resources 폴더 이하 Path(경로) 가져오기
        public static string GetPath(UnityObject p_Clip)
        {
            string returnStr = string.Empty;

            string path = string.Empty;
            path = AssetDatabase.GetAssetPath(p_Clip);  //Asset 이하 경로 반환
            //Assets\_Sample\01DialogTest\Resources\Dialog\Dialog.xml
            //Resources 폴더 이하 경로 얻어오기
            string[] path_node = path.Split('/');
            bool findResources = false;
            for (int i = 0; i < path_node.Length - 1; i++)
            {
                if(findResources == false)
                {
                    if (path_node[i] == "Resources")
                    {
                        findResources = true;
                    }
                }
                else
                {
                    returnStr += path_node[i] + "/";
                }
            }

            return returnStr;
        }

        //툴에 있는 데이터의 이름 리스트로 enum 만들기
        public static void CreateEnumStructure(string enumName, StringBuilder data)
        {
            string templateFilePath = "Assets/My3DGame/Editor/EnumTemplate.txt";

            //탬플릿 내용 읽어 와서 enum 파일 내용 구성
            string entittyTemplate = File.ReadAllText(templateFilePath);
            entittyTemplate = entittyTemplate.Replace("$ENUM$", enumName);
            entittyTemplate = entittyTemplate.Replace("$DATA$", data.ToString());

            //enum 파일 내용 완성하면 파일 저장
            string enumFilePath = "Assets/My3DGame/Scripts/GameData/";
            if(Directory.Exists(enumFilePath) == false)
            {
                Directory.CreateDirectory(enumFilePath);
            }

            string savePath = enumFilePath + enumName + ".cs";
            //기존 파일 삭제
            if(File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            File.WriteAllText(savePath, entittyTemplate);
        }

        //데이터 툴의 상단 레이어(Add, Copy, Remove 버튼) 만들고 기능 구현
        public static void EditorToolTopLayer(BaseData data, ref int selection,
            ref UnityObject source, int uiWidth)
        {
            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Add", GUILayout.Width(uiWidth)))
                {
                    //버튼 클릭시 기능 구현
                    data.AddData("New Data");
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                if (GUILayout.Button("Copy", GUILayout.Width(uiWidth)))
                {
                    //버튼 클릭시 기능 구현
                    data.CopyData(selection);
                    selection = data.GetDataCount() - 1;
                    source = null;
                }
                //데이터가 하나이면 삭제 불가
                if(data.GetDataCount() > 1)
                {
                    if (GUILayout.Button("Remove", GUILayout.Width(uiWidth)))
                    {
                        //버튼 클릭시 기능 구현
                        data.RemoveData(selection);
                        source = null;
                    }
                }
                //selection 범위체크
                if(selection > data.GetDataCount()-1)
                {
                    selection = data.GetDataCount()-1;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //데이터 이름 목록 리스트 레이어 만들기
        public static void EditorToolListLayer(BaseData data, ref int selection,
            ref UnityObject source, int uiWidth, ref Vector2 scrollPosition)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
            {
                EditorGUILayout.Separator();    //빈줄 하나 넣기
                EditorGUILayout.BeginVertical("box");
                {
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    {
                        int lastSelection = selection;
                        selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
                        if(lastSelection != selection)
                        {
                            source = null;
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}