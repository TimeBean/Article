using Markdig;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace Article.Web.Pages;

public class Work : PageModel
{
    public bool IsOpen { get; private set; } = true;
    private string _title = "Что случилось 25 января, в 1432-й день войны, самое важное";
    private string _author = "SVTV";
    private string _collection = "Важное";
    private string _mainImage = "https://github.com/TimeBean/static/blob/main/svtv-test.jpg?raw=true";
    
    private MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .DisableHtml()
            .Build();

    public string GetText()
    {
        const string markdownText = "# Заголовок\n" +
                                    "some text\n" +
                                    "- первый __пункт__\n" +
                                    "- второй пункт\n";
            
        var text = Dedent(markdownText);

        return Markdown.ToHtml(
            IsOpen ? text : GetTextPreview(text),
            _pipeline
        );
    }
    
    public string GetTitle() => _title;
    public string GetAuthorName() => _author;
    public string GetCollectionName() => _collection;
    public string GetMainImage() => _mainImage;
    
    private static string GetTextPreview(string text, double percent = 0.2)
    {
        var limit = (int)Math.Round(text.Length * percent);
        if (limit < 1)
            limit = 1;
        if (limit >= text.Length)
            return text;

        var i = limit;
        while (i < text.Length && text[i] != ' ')
            i++;

        var preview = text.Substring(0, i);

        if (preview.EndsWith("."))
        {
            preview = preview.Substring(0, preview.Length - 1) + " ";
        }

        preview = preview.TrimEnd() + "...";
        return preview;
    }

    private static string Dedent(string s)
    {
        var lines = s.Replace("\r\n", "\n").Split('\n');
        var minIndent = int.MaxValue;
        foreach (var line in lines)
        {
            if (line.Trim().Length == 0)
                continue;

            var indent = 0;
            while (indent < line.Length && char.IsWhiteSpace(line[indent]))
                indent++;

            if (indent < minIndent)
                minIndent = indent;
        }

        if (minIndent is int.MaxValue or 0) return s;
        for (var i = 0; i < lines.Length; i++)
            if (lines[i].Length >= minIndent)
                lines[i] = lines[i].Substring(minIndent);
        
        return string.Join("\n", lines);
    }
}