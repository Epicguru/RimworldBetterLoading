﻿using System.Collections;
using System.Collections.Generic;

namespace BetterLoading
{
    public static class Utils
    {
        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator) {
            while ( enumerator.MoveNext() ) {
                yield return enumerator.Current;
            }
        }
        
        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator enumerator) {
            while ( enumerator.MoveNext() ) {
                yield return (T) enumerator.Current;
            }
        }
    }
}