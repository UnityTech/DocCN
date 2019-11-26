using System;
using System.Collections.Generic;
using System.Linq;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace Unity.DocZh.Components
{
    public partial class Pager : StatelessWidget
    {
        public Pager(
            List<int> pages = null,
            int totalPages = 0,
            int currentPage = 0,
            Action<int> onPageChanged = null,
            Key key = null) : base(key)
        {
            _pages = pages;
            _totalPages = totalPages;
            _currentPage = currentPage;
            _onPageChanged = onPageChanged;
        }

        private readonly List<int> _pages;
        private readonly int _totalPages;
        private readonly int _currentPage;
        private readonly Action<int> _onPageChanged;
        private const string PreviousPage = "<";
        private const string NextPage = ">";

        public override Widget build(BuildContext context)
        {
            var children = _pages.Select<int, Widget>(
                page => new PagerItem(
                    label: (page + 1).ToString(),
                    onTap: () =>
                    {
                        if (_currentPage != page)
                        {
                            _onPageChanged?.Invoke(page);
                        }
                    },
                    active: _currentPage == page
                )
            ).ToList();
            if (_currentPage != 0)
            {
                children.Insert(
                    0,
                    new PagerItem(
                        label: PreviousPage,
                        onTap: () => _onPageChanged?.Invoke(_currentPage - 1)
                    )
                );
            }

            if (_currentPage != _totalPages - 1)
            {
                children.Add(
                    new PagerItem(
                        label: NextPage,
                        onTap: () => _onPageChanged?.Invoke(_currentPage + 1)
                    )
                );
            }

            return new Row(
                children: children
            );
        }
    }
}