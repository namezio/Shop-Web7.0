using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using WebApp.Database;

namespace WebApp.Common.Repositories;

public class BaseRepository
{
    protected readonly WebAppEntities DbContext;
    protected readonly IDistributedCache Cache;
    
    public BaseRepository(WebAppEntities dbContext, IDistributedCache cache = null)
    {
        DbContext = dbContext;
        Cache = cache;
    }
    
    private string GetValue(string name, object data)
    {
        if (data == null)
            return null;

        var index = name.IndexOf(':');
        var format = index < 0 ? null : name.Substring(index + 1);
        if (data is int i)
            return i.ToString(format);
            
        if (data is long l)
            return l.ToString(format);
            
        if (data is decimal d)
            return d.ToString(format);
            
        if (data is double @double)
            return @double.ToString(format);
            
        if (data is float @float)
            return @float.ToString(format);
            
        if (data is DateTime date)
            return date.ToString(format);
            
        return data.ToString();
    }

    
    protected string RenderHtml(string html, Dictionary<string, object> dict)
    {
        var regex = @"\{(list:)?[a-zA-Z0-9_]+(\.[a-zA-Z0-9_]+)?(\:[a-zA-Z0-9_\/\: ]+)?\}";
        var match = Regex.Match(html, regex, RegexOptions.IgnoreCase);
        var tags = new List<string>();
        var listTags = new List<string>();
        while (match.Success)
        {
            var value = match.Value;
            if (value.StartsWith("{list:"))
                listTags.Add(value.Trim('{', '}').Substring(5));
            else
                tags.Add(value);

            match = match.NextMatch();
        }

        foreach (var tag in tags)
        {
            var parameters = tag.Trim('{', '}').Split('.');
            var primaryKey = parameters[0];
            if (listTags.Contains(primaryKey))
                continue;

            object data;
            var secondaryKey = parameters.Length == 2 ? parameters[1] : string.Empty;
            if (primaryKey.Contains(":"))
            {
                var keyName = primaryKey.Split(':')[0];
                if (!dict.ContainsKey(keyName))
                    continue;

                data = dict[keyName];
            }
            else
            {
                if (!dict.ContainsKey(primaryKey))
                    continue;

                data = dict[primaryKey];
            }


            if (string.IsNullOrEmpty(secondaryKey))
            {
                html = html.Replace(tag, GetValue(primaryKey, data));
                continue;
            }

            var type = data.GetType();
            var property = type.GetProperties().FirstOrDefault(x => x.Name.Equals(secondaryKey.Split(':')[0]));
            if (property == null)
                continue;

            html = html.Replace(tag, GetValue(secondaryKey, property.GetValue(data)));
        }

        foreach (var listTag in listTags)
        {
            if (!dict.ContainsKey(listTag))
                continue;

            var data = ((IEnumerable<dynamic>) dict[listTag]).ToList();
            if (!dict.ContainsKey(listTag))
                continue;

            var beginTag = $"{{list:{listTag}}}";
            var endTag = $"{{!list:{listTag}}}";
            var index = html.IndexOf(beginTag, StringComparison.Ordinal);
            if (index < -1)
                continue;

            var beginHtml = html.Substring(0, index);
            var endHtml = html.Substring(index + beginTag.Length);
            index = endHtml.IndexOf(endTag, StringComparison.Ordinal);
            if (index < -1)
                continue;

            var processHtml = endHtml.Substring(0, index);
            endHtml = endHtml.Substring(index + endTag.Length);
            var dataNumber = 0;
            foreach (var d in data)
            {
                var type = (Type) d.GetType();
                var properties = type.GetProperties();
                var tempHtml = processHtml;
                regex = @"\{" + listTag + @".[a-zA-Z0-9_]+(\:[a-zA-Z0-9_\/]+)?\}";
                match = Regex.Match(processHtml, regex);
                while (match.Success)
                {
                    var value = match.Value;
                    var array = value.Trim('{', '}').Split('.');
                    var secondaryKey = array[1];
                    var name = secondaryKey.Split(':')[0];
                    if (name.Equals("DataNumber"))
                    {
                        tempHtml = tempHtml.Replace(value, GetValue(secondaryKey, dataNumber + 1));
                        match = match.NextMatch();
                        continue;
                    }

                    var property = (PropertyInfo) properties.FirstOrDefault(x => x.Name.Equals(name));
                    if (property == null)
                    {
                        match = match.NextMatch();
                        continue;
                    }

                    tempHtml = tempHtml.Replace(value, GetValue(secondaryKey, property.GetValue(d)));

                    match = match.NextMatch();
                }

                beginHtml += tempHtml;
                dataNumber++;
            }

            html = beginHtml + endHtml;
        }

        return html;
    }

    public virtual void InitData()
    {
        throw new NotImplementedException();
    }
}