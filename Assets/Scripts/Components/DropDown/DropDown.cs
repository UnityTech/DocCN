using System;
using System.Collections.Generic;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace DocCN.Utility.Components
{
    public enum DropDownDirection
    {
        top,
        bottom,
    }
    
    public partial class DropDown<T> : StatefulWidget
    {
        public DropDown(
            Func<Widget> selectBuilder = null,
            IEnumerable<T> items = null,
            Func<T, Widget> itemBuilder = null,
            Color overlayColor = null,
            DropDownDirection direction = DropDownDirection.bottom
        )
        {
            _selectBuilder = selectBuilder;
            _items = items;
            _itemBuilder = itemBuilder;
            _overlayColor = overlayColor;
            _direction = direction;
        }
        
        private readonly Func<Widget> _selectBuilder;

        private readonly IEnumerable<T> _items;

        private readonly Func<T, Widget> _itemBuilder;

        private readonly Color _overlayColor;

        private readonly DropDownDirection _direction;
        public override State createState() => new DropDownState();
    }
}