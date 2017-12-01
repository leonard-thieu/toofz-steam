using System;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Represents a Crypt of the NecroDancer product.
    /// </summary>
    public sealed class Product
    {
        // Required for Entity Framework
        private Product() { }

        /// <summary>
        /// Initializes an instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="productId">A unique identifier for the product.</param>
        /// <param name="name">The product's short name.</param>
        /// <param name="displayName">The product's display name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="displayName"/> is null.
        /// </exception>
        public Product(int productId, string name, string displayName) : this()
        {
            ProductId = productId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        }

        /// <summary>
        /// A unique identifier for the product.
        /// </summary>
        public int ProductId { get; private set; }
        /// <summary>
        /// The product's short name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The product's display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
