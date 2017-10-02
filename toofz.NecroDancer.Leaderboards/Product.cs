using System;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Product
    {
        Product() { }

        public Product(int productId, string name, string displayName)
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
