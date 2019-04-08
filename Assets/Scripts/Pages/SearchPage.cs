using System.Collections.Generic;
using DocCN.Components;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace DocCN.Pages
{
    public class SearchPage : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            var size = MediaQuery.of(context).size;
            return new Container(
                constraints: new BoxConstraints(
                    minHeight: size.height
                ),
                width: size.width,
                child: new SingleChildScrollView(
                    child: new ScrollableOverlay(
                        child: new Column(
                            children: new List<Widget>
                            {
                                new Header(),
                                new SearchBar(),
                                new Content<Column>(
                                    child: new Column(
                                        children: new List<Widget>
                                        {
                                            new Text(
                                                "How to get a height of a widget? · Issue #16061 · flutter/flutter · GitHub\nhttps://github.com/flutter/flutter/issues/16061\n翻译此页\n2018年3月29日 - I'm trying to get the height of an arbitrary widget, and I found no ... In that example, they are using MediaQuery to get the size of parent widget.\nflutter中获取元素的大小- flutter探索之路- SegmentFault 思否\nhttps://segmentfault.com/a/1190000015182891\n2018年6月5日 - Flutter的布局体系中，带有大小尺寸的元素并不多，比如SizedBox，ConstrainedBox ... Size get size; ... void visitAncestorElements(bool visitor(Element ... <Widget>[ new Container( key:_myKey, color:Colors.black12, child: new ...\n‎和大小有关的类和方法、属性 · ‎获取到Element · ‎实验一：非ScrollView\nFlutter : Widget Size and Position – Diego Velasquez – Medium\nhttps://medium.com/.../flutter-widget-size-and-position-b0a9ffed9407\n翻译此页\n2018年9月23日 - Ok so now the question is : How can I get the size and position of each panel? Let's focus on just one panel ... child: Container( key: _keyRed, ... flutter: The method 'findRenderObject' was called on null. flutter: Receiver: null\nHow to get constraints like Height and Width of container in ...\nhttps://stackoverflow.com/.../how-to-get-constraints-like-height-and-width-o...\n翻译此页\n2 个回答\n2017年10月19日 - While the answer by Darky is correct, in your case, you don't need to know the container dimensions. A much simpler way is to just have a ...\nget height of a Widget using its GlobalKey in flutter	2018年5月1日\nHow to get a height of a Widget?	2018年3月31日\nSizing elements to percentage of screen width/height	2017年3月30日\nHow can I layout widgets based on the size of the parent?	2017年1月9日\nstackoverflow.com站内的其它相关信息\nHow to get the size constraints in Widget.build - Google Groups\nhttps://groups.google.com/d/topic/flutter-dev/9JAK0WK2EC0\n翻译此页\n[1] http://docs.flutter.io/flutter/widgets/LayoutBuilder-class.html ... I have now the size of the parent and I can compute the best size. However I face a problem with ...\nFlutter 布局（一）- Container详解- 掘金\nhttps://juejin.im/post/5b13c3e1f265da6e3d666d80\n2018年6月3日 - Container在Flutter中太常见了。官方给出的简介，是一个结合了绘制（painting）、定位（positioning）以及尺寸（sizing）widget的widget。 可以得出几个 ...\nContainer class - widgets library - Dart API - Flutter API\nhttps://docs.flutter.io/flutter/widgets/Container-class.html\n翻译此页\nA convenience widget that combines common painting, positioning, and sizing widgets. A container first surrounds the child with padding (inflated by any ...\nSizedBox class - widgets library - Dart API - Flutter\nhttps://docs.flutter.io/flutter/widgets/SizedBox-class.html\n翻译此页\nIf given a child, this widget forces its child to have a specific width and/or height (assuming values are permitted by this widget's parent). If either the width or ...\nWidgets: Container – FlutterDoc\nhttps://flutterdoc.com/widgets-container-d8eee21ad2f4\n翻译此页\n2017年9月6日 - The Container widget is used to contain child widgets whilst also ... otherwise it will wrap the height & width of the given child elements.\nLayout widgets - Flutter\nhttps://flutter.dev › Docs › Development › UI › Widgets › Layout\n翻译此页\nSingle-child layout widgets. Container. A convenience widget that combines common ... Builds a widget tree that can depend on the parent widget's size.\nflutter get container size的相关搜索\nflutter get screen size\n\nflutter get height\n\nflutter get widget position\n\nflutter get child size\n\nflutter布局\n\nflutter flex\n\nflutter screen size\n\nflutter widget position\n\nflutter get screen width\n\nflutter屏幕适配")
                                        }
                                    )
                                ),
                                new Footer(),
                            }
                        )
                    )
                )
            );
        }
    }
}