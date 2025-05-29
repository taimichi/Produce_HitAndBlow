using System.Collections.Generic;
using System.Text;

/// <summary>
/// ルビ文字列
/// 
/// <ruby=るび>るび</ruby>のような文字列を扱う
/// </summary>
public class RubyString
{
    /// <summary>
    /// パース
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static RubyString Parse(string str)
    {
        if (str is null)
        {
            return null;
        }
        int startIndex = 0;
        int endIndex = 0;

        List<Pair> pairs = new List<Pair>(str.Length);
        while ((startIndex = str.IndexOf("<r=", endIndex)) != -1)
        {
            if (endIndex != startIndex)
            {
                pairs.Add(new Pair(str.Substring(endIndex, startIndex - endIndex), null));
            }
            endIndex = str.IndexOf("</r>", startIndex);
            if (endIndex == -1)
            {
                break;
            }

            startIndex += 3; // "<ruby="
            string rubyText = str.Substring(startIndex, endIndex - startIndex);
            string[] parts = rubyText.Split('>');
            string baseText = parts.Length > 1 ? parts[1] : null;
            string ruby = parts.Length > 0 ? parts[0] : null;
            if (ruby != null && ruby.Length >= 2 && ruby[0] == '"' && ruby[ruby.Length - 1] == '"')
            {
                // ダブルクォーテーションでで囲われているならそれを取り除く
                ruby = ruby.Substring(1, ruby.Length - 2);
            }

            pairs.Add(new Pair(baseText, ruby));
            endIndex += 4; // "</ruby>"
            if (endIndex >= str.Length)
            {
                break;
            }
        }
        if (startIndex == -1)
        {
            if (endIndex < str.Length)
            {
                pairs.Add(new Pair(str.Substring(endIndex), null));
            }
        }
        else if (endIndex == -1)
        {
            pairs.Add(new Pair(str.Substring(startIndex), null));
        }
        return new RubyString(str, pairs);
    }
    private RubyString(string str, List<Pair> data)
    {
        this.data = data;
        var builder = new StringBuilder();
        for (int i = 0; i < data.Count; ++i)
        {
            builder.Append(data[i].Str);
        }
        this.rawString = str;
        this.baseString = builder.ToString();
    }

    /// <summary>
    /// 生の文字列
    /// </summary>
    public string RawString => rawString;

    /// <summary>
    /// ルビを含めない文字列
    /// </summary>
    public string BaseString => baseString;

    /// <summary>
    /// ルビ文字列のセットデータを取得
    /// </summary>
    public IReadOnlyList<Pair> Data => data;

    public override string ToString()
    {
        return rawString;
    }

    public struct Pair
    {
        public Pair(string str, string ruby)
        {
            Str = str;
            Ruby = ruby;
        }
        public string Str;
        public string Ruby;
    }

    private string rawString;
    private string baseString;
    private List<Pair> data;
}
