using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using System.Text.RegularExpressions;

public class TalkManager : MonoBehaviour
{
    [SerializeField] private GameObject m_TalkObjectFolder; //Talkに関するものを入れてるGameObject
    private GameObject m_LeftCharacter; //左側立ち絵
    private string m_LeftCharacterName; //左側キャラファイル名
    private bool m_LeftCharacterMoved = false;
    private GameObject m_RightCharacter; //右側立ち絵
    private string m_RightCharacterName; //右側キャラファイル名
    private bool m_RightCharacterMoved = false;
    [SerializeField] private Image m_TextBox; //テキストボックス
    [SerializeField] private TextMeshProUGUI m_TalkTextMesh; //テキスト

    [SerializeField] private TextAsset m_TalkText; //会話txtファイル

    private int m_DialogNumber; //表示行数
    private List<string> m_TalkDialog = new List<string>(); //会話 行ごと
    private bool m_AppNext = true; //進行入力できるか
    private bool m_AppNextFalseNow = false; //App.Next制御しているか

    void OnEnable()
    {
        m_AppNext = true;
        m_AppNextFalseNow = false;
        m_DialogNumber = 0;
        m_TalkObjectFolder.SetActive(true);
        m_LeftCharacter = GameObject.Find("LeftCharacter");
        m_RightCharacter = GameObject.Find("RightCharacter");
        m_LeftCharacterName = "TestPerson";
        m_RightCharacterName = "TestPersonRed";
        CharacterPositionReset();
        LeftCharacterSpriteChange("TestPerson", "Normal");
        RightCharacterSpriteChange("TestPerson", "Normal");
        m_LeftCharacter.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        m_RightCharacter.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        m_TextBox.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        m_TalkTextMesh.color = new Color32(255, 255, 255, 255);
        m_TalkTextMesh.text = "";
        m_TalkDialog.Clear();
        DialogSplit(m_TalkText);
        DialogExecute(m_TalkDialog);
    }

    void OnDisable()
    {

    }

    void Update()
    {
        //Debug.Log(m_DialogNumber);
        if (Input.GetKeyDown(KeyCode.Space) && m_AppNext)
        {
            DialogExecute(m_TalkDialog);
            StartCoroutine(CoCanNext());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }
    }
    private IEnumerator CoCanNext()
    {
        m_AppNext = false;
        for (int i = 0; i < 64; i++)
        {
            yield return null;
        }
        if (!m_AppNextFalseNow)
        {
            m_AppNext = true;
        }
    }


