using Unity.UIWidgets.ui;
using UnityEngine;
using Unity.UIWidgets.widgets;

/**
 * Warning: DO NOT EDIT THIS GENERATED FILE
 * Call LoadIconFont only when initialize if you want to using IconFont Prefix Icon Data 
 */

namespace DocCN.Style
{
    public static partial class Icons
    {
        public static void LoadIconFont()
        {
            FontManager.instance.addFont(Resources.Load<Font>("Fonts/IconFont"), "IconFont");
        }
        
        <% @glyphs.each do |name, value| selector = @options[:css_selector].sub('{{glyph}}', name.to_s) %>
        public static readonly IconData <%=font_name%><%=name%> = new IconData(0x<%=value[:codepoint].to_s(16)%>, fontFamily: "<%=font_name%>");
        <% end %>
    }
}