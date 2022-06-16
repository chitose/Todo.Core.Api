namespace Todo.Core.Common.Extensions;

public static class StringExtensions
{
    public static string AddCloneSuffix(this string label)
    {
        return label + " Copy";
    }
}