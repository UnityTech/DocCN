using System;
using System.Collections.Generic;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public enum DropDownDirection
    {
        top,
        bottom,
    }

    public enum DropDownOverlayType
    {
        scrollable,
        builtin,
    }
    
    public partial class DropDown<T> : StatefulWidget
    {
        public DropDown(
            Func<Widget> selectBuilder = null,
            IEnumerable<T> items = null,
            Func<DropDownState, T, Widget> itemBuilder = null,
            Color overlayColor = null,
            Border overlayBorder = null,
            DropDownDirection direction = DropDownDirection.bottom,
            DropDownOverlayType overlayType = DropDownOverlayType.scrollable 
        )
        {
            _selectBuilder = selectBuilder;
            _items = items;
            _itemBuilder = itemBuilder;
            _overlayColor = overlayColor;
            _direction = direction;
            _overlayBorder = overlayBorder;
            _overlayType = overlayType;
        }
        
        private readonly Func<Widget> _selectBuilder;

        private readonly IEnumerable<T> _items;

        private readonly Func<DropDownState, T, Widget> _itemBuilder;

        private readonly Color _overlayColor;

        private readonly DropDownDirection _direction;

        private readonly Border _overlayBorder;

        private readonly DropDownOverlayType _overlayType;
        public override State createState() => new DropDownState();
    }
}