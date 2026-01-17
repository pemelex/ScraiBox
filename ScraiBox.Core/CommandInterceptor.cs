using System.Text.RegularExpressions;

namespace ScraiBox.Core;

public record InterceptedCommand(string Name, string Parameter);

public class CommandInterceptor
{
    private static readonly Regex CommandRegex = new(@"<!cmd:(?<name>[a-zA-Z0-9]+)(?:\:(?<param>.*?))?>", RegexOptions.Compiled);

    public List<InterceptedCommand> InterceptAll(string input)
    {
        var results = new List<InterceptedCommand>();
        var matches = CommandRegex.Matches(input);

        foreach (Match match in matches)
        {
            results.Add(new InterceptedCommand(
                match.Groups["name"].Value,
                match.Groups["param"].Value
            ));
        }

        return results;
    }
}