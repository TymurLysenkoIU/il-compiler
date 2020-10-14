using System.Collections.Generic;
using ILangCompiler.Scanner.Tokens;
using LanguageExt;

namespace ILangCompiler.Parser.AST
{
    public class SymT
    {
        public List<SymTableNode> elements;
        public Option<SymT> parent;

        public SymT()
        {
            elements = new List<SymTableNode>();
        }
        public SymT(SymT parent0)
        {
            elements = new List<SymTableNode>();
            parent = parent0;
        }

        public SymTableNode Get(IdentifierToken identifier)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].TheSame(identifier))
                {
                    return elements[i];
                }
            }
      
            return parent.ToList()[0].Get(identifier);
        }

        public bool Add(IdentifierToken identifier)
        {
            SymTableNode SymTNode = new SymTableNode(identifier);
            elements.Add(SymTNode);
            return true;
        }

        public bool Contain(IdentifierToken identifier)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].TheSame(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainRec(IdentifierToken identifier)
        {
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].TheSame(identifier))
                    {
                        return true;
                    }
                }

                if (parent.IsNone)
                    return false;
                else
                {
                    return parent.ToList()[0].Contain(identifier);
                }
            }    
        }


    }
}