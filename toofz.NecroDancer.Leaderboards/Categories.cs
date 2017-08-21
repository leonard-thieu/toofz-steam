using System;
using System.Collections.Generic;
using System.Linq;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class Categories : Dictionary<string, Category>
    {
        public Category GetCategory(string categoryName)
        {
            if (categoryName == null)
                throw new ArgumentNullException(nameof(categoryName), $"{nameof(categoryName)} is null.");

            if (TryGetValue(categoryName, out Category c))
            {
                return c;
            }
            else
            {
                throw new ArgumentException($"'{categoryName}' is not a valid category.");
            }
        }

        public string GetItemName(string categoryName, int id)
        {
            var category = GetCategory(categoryName);
            try
            {
                var item = category.Single(i => i.Value.id == id);

                return item.Key;
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException($"Unable to find an item with id '{id}' in '{categoryName}'.", nameof(id), ex);
            }
        }

        public string GetAllItemNames(string categoryName)
        {
            var category = GetCategory(categoryName);
            var itemNames = category.Select(c => c.Key);

            return string.Join(",", itemNames);
        }

        public int GetItemId(string categoryName, string itemName)
        {
            if (itemName == null)
                throw new ArgumentNullException(nameof(itemName), $"{nameof(itemName)} is null.");

            var category = GetCategory(categoryName);
            try
            {
                var item = category[itemName];

                return item.id;
            }
            catch (KeyNotFoundException ex)
            {
                throw new ArgumentException($"Unable to find an item with name '{itemName}' in '{categoryName}'.", nameof(itemName), ex);
            }
        }
    }
}
