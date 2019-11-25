using System;
using System.Collections.Generic;
using System.Linq;
using Unity.DocZh.Style;
using Unity.DocZh.Utility;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public enum Language
    {
        enUs,
        esEs,
        jaJp,
        koKr,
        ruRu,
        zhCn,
    }

    public static class LanguagesExtension
    {
        public static string FontFamily(this Language l)
        {
            return l == Language.koKr ? "NotoSans" : "PingFang";
        }
        
        public static string Text(this Language l)
        {
            switch (l)
            {
                case Language.enUs:
                    return "English";
                case Language.esEs:
                    return "Español";
                case Language.jaJp:
                    return "日本語";
                case Language.koKr:
                    return "한국어";
                case Language.ruRu:
                    return "Русский";
                case Language.zhCn:
                    return "简体中文";
                default:
                    throw new ArgumentOutOfRangeException(nameof(l), l, null);
            }
        }

        public static string Link(this Language l)
        {
            switch (l)
            {
                case Language.enUs:
                    return "https://docs.unity3d.com/Manual/UnityManual.html";
                case Language.esEs:
                    return "https://docs.unity3d.com/es/current/Manual/UnityManual.html";
                case Language.jaJp:
                    return "https://docs.unity3d.com/ja/current/Manual/UnityManual.html";
                case Language.koKr:
                    return "https://docs.unity3d.com/kr/current/Manual/UnityManual.html";
                case Language.ruRu:
                    return "https://docs.unity3d.com/ru/current/Manual/UnityManual.html";
                case Language.zhCn:
                    return "https://docs.unity3d.com/zh/current/Manual/UnityManual.html";
                default:
                    throw new ArgumentOutOfRangeException(nameof(l), l, null);
            }
        }
    }

    public class LanguageItem : StatefulWidget
    {
        public LanguageItem(
            Language language,
            DropDown<Language>.DropDownState parentState,
            Key key = null) : base(key)
        {
            _language = language;
            _parentState = parentState;
        }

        private readonly Language _language;
        private readonly DropDown<Language>.DropDownState _parentState;
        public override State createState() => new LanguageItemState();

        private static readonly Color HoverColor = new Color(0xffd8d8d8);
        private static readonly Color NormalColor = new Color(0xffffffff);

        private class LanguageItemState : State<LanguageItem>
        {
            private bool _hover;

            public override void initState()
            {
                base.initState();
                _hover = false;
            }

            private void OnHoverChanged(bool hover)
            {
                if (mounted)
                {
                    setState(() => _hover = hover);
                }
            }

            private void OnTap()
            {
                if (widget._language != Language.zhCn)
                {
                    LocationUtil.HrefTo(widget._language.Link());
                }

                widget._parentState.Dismiss();
            }

            public override Widget build(BuildContext context)
            {
                return new Clickable(
                    onTap: OnTap,
                    hoverChanged: OnHoverChanged,
                    child: new Container(
                        width: 178f,
                        height: 48f,
                        padding: EdgeInsets.symmetric(horizontal: 8f),
                        color: _hover ? HoverColor : NormalColor,
                        child: new Align(
                            alignment: Alignment.centerLeft,
                            child: new Text(
                                widget._language.Text(),
                                style: new TextStyle(
                                    fontSize: 16,
                                    color: new Color(0xff616161),
                                    fontFamily: widget._language.FontFamily()
                                )
                            )
                        )
                    )
                );
            }
        }
    }

    public partial class LanguageSelect
    {
        private class LanguageSelectState : State<LanguageSelect>
        {
            public override Widget build(BuildContext buildContext)
            {
                return new DropDown<Language>(
                    direction: DropDownDirection.top,
                    items: Enum.GetValues(typeof(Language)).Cast<Language>(),
                    overlayBorder: Border.all(color: new Color(0xffd8d8d8), width: 1f),
                    itemBuilder: (state, lang) => new LanguageItem(lang, state),
                    overlayColor: new Color(0xffd8d8d8),
                    selectBuilder: () => new Container(
                        height: 32f,
                        decoration: new BoxDecoration(
                            border: new Border(
                                bottom: new BorderSide(
                                    width: 1f,
                                    color: widget._underlineColor
                                )
                            )
                        ),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: new List<Widget>
                            {
                                new Text(
                                    "简体中文",
                                    style: new TextStyle(
                                        fontSize: 16f,
                                        color: widget._selectTextColor
                                    )
                                ),
                                new Icon(
                                    Icons.MaterialExpandMore,
                                    color: widget._expandIconColor
                                )
                            }
                        )
                    )
                );
            }
        }
    }
}