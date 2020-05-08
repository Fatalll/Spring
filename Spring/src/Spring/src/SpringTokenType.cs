using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace JetBrains.ReSharper.Plugins.Spring
{
    internal class SpringToken : LeafElementBase, ITokenNode
    {
        private readonly SpringTokenType _type;
        private readonly IBuffer _buffer;
        private readonly TextRange _range;

        public SpringToken(SpringTokenType type, IBuffer buffer, TextRange range)
        {
            _type = type;
            _buffer = buffer;
            _range = range;
        }
            
        public override int GetTextLength() => _range.Length;
        public override string GetText() => _buffer.GetText(_range);
        public override StringBuilder GetText(StringBuilder to) => to.Append(GetText());
        public override IBuffer GetTextAsBuffer() => new StringBuffer(GetText());
        public override NodeType NodeType => _type;
        public override PsiLanguageType Language => SpringLanguage.Instance;
        public TokenNodeType GetTokenType() => _type;
    }
    
    class SpringTokenType : TokenNodeType
    {
        private static readonly SpringTokenType[] Types = new SpringTokenType[121];

        public static readonly SpringTokenType T__0 = new SpringTokenType("T__0", 1);
        public static readonly SpringTokenType T__1 = new SpringTokenType("T__1", 2);
        public static readonly SpringTokenType T__2 = new SpringTokenType("T__2", 3);
        public static readonly SpringTokenType T__3 = new SpringTokenType("T__3", 4);
        public static readonly SpringTokenType T__4 = new SpringTokenType("T__4", 5);
        public static readonly SpringTokenType T__5 = new SpringTokenType("T__5", 6);
        public static readonly SpringTokenType T__6 = new SpringTokenType("T__6", 7);
        public static readonly SpringTokenType T__7 = new SpringTokenType("T__7", 8);
        public static readonly SpringTokenType T__8 = new SpringTokenType("T__8", 9);
        public static readonly SpringTokenType T__9 = new SpringTokenType("T__9", 10);
        public static readonly SpringTokenType T__10 = new SpringTokenType("T__10", 11);
        public static readonly SpringTokenType T__11 = new SpringTokenType("T__11", 12);
        public static readonly SpringTokenType T__12 = new SpringTokenType("T__12", 13);
        public static readonly SpringTokenType T__13 = new SpringTokenType("T__13", 14);
        public static readonly SpringTokenType Auto = new SpringTokenType("Auto", 15);
        public static readonly SpringTokenType Break = new SpringTokenType("Break", 16);
        public static readonly SpringTokenType Case = new SpringTokenType("Case", 17);
        public static readonly SpringTokenType Char = new SpringTokenType("Char", 18);
        public static readonly SpringTokenType Const = new SpringTokenType("Const", 19);
        public static readonly SpringTokenType Continue = new SpringTokenType("Continue", 20);
        public static readonly SpringTokenType Default = new SpringTokenType("Default", 21);
        public static readonly SpringTokenType Do = new SpringTokenType("Do", 22);
        public static readonly SpringTokenType Double = new SpringTokenType("Double", 23);
        public static readonly SpringTokenType Else = new SpringTokenType("Else", 24);
        public static readonly SpringTokenType Enum = new SpringTokenType("Enum", 25);
        public static readonly SpringTokenType Extern = new SpringTokenType("Extern", 26);
        public static readonly SpringTokenType Float = new SpringTokenType("Float", 27);
        public static readonly SpringTokenType For = new SpringTokenType("For", 28);
        public static readonly SpringTokenType Goto = new SpringTokenType("Goto", 29);
        public static readonly SpringTokenType If = new SpringTokenType("If", 30);
        public static readonly SpringTokenType Inline = new SpringTokenType("Inline", 31);
        public static readonly SpringTokenType Int = new SpringTokenType("Int", 32);
        public static readonly SpringTokenType Long = new SpringTokenType("Long", 33);
        public static readonly SpringTokenType Register = new SpringTokenType("Register", 34);
        public static readonly SpringTokenType Restrict = new SpringTokenType("Restrict", 35);
        public static readonly SpringTokenType Return = new SpringTokenType("Return", 36);
        public static readonly SpringTokenType Short = new SpringTokenType("Short", 37);
        public static readonly SpringTokenType Signed = new SpringTokenType("Signed", 38);
        public static readonly SpringTokenType Sizeof = new SpringTokenType("Sizeof", 39);
        public static readonly SpringTokenType Static = new SpringTokenType("Static", 40);
        public static readonly SpringTokenType Struct = new SpringTokenType("Struct", 41);
        public static readonly SpringTokenType Switch = new SpringTokenType("Switch", 42);
        public static readonly SpringTokenType Typedef = new SpringTokenType("Typedef", 43);
        public static readonly SpringTokenType Union = new SpringTokenType("Union", 44);
        public static readonly SpringTokenType Unsigned = new SpringTokenType("Unsigned", 45);
        public static readonly SpringTokenType Void = new SpringTokenType("Void", 46);
        public static readonly SpringTokenType Volatile = new SpringTokenType("Volatile", 47);
        public static readonly SpringTokenType While = new SpringTokenType("While", 48);
        public static readonly SpringTokenType Alignas = new SpringTokenType("Alignas", 49);
        public static readonly SpringTokenType Alignof = new SpringTokenType("Alignof", 50);
        public static readonly SpringTokenType Atomic = new SpringTokenType("Atomic", 51);
        public static readonly SpringTokenType Bool = new SpringTokenType("Bool", 52);
        public static readonly SpringTokenType Complex = new SpringTokenType("Complex", 53);
        public static readonly SpringTokenType Generic = new SpringTokenType("Generic", 54);
        public static readonly SpringTokenType Imaginary = new SpringTokenType("Imaginary", 55);
        public static readonly SpringTokenType Noreturn = new SpringTokenType("Noreturn", 56);
        public static readonly SpringTokenType StaticAssert = new SpringTokenType("StaticAssert", 57);
        public static readonly SpringTokenType ThreadLocal = new SpringTokenType("ThreadLocal", 58);
        public static readonly SpringTokenType LeftParen = new SpringTokenType("LeftParen", 59);
        public static readonly SpringTokenType RightParen = new SpringTokenType("RightParen", 60);
        public static readonly SpringTokenType LeftBracket = new SpringTokenType("LeftBracket", 61);
        public static readonly SpringTokenType RightBracket = new SpringTokenType("RightBracket", 62);
        public static readonly SpringTokenType LeftBrace = new SpringTokenType("LeftBrace", 63);
        public static readonly SpringTokenType RightBrace = new SpringTokenType("RightBrace", 64);
        public static readonly SpringTokenType Less = new SpringTokenType("Less", 65);
        public static readonly SpringTokenType LessEqual = new SpringTokenType("LessEqual", 66);
        public static readonly SpringTokenType Greater = new SpringTokenType("Greater", 67);
        public static readonly SpringTokenType GreaterEqual = new SpringTokenType("GreaterEqual", 68);
        public static readonly SpringTokenType LeftShift = new SpringTokenType("LeftShift", 69);
        public static readonly SpringTokenType RightShift = new SpringTokenType("RightShift", 70);
        public static readonly SpringTokenType Plus = new SpringTokenType("Plus", 71);
        public static readonly SpringTokenType PlusPlus = new SpringTokenType("PlusPlus", 72);
        public static readonly SpringTokenType Minus = new SpringTokenType("Minus", 73);
        public static readonly SpringTokenType MinusMinus = new SpringTokenType("MinusMinus", 74);
        public static readonly SpringTokenType Star = new SpringTokenType("Star", 75);
        public static readonly SpringTokenType Div = new SpringTokenType("Div", 76);
        public static readonly SpringTokenType Mod = new SpringTokenType("Mod", 77);
        public static readonly SpringTokenType And = new SpringTokenType("And", 78);
        public static readonly SpringTokenType Or = new SpringTokenType("Or", 79);
        public static readonly SpringTokenType AndAnd = new SpringTokenType("AndAnd", 80);
        public static readonly SpringTokenType OrOr = new SpringTokenType("OrOr", 81);
        public static readonly SpringTokenType Caret = new SpringTokenType("Caret", 82);
        public static readonly SpringTokenType Not = new SpringTokenType("Not", 83);
        public static readonly SpringTokenType Tilde = new SpringTokenType("Tilde", 84);
        public static readonly SpringTokenType Question = new SpringTokenType("Question", 85);
        public static readonly SpringTokenType Colon = new SpringTokenType("Colon", 86);
        public static readonly SpringTokenType Semi = new SpringTokenType("Semi", 87);
        public static readonly SpringTokenType Comma = new SpringTokenType("Comma", 88);
        public static readonly SpringTokenType Assign = new SpringTokenType("Assign", 89);
        public static readonly SpringTokenType StarAssign = new SpringTokenType("StarAssign", 90);
        public static readonly SpringTokenType DivAssign = new SpringTokenType("DivAssign", 91);
        public static readonly SpringTokenType ModAssign = new SpringTokenType("ModAssign", 92);
        public static readonly SpringTokenType PlusAssign = new SpringTokenType("PlusAssign", 93);
        public static readonly SpringTokenType MinusAssign = new SpringTokenType("MinusAssign", 94);
        public static readonly SpringTokenType LeftShiftAssign = new SpringTokenType("LeftShiftAssign", 95);
        public static readonly SpringTokenType RightShiftAssign = new SpringTokenType("RightShiftAssign", 96);
        public static readonly SpringTokenType AndAssign = new SpringTokenType("AndAssign", 97);
        public static readonly SpringTokenType XorAssign = new SpringTokenType("XorAssign", 98);
        public static readonly SpringTokenType OrAssign = new SpringTokenType("OrAssign", 99);
        public static readonly SpringTokenType Equal = new SpringTokenType("Equal", 100);
        public static readonly SpringTokenType NotEqual = new SpringTokenType("NotEqual", 101);
        public static readonly SpringTokenType Arrow = new SpringTokenType("Arrow", 102);
        public static readonly SpringTokenType Dot = new SpringTokenType("Dot", 103);
        public static readonly SpringTokenType Ellipsis = new SpringTokenType("Ellipsis", 104);
        public static readonly SpringTokenType Identifier = new SpringTokenType("Identifier", 105);
        public static readonly SpringTokenType Constant = new SpringTokenType("Constant", 106);
        public static readonly SpringTokenType DigitSequence = new SpringTokenType("DigitSequence", 107);
        public static readonly SpringTokenType StringLiteral = new SpringTokenType("StringLiteral", 108);
        public static readonly SpringTokenType ComplexDefine = new SpringTokenType("ComplexDefine", 109);
        public static readonly SpringTokenType IncludeDirective = new SpringTokenType("IncludeDirective", 110);
        public static readonly SpringTokenType AsmBlock = new SpringTokenType("AsmBlock", 111);

        public static readonly SpringTokenType LineAfterPreprocessing = new SpringTokenType("LineAfterPreprocessing", 112);

        public static readonly SpringTokenType LineDirective = new SpringTokenType("LineDirective", 113);
        public static readonly SpringTokenType PragmaDirective = new SpringTokenType("PragmaDirective", 114);
        public static readonly SpringTokenType Whitespace = new SpringTokenType("Whitespace", 115);
        public static readonly SpringTokenType Newline = new SpringTokenType("Newline", 116);
        public static readonly SpringTokenType BlockComment = new SpringTokenType("BlockComment", 117);
        public static readonly SpringTokenType LineComment = new SpringTokenType("LineComment", 118);
        public static readonly SpringTokenType Anythings = new SpringTokenType("Anythings", 119);
        public static readonly SpringTokenType Anything = new SpringTokenType("Anything", 120);

        public SpringTokenType(string s, int index) : base(s, index)
        {
            Types[index] = this;
        }

        public static SpringTokenType GetByIndex(int index)
        {
            return index == -1 ? null : Types[index];
        }

        public override LeafElementBase Create(IBuffer buffer, TreeOffset startOffset, TreeOffset endOffset)
        {
            return new SpringToken(this, buffer, new TextRange(startOffset.Offset, endOffset.Offset));
        }


        public override bool IsWhitespace => this == Whitespace || this == Newline;
        public override bool IsComment => this == LineComment || this == BlockComment;
        public override bool IsStringLiteral => this == StringLiteral;
        public override bool IsConstantLiteral => this == Constant;
        public override bool IsIdentifier => this == Identifier;

        public override bool IsKeyword => new HashSet<SpringTokenType>
        {
            Auto, Break, Case, Char, Const, Continue, Default, Do, Double, Else, Enum, Extern, Float, For, Goto, If,
            Inline, Int, Long, Register, Restrict, Return, Short, Signed, Sizeof, Static, Struct, Switch, Typedef,
            Union, Unsigned, Void, Volatile, While, Alignas, Alignof, Atomic, Bool
        }.Contains(this);

        public override string TokenRepresentation { get; }
    }
}