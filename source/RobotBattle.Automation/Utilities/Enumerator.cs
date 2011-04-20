#region Copyright & License

// Copyright (C) 2011 by Alex Lyman
// RobotBattle.Automation is licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php

#endregion

using System;
using System.Collections.Generic;

namespace RobotBattle.Automation
{
    internal static class Enumerator
    {
        public static bool SkipWhile<T>(this IEnumerator<T> enumerator, Predicate<T> predicate)
        {
            bool haveNext;
            while ((haveNext = enumerator.MoveNext()) &&
                   predicate(enumerator.Current)) {
            }
            return haveNext;
        }

        public static bool SkipUntil<T>(this IEnumerator<T> enumerator, Predicate<T> predicate)
        {
            bool haveNext;
            while ((haveNext = enumerator.MoveNext()) &&
                   !predicate(enumerator.Current)) {
            }
            return haveNext;
        }

        public static IEnumerable<T> Take<T>(this IEnumerator<T> enumerator, int count)
        {
            for (var i = 0; enumerator.MoveNext() && i < count; i++) {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> TakeWhile<T>(this IEnumerator<T> enumerator, Predicate<T> predicate)
        {
            while (enumerator.MoveNext() &&
                   predicate(enumerator.Current)) {
                yield return enumerator.Current;
            }
        }

        public static IEnumerable<T> TakeUntil<T>(this IEnumerator<T> enumerator, Predicate<T> predicate)
        {
            while (enumerator.MoveNext() &&
                   !predicate(enumerator.Current)) {
                yield return enumerator.Current;
            }
        }
    }
}