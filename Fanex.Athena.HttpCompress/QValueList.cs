namespace Fanex.Athena.HttpCompress
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    [DebuggerDisplay("QValue[{Count}, {AcceptWildcard}]")]
    internal sealed class QValueList : List<QValue>
    {
        private static readonly char[] Delimiters = { ',' };
        private readonly bool _autoSort;

        private bool _acceptWildcard;

        public QValueList(bool autoSort)
        {
            _autoSort = autoSort;
        }

        public bool AcceptWildcard
        {
            get { return _acceptWildcard; }
        }

        public new void Add(QValue item)
        {
            if (item.Name.Equals("*"))
            {
                _acceptWildcard = item.CanAccept;
            }

            base.Add(item);
            ApplyAutoSort();
        }

        public void DefaultSort()
        {
            Sort(QValue.CompareByWeightDesc);
        }

        public QValue Find(string name)
        {
            return Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public QValue FindPreferred(params string[] candidates)
        {
            return Find(x => IsCandidate(x.Name, candidates) && x.CanAccept);
        }

        private static bool IsCandidate(string item, params string[] candidates)
        {
            return candidates.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase));
        }

        private void ApplyAutoSort()
        {
            if (_autoSort)
            {
                DefaultSort();
            }
        }
    }
}