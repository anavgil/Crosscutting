using Crosscutting.Persistence.Abstractions.Repositories;

namespace Crosscutting.Persistence.Repositories.Implementation
{
    public class Paginate<TEntity> : IPaginate<TEntity> where TEntity : class, new()
    {
        public int From { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }

        public int Count { get; set; }

        public int Pages { get; set; }

        public IList<TEntity> Items { get; set; }

        public bool HasPrevious => Index - From > 1;

        public bool HasNext => Index - From + 1 < Pages;

        public Paginate()
        {
        }

        public Paginate(IEnumerable<TEntity> source, int index, int size, int from)
        {
            var enumerable = source as TEntity[] ?? source.ToArray();

            if (from > index)
                throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");

            Size = size;
            Index = index;
            From = from;

            if (source is IQueryable<TEntity> queryable)
            {
                Count = queryable.Count();
                Items = [.. queryable.Skip((Index - From) * Size).Take(Size)];
            }
            else
            {
                Count = enumerable.Length;
                Items = enumerable.Skip((Index - From) * Size).Take(Size).ToList();
            }

            Pages = (int)(Math.Ceiling(Count / (double)Size));
        }
    }
}
