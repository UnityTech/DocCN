using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace Unity.DocZh.Components
{
    public partial class Footer
    {
        public class Style
        {
            public Color bgColor { get; set; }
            public TextStyle siteLinkStyle { get; set; }
            public TextStyle legalLinkStyle { get; set; }
            public TextStyle hintTextStyle { get; set; }
            public TextStyle copyRightStyle { get; set; }

            public ImageProvider unityBrand { get; set; }
            
            public Color langSelectTextColor { get; set; }
            public Color langUnderlineColor { get; set; }
            public Color langExpandIconColor { get; set; }
            public bool horizontalPadding { get; set; }
            public TextStyle copyrightLabelStyle { get; set; }
            public TextStyle copyrightTextStyle { get; set; }
        }

        public static readonly Style Dark = new Style
        {
            bgColor = new Color(0xff111111),
            siteLinkStyle = new TextStyle(
                fontSize: 14f,
                color: new Color(0xffd8d8d8),
                decoration: TextDecoration.underline
            ),
            legalLinkStyle = new TextStyle(
                fontSize: 14.0f,
                color: new Color(0xff2196f3),
                decoration: TextDecoration.underline
            ),
            hintTextStyle = new TextStyle(
                fontSize: 14.0f,
                color: new Color(0xffd8d8d8)
            ),
            copyRightStyle = new TextStyle(
                fontSize: 14f,
                color: new Color(0xffd8d8d8)
            ),
            unityBrand = new AssetImage("Images/unity-master-white"),
            langSelectTextColor = new Color(0xffffffff),
            langUnderlineColor = new Color(0xffffffff),
            langExpandIconColor = new Color(0xffffffff),
            horizontalPadding = true,
            copyrightLabelStyle = new TextStyle(
                color: new Color(0xffffffff),
                fontSize: 12
            ),
            copyrightTextStyle = new TextStyle(
                color: new Color(0xffffffff),
                fontSize: 16,
                fontWeight: FontWeight.w500
            ),
        };

        public static readonly Style Light = new Style
        {
            bgColor = new Color(0xffffffff),
            siteLinkStyle = new TextStyle(
                fontSize: 14f,
                color: new Color(0xff616161),
                decoration: TextDecoration.underline
            ),
            legalLinkStyle = new TextStyle(
                fontSize: 14.0f,
                color: new Color(0xff2196f3),
                decoration: TextDecoration.underline
            ),
            hintTextStyle = new TextStyle(
                fontSize: 14.0f,
                color: new Color(0xff616161)
            ),
            copyRightStyle = new TextStyle(
                fontSize: 14f,
                color: new Color(0xff616161)
            ),
            unityBrand = new AssetImage("Images/unity-master-black"),
            langSelectTextColor = new Color(0xff616161),
            langUnderlineColor = new Color(0xffd8d8d8),
            langExpandIconColor = new Color(0xffd8d8d8),
            horizontalPadding = false,
            copyrightLabelStyle = new TextStyle(
                color: new Color(0xff000000),
                fontSize: 12
            ),
            copyrightTextStyle = new TextStyle(
                color: new Color(0xff000000),
                fontSize: 16,
                fontWeight: FontWeight.w500
            ),
        };
    }
}