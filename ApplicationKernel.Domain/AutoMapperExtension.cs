using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ApplicationKernel.Domain
{
    public static class AutoMapperExtension
    {
        public static T Map<T>(this IMapper mapper, object source, Action<T> afterMap)
        {
            var dst = mapper.Map<T>(source);
            afterMap(dst);
            return dst;
        }
    }
}