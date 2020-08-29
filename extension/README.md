# Imperative language LSP extension

Language Server Protocol – highly powerful tool for syntax higlight, auto-complete, validation etc. for your language.

This extension provides an LSP (server + client) for the imperative language with following grammar:

```
Program : { SimpleDeclaration | RoutineDeclaration }

SimpleDeclaration : VariableDeclaration | TypeDeclaration

VariableDeclaration
    : var Identifier ':' Type [ is Expression ]
	| var Identifier is Expression

TypeDeclaration : type Identifier is Type

RoutineDeclaration
    : routine Identifier ( Parameters ) [ ':' Type ]
    is Body end

Parameters
    : ParameterDeclaration { ',' ParameterDeclaration }

ParameterDeclaration
    : Identifier : Type

Type : PrimitiveType | ArrayType | RecordType | Identifier

PrimitiveType : integer | real | boolean

RecordType : record { VariableDeclaration } end

ArrayType : array '[' [ Expression ] ']' Type


Body : { SimpleDeclaration | Statement }

Statement : Assignment | RoutineCall
    | WhileLoop | ForLoop | IfStatement 

Assignment : ModifiablePrimary ':=' Expression

RoutineCall
    : Identifier [ '(' Expression { ',' Expression } ')' ]

WhileLoop : while Expression loop Body end

ForLoop : for Identifier Range loop Body end

Range : in [ reverse ] Expression .. Expression

IfStatement : if Expression then Body [ else Body ] end


Expression : Relation { ( and | or | xor ) Relation }

Relation : Simple
	[ ( '<' | '<=' | '>' | '>=' | '=' | '/=' ) Simple ] Simple :Factor{('*'|'/'|'%')Factor}

Simple : Factor{( * | / | % )Factor}

Factor : Summand { ( '+' | '-' ) Summand }

Summand : Primary | '(' Expression ')'

Primary : [ Sign | not ] IntegerLiteral
    | [ Sign ] RealLiteral
    | true
    | false
    | ModifiablePrimary
    | RoutineCall

Sign : '+' | '-'

ModifiablePrimary
    : Identifier 
    	{ '.' Identifier
    	| '[' Expression ']'
        }

```