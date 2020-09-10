using ILangCompiler.AST.Types.PrimitiveTypes;
using ILangCompiler.Parser.Abstractions;
using ILangCompiler.Parser.Combinators;
using ILangCompiler.Parser.Exceptions;

namespace ILangCompiler.Parser.Types.PrimitiveTypes
{
  public class PrimitiveTypeParser : EitherParser<
    ParseException,
    IntegerNode,

    ParseException,
    RealNode,

    ParseException,
    BooleanNode,

    ParseException,
    IPrimitiveTypeNode
  >
  {
    public PrimitiveTypeParser() :
      base(new IntegerParser(), new RealParser(), new BooleanParser())
    {
    }
  }
}