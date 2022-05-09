using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista11.API
{
    public interface IRepository<T>
    {
        IEnumerable<T> elements { get; }
        T this[int id] { get; }
        T Add(T element);
        T Update(T element);
        void Delete(int id);
        IEnumerable<T> GetNext(int id, int cat);
    }
}
