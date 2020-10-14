using System.Collections.Generic;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
    public interface IScopedTable<T, TK>
    {
        protected IDictionary<TK, T> Table { get; }
        protected Option<IScopedTable<T, TK>> ParentTable { get; }

        protected bool Insert(TK key, T value)
        {
            if (!Table.ContainsKey(key))
            {
                Table.Add(key, value);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Option<T> Get(TK key) =>
            Table.TryGetValue(key) ||
            from pt in ParentTable
            from res in pt.Get(key)
            select res
        ;

        public bool Update(TK key, T entityType)
        {
            if (Table.ContainsKey(key))
            {
                Table[key] = entityType;
                return true;
            }
            else
            {
                return ParentTable
                    .Map(pt => pt.Update(key, entityType))
                    .IfNone(false);
            }
        }
    }
}