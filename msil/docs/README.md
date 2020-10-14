# MSIL language short introduction

## Info

MSIL is an assembly language which main idea is the *evaluation **stack***. It means that you can do something like:
```
ldc.i4.1
ldc.i4.2
add
```

And get the summation of constant 1 and 2, while in register-type assembly languages, it would be:

```
add $t3, $t1, $t2
```

## Run

Just do:

```
sh run.sh filename.msil
```

And you will immidiately get a program result. Works for sure for MacOS with installed (Mono Framework)[https://www.mono-project.com/]. Check Windows and Linux versions.

## Instructions

**ldc.type value** – set a constant to the evaluation stack
**ldloc index/name** – locates a local variable
**ldloca index/name** – locates and address
**ldarg index/name** – locates an argument
**ldftn index/name** – locates a function
**ldnull** – just a null

**stloc index/name** – transfers a value from the evaluation stack to a local variable
**starg index/name** – moves a value from the evaluation stack to a method argument

These instructions have the short **.s** format which works only with one byte of data.

In all the cases, index is defined by some number and assigned to each argument or local variable in sequential order starting from 0.

## Definitions

**Locals**
`.locals [init] ([index] local1, [index] localn)` – set the local variables in the beginning of the method. Init initializes all varibles with 0 value.

**Fields**
`.field attributes type fieldname fieldinit at datalabel` – set the fields - global variables of the class. Has attributes: initonly (like readonly), specialname (field is special), rtspecialname (field *has* a special name), public or private. Example:
`.field private initonly int32 fielda`.
Fields can be referensed in the code with something like `ldfld int32 cilFields.fldsDemo::x` where *cilFields.fldsDemo* is a class name.
And `stfld int32 Donis.CSharpBook.ZClass::fielda` fills the fields

**Properties**
`.property attributes return propertyname parameters default {propertyblock}` – set restricted access to some fields. Usually used as getters and setters in the class:
```
.property instance string Color()  
   {  
      .get instance string cilProperties.cPrptDemo::get_Color()  
      .set instance void cilProperties.cPrptDemo::set_Color(string)  
   }  
```


`.entrypoint` – shows which method is main
`.maxstack number` – set the maximum value of entries that can be in the stack

`box type` wraps as value type to become a reference. `unbox` unwraps it.


## Branching

Comparisons:
**ceq** - ==
**cgt** = >
**clt** = <
**and**
**or**

Goto:
**br where** – unconditional jump
**brtrue** – jump when condition in the stack is true
**brfalse** – on false condition

Also, branches and conditions can be combined in one command:
**Beq** - ==
**Bne** - !=
**Bge** - >=
**Bgt** - >
**Ble** - <=
**Blt** - <

## Arrays and structures

**Arrays**

**type [] arrayname** – array is a simple collection of similar types. Can accept the size on the brackets.
**newarr type** – creates an array of size of the number in the stack
**ldelem** – locate array element
**stelem** – store into element
**stelem.ref** – write into the element of reference. For example:
**ldlen** – get length of an array

```
.locals init (string [] names)
ldc.i4.3
newarr string
stloc names // Initialize array of size 3

ldloc names
ldc.i4.0
ldstr "Aqua"
stelem.ref // Fill index 0 of names with the string "Aqua"
```

**Structures**

*To define a structure, declare a type with the class directive and add the value keyword to the class detail. The semantics of a structure are then enforced by the MSIL compiler. The ldloc instruction would load the structure on the evaluation stack. We need the address of the structure. To load the address of a local variable, use the ldloca instruction. Call the constructor of a structure explicitly with the call instruction, not the newobj instruction. Structures are not created on the heap. When the constructor is called directly, the type is simply initialized.*

Check the structures.msil* for the code of the structures. You can see in the, `value` keyword is added to the structure class. It also has a `.ctor(int32)` constructor.

**valuetype Some.Class obj** – initializes a structure.

The rest i don't really understand. Help needed!!!

## Arithmetics

**add**
**sub**
**mul**
**div**
**rem**
**neg**
**conv.ovf** – prevent the overflow when casting from type to type

## Useful

**break** – debugger breakpoint
**castclass** – from one type to another
**dup** – duplicate top element in the stack
**pop** – remove top element in the stack
**dop** – empty instruction (wtf?)

