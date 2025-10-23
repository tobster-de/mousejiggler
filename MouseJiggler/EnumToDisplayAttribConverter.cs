using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace MouseJiggler;

public class EnumToDisplayAttribConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        string asString = value?.ToString() ?? throw new ArgumentNullException(nameof(value));
        if (!value.GetType().IsEnum)
        {
            throw new ArgumentException("Value must be an Enumeration type");
        }

        FieldInfo fieldInfo = value.GetType().GetField(asString)!;
        object[] array = fieldInfo.GetCustomAttributes(false);

        foreach (object attrib in array)
        {
            if (attrib is DisplayAttribute displayAttrib)
            {
                //if there is no ressource assume we don't care about localization
                if (displayAttrib.ResourceType == null)
                {
                    return displayAttrib.Name ?? throw new InvalidOperationException($"No display name for {value} provided");
                }

                // per http://stackoverflow.com/questions/5015830/get-the-value-of-displayname-attribute
                ResourceManager resourceManager = 
                    new ResourceManager(displayAttrib.ResourceType.FullName!, displayAttrib.ResourceType.Assembly);
                DictionaryEntry entry =
                    resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)!
                                   .OfType<DictionaryEntry>()
                                   .FirstOrDefault(p => p.Key.ToString() == displayAttrib.Name);

                return entry.Value?.ToString()!;
            }
        }

        //if we get here then there was no attrib, just pretty up the output by spacing on case
        // per http://stackoverflow.com/questions/155303
        string name = Enum.GetName(value.GetType(), value)!;
        return Regex.Replace(name, "([a-z](?=[A-Z0-9])|[A-Z](?=[A-Z][a-z]))", "$1 ");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}