using System.Text.RegularExpressions;

namespace Eml.Extensions;

public class UiMessage
{
    private readonly List<KeyValuePair<string, string>>? htmlTagsWithNoPairToReplace;
    private readonly List<string> messageList;
    private string? methodName;

    public bool Any => messageList.Any();

    public UiMessage(string methodName, IEnumerable<string> messages)
    {
        this.methodName = methodName;

        messageList = messages.ToList();
    }

    public UiMessage(IEnumerable<string> messages)
        : this(string.Empty, messages)
    {
    }

    public UiMessage()
    {
        messageList = new List<string>();
    }

    public UiMessage(string methodName, IEnumerable<string> messages, List<KeyValuePair<string, string>> htmlTagsWithNoPairToReplace)
        : this(methodName, messages)
    {
        this.htmlTagsWithNoPairToReplace = htmlTagsWithNoPairToReplace;
    }

    public UiMessage(IEnumerable<string> messages, List<KeyValuePair<string, string>> htmlTagsWithNoPairToReplace)
        : this(string.Empty, messages)
    {
        this.htmlTagsWithNoPairToReplace = htmlTagsWithNoPairToReplace;
    }

    public void SetMethodName(string newMethodName)
    {
        methodName = newMethodName;
    }

    public List<string> GetPrivateMessages()
    {
        return messageList;
    }

    /// <summary>
    ///     For users view only. No debugging details.
    /// </summary>
    /// <returns></returns>
    public string GetHtmlMessages()
    {
        var messages = messageList.ConvertAll(r => r.Replace(Environment.NewLine, "<br>"));
        var message = string.Join("<br>", messages.ToArray());

        return message;
    }

    /// <summary>
    ///     For system logging. Contains information for debugging purposes. Removes all html tags.
    /// </summary>
    /// <returns></returns>
    public string GetMessages()
    {
        const string pairsOfHtmlTags = @"<.*?>|</.*?>";

        var regex = new Regex(pairsOfHtmlTags, RegexOptions.IgnoreCase);
        var messages = messageList.ConvertAll(r => regex.Replace(r, string.Empty));

        if (!string.IsNullOrWhiteSpace(methodName))
        {
            messages.Insert(0, $"Method: {regex.Replace(methodName, string.Empty)}");
        }

        htmlTagsWithNoPairToReplace?.ForEach(tag =>
        {
            messages = messages.ConvertAll(r => r.Replace(tag.Key, tag.Value));
        });

        var message = string.Join(Environment.NewLine, messages.ToArray());

        return message;
    }

    public static KeyValuePair<string, string> GetHtmlTagToReplace(string htmlTagsWithNoPair, string replaceString)
    {
        return new KeyValuePair<string, string>(htmlTagsWithNoPair, replaceString);
    }

    public static KeyValuePair<string, string> GetHtmlTagToReplace(string htmlTagsWithNoPair)
    {
        return GetHtmlTagToReplace(htmlTagsWithNoPair, string.Empty);
    }

    public static List<KeyValuePair<string, string>> GetHtmlTagsToReplace(IEnumerable<string> tags)
    {
        return tags.ToList().ConvertAll(GetHtmlTagToReplace);
    }
}
