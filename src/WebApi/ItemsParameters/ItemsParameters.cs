namespace WebApi.ItemsParameters
{
    /// <summary>
    ///     Параметры элементов.
    /// </summary>
    public class ItemsParameters<TClass> where TClass : class
    {
        /// <inheritdoc cref="ItemsParameters&lt;TClass&gt;" />
        /// <param name="items">Перечисление элементов.</param>
        /// <param name="totalCountItems">Общее количество элементов.</param>
        public ItemsParameters(IAsyncEnumerable<TClass> items, int totalCountItems)
        {
            Items = items;
            TotalCountItems = totalCountItems;
        }

        /// <summary>
        ///     Перечисление элементов.
        /// </summary>
        public IAsyncEnumerable<TClass> Items { get; }

        /// <summary>
        ///     Общее количество элементов.
        /// </summary>
        public int TotalCountItems { get; }

        /// <summary>
        ///     Формирование параметров элементов.
        /// </summary>
        /// <param name="source">Запрос элементов.</param>
        /// <param name="currentNumberPage">Номер текущей страницы.</param>
        /// <param name="countItemsPage">Количество элементов на странице.</param>
        /// <returns>Параметры элементов.</returns>
        public static ItemsParameters<TClass> FormationItemsParameters(IQueryable<TClass> source, int currentNumberPage,
            int countItemsPage)
        {
            var totalCountItems = source.Count();
            var items = source.Skip((currentNumberPage - 1) * countItemsPage).Take(countItemsPage).ToAsyncEnumerable();

            return new ItemsParameters<TClass>(items, totalCountItems);
        }
    }
}