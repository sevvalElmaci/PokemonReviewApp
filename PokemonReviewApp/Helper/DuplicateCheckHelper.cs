using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonReviewApp.Helpers
{
    public static class DuplicateCheckHelper
    {
        public static bool ExistsDuplicate<T>(
            IEnumerable<T> allItems,
            Func<T, string> propertySelector,
            string newValue,
            int currentId,
            Func<T, int> idSelector)
        {
            if (string.IsNullOrWhiteSpace(newValue))
                return false;

            string normalized = newValue.Trim().ToLower();

            return allItems.Any(item =>
                propertySelector(item).Trim().ToLower() == normalized &&
                idSelector(item) != currentId);
        }
    }
}
