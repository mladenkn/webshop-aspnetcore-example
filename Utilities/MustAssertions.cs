using System;

namespace Utilities
{
    public struct MustAssertions
    {
        private readonly object _o;

        public MustAssertions(object o)
        {
            _o = o;
        }

        public void NotBeNull()
        {
            if(_o == null)
                throw new NullReferenceException();
        }
    }
}
