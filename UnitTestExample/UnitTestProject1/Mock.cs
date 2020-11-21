using System;

namespace UnitTestProject1
{
    internal class Mock<T>
    {
        private object strict;

        public Mock(object strict)
        {
            this.strict = strict;
        }

        internal object Setup(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}