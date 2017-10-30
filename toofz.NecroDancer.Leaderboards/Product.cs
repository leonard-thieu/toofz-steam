using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Product
    {
        private Product() { }

        public Product(int productId, string name, string displayName) : this()
        {
            ProductId = productId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        public int ProductId { get; private set; }
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
    }
}
