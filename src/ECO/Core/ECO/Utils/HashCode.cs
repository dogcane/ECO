namespace ECO.Utils
{
    internal readonly struct HashCode
    {
        internal static int Combine<T1, T2>(T1 t1, T2 t2)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 37 + (t1?.GetHashCode() ?? 0);
                hash = hash * 37 + (t2?.GetHashCode() ?? 0);
                return hash;
            }
        }

        internal static int Combine<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 37 + (t1?.GetHashCode() ?? 0);
                hash = hash * 37 + (t2?.GetHashCode() ?? 0);
                hash = hash * 37 + (t3?.GetHashCode() ?? 0);
                return hash;
            }
        }

        internal static int Combine<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 37 + (t1?.GetHashCode() ?? 0);
                hash = hash * 37 + (t2?.GetHashCode() ?? 0);
                hash = hash * 37 + (t3?.GetHashCode() ?? 0);
                hash = hash * 37 + (t4?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
