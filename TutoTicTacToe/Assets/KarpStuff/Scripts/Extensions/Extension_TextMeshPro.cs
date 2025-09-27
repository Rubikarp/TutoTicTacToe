using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Text.RegularExpressions;

public static class Extension_TextMeshPro
{
    public static Tweener DOFade(this TextMeshProUGUI tmp, float value, float duration)
    {
        return DOVirtual.Float(tmp.alpha, value, duration, (alpha) =>
        {
            tmp.alpha = alpha;
        }).SetTarget(tmp);
    }

    public static string ConvertMarkdownToTmpBalise(this string markdown, Color urlColor) => markdown.ConvertMarkdownToTmpBalise(ColorUtility.ToHtmlStringRGB(urlColor));
    public static string ConvertMarkdownToTmpBalise(this string markdown, string urlColorHex = "2980b9")
    {
        string tmp = markdown;

        // Bold: **text** → <b>text</b>
        tmp = Regex.Replace(tmp, @"\*\*(.+?)\*\*", "<b>$1</b>");

        // Italic: *text* or _text_ → <i>text</i>
        tmp = Regex.Replace(tmp, @"(\*|_)(.+?)\1", "<i>$2</i>");

        // Headings: # text → <size=150%><b>text</b></size>
        tmp = Regex.Replace(tmp, @"^# (.+)$", "<size=150%><b>$1</b></size>", RegexOptions.Multiline);
        tmp = Regex.Replace(tmp, @"^## (.+)$", "<size=125%><b>$1</b></size>", RegexOptions.Multiline);
        tmp = Regex.Replace(tmp, @"^### (.+)$", "<size=110%><b>$1</b></size>", RegexOptions.Multiline);

        // Hyperlinks Stylization : [text](url) → <color=#2980b9><u>text</u></color>
        tmp = Regex.Replace(tmp, @"\[(.+?)\]\((.+?)\)", $"<color=#{urlColorHex}><u>$1</u></color>");

        return tmp;
    }
}