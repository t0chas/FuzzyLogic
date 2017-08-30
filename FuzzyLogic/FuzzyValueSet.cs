using System;
using System.Collections.Generic;

namespace Tochas.FuzzyLogic
{
    public partial class FuzzyValueSet
    {

        private Container container;

        public FuzzyValueSet(int size = 100)
        {
            this.container = new Container(size);
        }

        private FuzzyValueSet(Container container)
        {
            this.container = container;
        }

        public void Set<T>(FuzzyValue<T> fuzzyValue) where T : struct, IConvertible
        {
            this.container.Set(fuzzyValue);
        }

        public FuzzyValue<T> Get<T>(T linguisticVariable) where T : struct, IConvertible
        {
            return this.container.Get(linguisticVariable);
        }

        public FuzzyValueSet Clone()
        {
            return new FuzzyValueSet(this.container.Clone());
        }
    }
}
