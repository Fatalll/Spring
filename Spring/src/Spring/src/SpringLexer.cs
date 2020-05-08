using Antlr4.Runtime;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.Text;

namespace JetBrains.ReSharper.Plugins.Spring
{
    public class SpringLexer : ILexer
    {
        private CLexer _lexer;
        private ITokenStream _stream;

        public SpringLexer(IBuffer buffer)
        {
            Buffer = buffer;
            _lexer = new CLexer(new AntlrInputStream(buffer.GetText()));
            _stream = new BufferedTokenStream(_lexer);
        }

        public void Start() => _stream.Seek(0);

        public void Advance() => _stream.Consume();

        public object CurrentPosition
        {
            get => _stream.Index;
            set => _stream.Seek((int) value);
        }

        public TokenNodeType TokenType => SpringTokenType.GetByIndex(_stream.Get(_stream.Index).Type);
        public int TokenStart => _stream.Get(_stream.Index).StartIndex;
        public int TokenEnd => _stream.Get(_stream.Index).StopIndex + 1;
        public IBuffer Buffer { get; }
    }
}