    private void DialogSplit(TextAsset talkText)
    {
        m_TalkDialog = talkText.text.Replace("\r\n", "\n").Split('\n').ToList();
    }
    private void DialogExecute(List<string> dialog) //一行ずつ実行
    {
        int i = 0;
        List<string> executeContents = dialog[m_DialogNumber].Replace(" ", "").Split(';').ToList();

        if(executeContents[executeContents.Count() - 1].Length == 0)
        {
            executeContents.RemoveAt(executeContents.Count() - 1);
        }
        else if(executeContents[executeContents.Count() - 1].Substring(0, 2) == "//")
        {
            executeContents.RemoveAt(executeContents.Count() - 1);
        }
        else
        {
            Debug.LogError("[OgameTalkScriptEngine]Error in (" + (m_DialogNumber + 1) + ", " + (i + 1) + ") : <b><i><color=white>"
                            + "; " + "</color></i></b>expected.\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
            return;
        }

        //Debug.Log(executeContents.Count());
        /*foreach (string s in executeContents)
        {
            Debug.Log(s);
        }*/
        if (executeContents.Count() != 0)
        {
            foreach (string executeContent in executeContents)
            {
                i++;
                string exe = executeContent.Split('(')[0];
                string subExe = "";
                string content = executeContent.Split('(')[1].Replace(")", "");
                //Debug.Log("exe = " + exe);
                //Debug.Log("content = " + content);
                if (Regex.IsMatch(exe, "[.]"))
                {
                    subExe = exe.Split('.')[1];
                    exe = exe.Split('.')[0];
                }
                switch (exe)
                {
                    case "Dialog":
                        TalkIndicate(content.Replace("_", " "));
                        break;

                    case "Active":
                        if (content == "L.Chara")
                        {
                            LeftCharacterON(true);
                        }
                        else if (content == "R.Chara")
                        {
                            RightCharacterON(true);
                        }
                        else
                        {
                            string[] canUseArguments = { "L.Chara", "R.Chara" };
                            DebugErrorContent(m_DialogNumber, i, exe, content, canUseArguments);
                        }
                        break;

                    case "Negative":
                        if (content == "L.Chara")
                        {
                            LeftCharacterOFF();
                        }
                        else if (content == "R.Chara")
                        {
                            RightCharacterOFF();
                        }
                        else
                        {
                            string[] canUseArguments = { "L.Chara", "R.Chara" };
                            DebugErrorContent(m_DialogNumber, i, exe, content, canUseArguments);
                        }
                        break;

                    case "Switch":
                        switch (subExe)
                        {
                            case "ActNeg":
                                if (content == "")
                                {

                                    if (m_LeftCharacterMoved)
                                    {
                                        LeftCharacterON(true);
                                        RightCharacterOFF();
                                    }
                                    else
                                    {
                                        LeftCharacterOFF();
                                        RightCharacterON(true);
                                    }
                                }
                                else
                                {
                                    string postScript = "You cannot use argments.";
                                    DebugErrorContent(m_DialogNumber, i, exe + subExe, content, postScript);
                                }
                                break;

                            default:
                                DebugErrorExe(m_DialogNumber, i, exe + "." + subExe, content);
                                break;
                        }

                        break;

                    case "WaitNext":
                        if (JudgeIsStringNumber(content))
                        {
                            float second = float.Parse(content);
                            StartCoroutine(CoDialogExecute(second));
                        }
                        else
                        {
                            string postScript = "You can only use numbers as arguments.";
                            DebugErrorContent(m_DialogNumber, i, exe, content, postScript);
                        }
                        break;

                    case "App":
                        switch (subExe)
                        {
                            case "Next":
                                if (content == "True")
                                {
                                    m_AppNext = true;
                                    m_AppNextFalseNow = false;
                                }
                                else if (content == "False")
                                {
                                    m_AppNext = false;
                                    m_AppNextFalseNow = true;
                                }
                                else
                                {
                                    string[] canUseArguments = { "True", "False" };
                                    DebugErrorContent(m_DialogNumber, i, exe, content, canUseArguments);
                                }
                                break;

                            default:
                                DebugErrorExe(m_DialogNumber, i, exe + "." + subExe, content);
                                break;
                        }

                        break;

                    case "Next":
                        if (content == "")
                        {
                            StartCoroutine(CoDialogExecute(0));
                        }
                        else
                        {
                            string tempStr = "You cannot use argments.";
                            DebugErrorContent(m_DialogNumber, i, exe, content, tempStr);
                        }
                        break;

                    case "Start":
                        List<bool> truefalse = new List<bool>();
                        string[] contents = content.Split(',');
                        bool isError = false;
                        int count = 0;
                        for (int j = 0; j < contents.Length; j++)
                        {
                            truefalse.Add(true);
                        }
                        foreach (string con in contents)
                        {
                            if (con == "TxtBox")
                            {
                                TextBoxStart();
                            }
                            else if (con == "L.Chara")
                            {
                                LeftCharacterStart();
                            }
                            else if (con == "R.Chara")
                            {
                                RightCharacterStart();
                            }
                            else
                            {
                                truefalse[count] = false;
                                isError = true;
                            }
                            count++;
                        }

                        if (isError)
                        {
                            string[] canUseArguments = { "TxtBox", "L.Chara", "R.Chara" };
                            DebugErrorContent(m_DialogNumber, i, exe, contents, truefalse.ToArray(), canUseArguments);
                        }
                        break;

                    case "End":
                        truefalse = new List<bool>();
                        contents = content.Split(',');
                        isError = false;
                        count = 0;
                        for (int j = 0; j < contents.Length; j++)
                        {
                            truefalse.Add(true);
                        }
                        foreach (string con in contents)
                        {
                            if (con == "TxtBox")
                            {
                                TextBoxEnd();
                            }
                            else if (con == "L.Chara")
                            {
                                LeftCharacterEnd();
                            }
                            else if (con == "R.Chara")
                            {
                                RightCharacterEnd();
                            }
                            else if (con == "TalkText")
                            {
                                TalkTextEnd();
                            }
                            else if (con == "This")
                            {
                                TalkManagerEnd();
                            }
                            else
                            {
                                truefalse[count] = false;
                                isError = true;
                            }
                            count++;
                        }

                        if (isError)
                        {
                            string[] canUseArguments = { "TxtBox", "L.Chara", "R.Chara", "TalkText", "This" };
                            DebugErrorContent(m_DialogNumber, i, exe, contents, truefalse.ToArray(), canUseArguments);
                        }
                        break;

                    case "Change":
                        string[] splitContents = content.Split(char.Parse(","));
                        truefalse = new List<bool>();
                        truefalse.Add(false); truefalse.Add(true);
                        if (splitContents[0].Replace(" ", "") == "L.Chara")
                        {
                            LeftCharacterSpriteChange(m_LeftCharacterName, splitContents[1]);
                        }
                        else if (splitContents[0].Replace(" ", "") == "R.Chara")
                        {
                            RightCharacterSpriteChange(m_RightCharacterName, splitContents[1]);
                        }
                        else
                        {
                            string[] canUseArguments = { "L.Chara", "R.Chara" };
                            DebugErrorContent(m_DialogNumber, i, exe, splitContents, truefalse.ToArray(), canUseArguments, 1);
                        }
                        break;

                    case "SetPath":
                        splitContents = content.Split(char.Parse(","));
                        truefalse = new List<bool>();
                        truefalse.Add(false); truefalse.Add(true);
                        if (splitContents[0].Replace(" ", "") == "L.Chara")
                        {
                            m_LeftCharacterName = splitContents[1].Replace(" ", "");
                        }
                        else if (splitContents[0].Replace(" ", "") == "R.Chara")
                        {
                            m_RightCharacterName = splitContents[1].Replace(" ", "");
                        }
                        else
                        {
                            string[] canUseArguments = { "L.Chara", "R.Chara" };
                            DebugErrorContent(m_DialogNumber, i, exe, splitContents, truefalse.ToArray(), canUseArguments, 1);
                        }
                        break;

                    case "Font":
                        switch (subExe)
                        {
                            case "Size":
                                if (content == "")
                                {
                                    m_TalkTextMesh.fontSize = 38f;
                                }
                                else
                                {
                                    if (JudgeIsStringNumber(content))
                                    {
                                        float size = float.Parse(content);
                                        m_TalkTextMesh.fontSize = size;
                                    }
                                    else
                                    {
                                        string postScript = "You can only use numbers as arguments.";
                                        DebugErrorContent(m_DialogNumber, i, exe, content, postScript);
                                        break;
                                    }
                                }
                                break;

                            case "Color":
                                if (content == "")
                                {
                                    m_TalkTextMesh.color = new Color32(255, 255, 255, 255);
                                }
                                else
                                {
                                    if (JudgeIsStringColorCode(content))
                                    {
                                        Color color = new Color32(0, 0, 0, 1);
                                        if (ColorUtility.TryParseHtmlString(content, out color))
                                        {
                                            m_TalkTextMesh.color = color;
                                        }
                                    }
                                    else
                                    {
                                        string postScript = "You can only use colorcode as arguments.";
                                        DebugErrorContent(m_DialogNumber, i, exe + "." + subExe, content, postScript);
                                        break;
                                    }
                                    break;
                                }
                                break;

                            case "OutlineColor":
                                if (content == "")
                                {
                                    m_TalkTextMesh.outlineColor = new Color32(0, 0, 0, 0);
                                }
                                else
                                {
                                    if (JudgeIsStringColorCode(content))
                                    {
                                        Color outlineColor = new Color32(0, 0, 0, 1);
                                        if (ColorUtility.TryParseHtmlString(content, out outlineColor))
                                        {
                                            m_TalkTextMesh.outlineColor = outlineColor;
                                        }
                                    }
                                    else
                                    {
                                        string postScript = "You can only use colorcode as arguments.";
                                        DebugErrorContent(m_DialogNumber, i, exe + "." + subExe, content, postScript);
                                        break;
                                    }
                                    break;
                                }
                                break;

                            case "OutlineThickness":
                                if (content == "")
                                {
                                    m_TalkTextMesh.SetOutline(0, 0);
                                }
                                else
                                {
                                    if (JudgeIsStringNumber(content))
                                    {
                                        float thickness = float.Parse(content);

                                        m_TalkTextMesh.SetOutline(thickness, thickness);
                                    }
                                    else
                                    {
                                        string postScript = "You can only use numbers (float, 0-1) as arguments.";
                                        DebugErrorContent(m_DialogNumber, i, exe + "." + subExe, content, postScript);
                                        break;
                                    }
                                    break;
                                }
                                break;

                            default:
                                DebugErrorExe(m_DialogNumber, i, exe + "." + subExe, content);
                                break;
                        }
                        break;

                    default:
                        if (subExe == "")
                        {
                            DebugErrorExe(m_DialogNumber, i, exe, content);
                        }
                        else
                        {
                            DebugErrorExe(m_DialogNumber, i, exe + "." + subExe, content);
                        }
                        break;
                }
            }
        }
        else
        {
            StartCoroutine(CoDialogExecute(0));
        }
        
        m_DialogNumber++;
    }

    private IEnumerator CoDialogExecute(float second)
    {
        yield return new WaitForSeconds(second);
        DialogExecute(m_TalkDialog);
        StartCoroutine(CoCanNext());
    }

    private void CharacterPositionReset()
    {
        if (m_LeftCharacterMoved)
        {
            LeftCharacterON(false);
        }
        if (m_RightCharacterMoved)
        {
            RightCharacterON(false);
        }
    }

    private void LeftCharacterStart() //左側キャラクター表示
    {
        StartCoroutine(CoLeftCharacterStart());
    }
    private IEnumerator CoLeftCharacterStart()
    {
        Image image = m_LeftCharacter.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color += new Color32(0, 0, 0, 4);
            yield return null;
        }
    }
    private void LeftCharacterON(bool colorChange) //左側キャラクターActive
    {
        StartCoroutine(CoLeftCharacterON(colorChange));
    }
    private IEnumerator CoLeftCharacterON(bool colorChange)
    {
        Image image = m_LeftCharacter.GetComponent<Image>();
        RectTransform anchor = m_LeftCharacter.GetComponent<RectTransform>();
        anchor.anchoredPosition = new Vector2(-623f, -208f);
        image.color = new Color32(127, 127, 127, 255);
        for (int i = 0; i < 64; i++)
        {
            if (colorChange)
            {
                image.color += new Color32(2, 2, 2, 0);
            }
            anchor.anchoredPosition += new Vector2(0.5f, 0.25f);
            m_LeftCharacterMoved = false;
            yield return null;
        }
    }
    private void LeftCharacterOFF() //左側キャラクターNegative
    {
        StartCoroutine(CoLeftCharacterOFF());
    }
    private IEnumerator CoLeftCharacterOFF()
    {
        Image image = m_LeftCharacter.GetComponent<Image>();
        RectTransform anchor = m_LeftCharacter.GetComponent<RectTransform>();
        anchor.anchoredPosition = new Vector2(-591f, -192f);
        image.color = new Color32(255, 255, 255, 255);
        for (int i = 0; i < 64; i++)
        {
            image.color -= new Color32(2, 2, 2, 0);
            anchor.anchoredPosition -= new Vector2(0.5f, 0.25f);
            m_LeftCharacterMoved = true;
            yield return null;
        }
    }
    private void LeftCharacterEnd() //左側キャラクター非表示
    {
        StartCoroutine(CoLeftCharacterEnd());
    }
    private IEnumerator CoLeftCharacterEnd()
    {
        Image image = m_LeftCharacter.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color -= new Color32(0, 0, 0, 4);
            yield return null;
        }
    }
    private void RightCharacterStart() //右側キャラクター表示
    {
        StartCoroutine(CoRightCharacterStart());
    }
    private IEnumerator CoRightCharacterStart()
    {
        Image image = m_RightCharacter.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color += new Color32(0, 0, 0, 4);
            yield return null;
        }
    }
    private void RightCharacterON(bool colorChange) //右側キャラクターActive
    {
        StartCoroutine(CoRightCharacterON(colorChange));
    }
    private IEnumerator CoRightCharacterON(bool colorChange)
    {
        Image image = m_RightCharacter.GetComponent<Image>();
        RectTransform anchor = m_RightCharacter.GetComponent<RectTransform>();
        anchor.anchoredPosition = new Vector2(-33f, -208f);
        image.color = new Color32(127, 127, 127, 255);
        for (int i = 0; i < 64; i++)
        {
            if (colorChange)
            {
                image.color += new Color32(2, 2, 2, 0);
            }
            anchor.anchoredPosition += new Vector2(-0.5f, 0.25f);
            m_RightCharacterMoved = false;
            yield return null;
        }
    }
    private void RightCharacterOFF() //左側キャラクターNegative
    {
        StartCoroutine(CoRightCharacterOFF());
    }
    private IEnumerator CoRightCharacterOFF()
    {
        Image image = m_RightCharacter.GetComponent<Image>();
        RectTransform anchor = m_RightCharacter.GetComponent<RectTransform>();
        anchor.anchoredPosition = new Vector2(-65f, -192f);
        image.color = new Color32(255, 255, 255, 255);
        for (int i = 0; i < 64; i++)
        {
            image.color -= new Color32(2, 2, 2, 0);
            anchor.anchoredPosition -= new Vector2(-0.5f, 0.25f);
            m_RightCharacterMoved = true;
            yield return null;
        }
    }
    private void RightCharacterEnd() //右側キャラクター非表示
    {
        StartCoroutine(CoRightCharacterEnd());
    }
    private IEnumerator CoRightCharacterEnd()
    {
        Image image = m_RightCharacter.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color -= new Color32(0, 0, 0, 4);
            yield return null;
        }
    }

    private void TextBoxStart() //テキストボックス表示
    {
        StartCoroutine(CoTextBoxStart());
    }
    private IEnumerator CoTextBoxStart()
    {
        Image image = m_TextBox.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color += new Color32(0, 0, 0, 2);
            yield return null;
        }
    }
    private void TextBoxEnd() //テキストボックス非表示
    {
        StartCoroutine(CoTextBoxEnd());
    }
    private IEnumerator CoTextBoxEnd()
    {
        Image image = m_TextBox.GetComponent<Image>();
        for (int i = 0; i < 64; i++)
        {
            image.color -= new Color32(0, 0, 0, 2);
            yield return null;
        }
    }

    private void TalkTextEnd() //トークテキスト終了
    {
        StartCoroutine(CoTalkTextEnd());
    }
    private IEnumerator CoTalkTextEnd()
    {
        for (int i = 0; i < 64; i++)
        {
            m_TalkTextMesh.color -= new Color32(0, 0, 0, 4);
            yield return null;
        }
    }
    private void TalkManagerEnd() //TalkManager非アクティブ化
    {
        StartCoroutine(CoTalkManagerEnd());
    }
    private IEnumerator CoTalkManagerEnd()
    {
        for(int i = 0; i < 71; i++)
        {
            yield return null;
        }
        m_TalkObjectFolder.SetActive(false);
        GameObject.Find("TalkManager").SetActive(false);
    }

    private void LeftCharacterSpriteChange(string charaName, string spriteName) //左側立ち絵変更
    {
        m_LeftCharacter.GetComponent<Image>().sprite = GetCharactorSprite(charaName, spriteName);
    }
    private void RightCharacterSpriteChange(string charaName, string spriteName) //右側立ち絵変更
    {
        m_RightCharacter.GetComponent<Image>().sprite = GetCharactorSprite(charaName, spriteName);
    }

    private void TalkIndicate(string dialog) //トーク進行
    {
        m_TalkTextMesh.text = dialog;
        m_TalkTextMesh.GetComponent<RectTransform>().anchoredPosition = new Vector2(-331f, -445f);
        StartCoroutine(CoTalkIndicate());
    }
    private IEnumerator CoTalkIndicate()
    {
        RectTransform anchor = m_TalkTextMesh.GetComponent<RectTransform>();
        for (int i = 0; i < 20; i++)
        {
            anchor.anchoredPosition += new Vector2(0, 100f) * 1.8f * Time.deltaTime;
            yield return null;
        }
    }

    private string ConvertIntToOrdinal(int number)
    {
        string numberStr = number.ToString();
        if (number == 0) return "0";
        else if ((number / 10) % 10 == 1 | number % 10 >= 4)
            return numberStr + "th ";
        else
        {
            int temp = number % 10;
            switch (temp)
            {
                case 1:
                    return numberStr + "st ";

                case 2:
                    return numberStr + "nd ";

                case 3:
                    return numberStr + "rd ";
            }
        }

        return "";
    }

    private void DebugErrorExe(int row, int number, string exe, string content)
    {
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '<b><i><color=white>" 
                            + exe + "</color></i></b>(" + content + ")' expected.\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string content)//普通
    {
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(<b><i><color=white>" 
                            + content + "</color></i></b>)' expected.\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string content, string postScript)//追記
    {
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(<b><i><color=white>" 
                            + content + "</color></i></b>)' expected. " + postScript + "\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string content, string[] canUseArguments)//使用可能な引数
    {
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(<b><i><color=white>" 
                            + content + "</color></i></b>)' expected. The arguments you can use are '" + string.Join(", ", canUseArguments) + "'\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string content, string[] canUseArguments, int argumentNum)//n番目に使用可能な引数
    {
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(<b><i><color=white>" 
                            + content + "</color></i></b>)' expected. The " + ConvertIntToOrdinal(argumentNum) 
                            + "arguments you can use are '" + string.Join(", ", canUseArguments) + "'\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string[] splitContent, bool[] truefalse)//splitContent
    {
        for (int i = 0; i < splitContent.Length; i++)
        {
            if (!truefalse[i])
            {
                splitContent[i] = "<b><i><color=white>" + splitContent[i] + "</color></i></b>";
            }
        }
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(" 
                            + string.Join(", ", splitContent) + ")' expected.\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string[] splitContent,
                                                bool[] truefalse, string[] canUseArguments)//splitContent 使用可能引数
    {
        for (int i = 0; i < splitContent.Length; i++)
        {
            if (!truefalse[i])
            {
                splitContent[i] = "<b><i><color=white>" + splitContent[i] + "</color></i></b>";
            }
        }
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(" 
                            + string.Join(", ", splitContent) + ")' expected.　The argument you can use are '" 
                            + string.Join(", ", canUseArguments) + "'\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }
    private void DebugErrorContent(int row, int number, string exe, string[] splitContent,
                                                bool[] truefalse, string[] canUseArguments, int argumentNum)//splitContent n番目使用可能引数
    {
        for (int i = 0; i < splitContent.Length; i++)
        {
            if (!truefalse[i])
            {
                splitContent[i] = "<b><i><color=white>" + splitContent[i] + "</color></i></b>";
            }
        }
        Debug.LogError("[OgameTalkScriptEngine]Error in (" + (row + 1) + ", " + number + ") : '" + exe + "(" 
                            + string.Join(", ", splitContent) + ")' expected.　The " + ConvertIntToOrdinal(argumentNum) + "argument you can use are '" 
                            + string.Join(", ", canUseArguments) + "'\n"
                            + "TalkScript name is '" + m_TalkText.name + "'");
    }

    private bool JudgeIsStringNumber(string str)
    {
        List<char> chara = str.ToCharArray().ToList();
        foreach (char c in chara)
        {
            bool condition = Regex.IsMatch(c.ToString(), "[0-9.]");
            if (!condition)
            {
                return false;
            }
        }
        return true;
    }
    private bool JudgeIsStringColorCode(string str)
    {
        List<char> chara = str.ToCharArray().ToList();
        foreach (char c in chara)
        {
            bool condition = Regex.IsMatch(c.ToString(), "[#0-9A-F]");
            if (!condition)
            {
                return false;
            }
        }
        return true;
    }

    private string GetBeforeString(char chr, string orgStr)
    {
        return orgStr.Split(chr)[0];
    }
    private string GetBetweenStrings(string str1, string str2, string orgStr)
    {
        int orgLen = orgStr.Length; //原文の文字列の長さ
        int str1Len = str1.Length; //str1の長さ

        int str1Num = orgStr.IndexOf(str1); //str1が原文のどの位置にあるか

        string s = ""; //返す文字列

        s = orgStr.Remove(0, str1Num + str1Len); //原文の初めからstr1のある位置まで削除
        int str2Num = s.IndexOf(str2); //str2がsのどの位置にあるか
        s = s.Remove(str2Num); //sのstr2のある位置から最後まで削除

        return s; //戻り値
    }
    private Sprite GetCharactorSprite(string charaName, string spriteName)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Talk/Character/" + charaName + "/" + spriteName + ".png");
        return sprite;
    }

    public void SetTalkScript(TextAsset textasset)
    {
        m_TalkText = textasset;
    }
}