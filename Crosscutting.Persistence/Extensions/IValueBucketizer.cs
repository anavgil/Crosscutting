namespace Crosscutting.Persistence.Extensions;

public interface IValueBucketizer
{
    T[] Bucketize<T>(T[] values);
}
