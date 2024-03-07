using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crosscutting.Persistence.Extensions;

public interface IValueBucketizer
{
    T[] Bucketize<T>(T[] values);
}
