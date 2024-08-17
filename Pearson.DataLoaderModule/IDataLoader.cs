using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pearson.Entities;

namespace Pearson.DataLoaderModule
{
    public interface IDataLoader
    {
         public BookDetails Load();
    }
}
