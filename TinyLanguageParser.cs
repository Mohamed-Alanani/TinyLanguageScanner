using CompilerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyLanguageScanner;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TinyLanguageScanner
{
    public class TinyLanguageParser
    {
        private List<Token> tokens;
        private int index = 0;
        private Token currentToken;

        public TinyLanguageParser(List<Token> tokens)
        {
            this.tokens = tokens;
            if (tokens.Count > 0)
                currentToken = tokens[index];
        }

        private void Match(string expected)
        {
            if (index < tokens.Count && (currentToken.Value == expected || currentToken.Type == expected))
            {
                index++;
                if (index < tokens.Count)
                    currentToken = tokens[index];
            }
            else
            {
                throw new Exception($"Syntax Error: Expected '{expected}' but found '{currentToken?.Value}'");
            }
        }

        public void ParseProgram()
        {
            ParseStatements();
        }

        private void ParseStatements()
        {
            ParseStatement();
            if (index < tokens.Count && currentToken.Value == ";")
            {
                Match(";");
                if (index < tokens.Count && currentToken.Value != "}" && currentToken.Value != "until" && currentToken.Value != "end")
                {
                    ParseStatements();
                }
            }
        }

        private void ParseStatement()
        {
            if (index >= tokens.Count) return;

            if (currentToken.Value == "int" || currentToken.Value == "float" || currentToken.Value == "string")
                ParseDeclaration();
            else if (currentToken.Type == "Identifier")
                ParseAssignment();
            else if (currentToken.Value == "if")
                ParseIf();
            else if (currentToken.Value == "repeat")
                ParseRepeat();
            else if (currentToken.Value == "read")
                ParseRead();
            else if (currentToken.Value == "write")
                ParseWrite();
            else if (currentToken.Value == "{")
                ParseBlock();
            else if (currentToken.Value == "return")
                Match("return");
            else
                throw new Exception($"Error: Statement cannot start with '{currentToken.Value}'");
        }

        private void ParseDeclaration()
        {
            Match(currentToken.Value);
            ParseAssignment();
        }

        private void ParseAssignment()
        {
            Match("Identifier");
            Match(":=");
            ParseExpression();
        }

        private void ParseIf()
        {
            Match("if");
            ParseCondition();
            Match("then");
            ParseStatements();
            if (index < tokens.Count && currentToken.Value == "else")
            {
                Match("else");
                ParseStatements();
            }
            Match("end");
        }

        private void ParseRepeat()
        {
            Match("repeat");
            ParseStatements();
            Match("until");
            ParseCondition();
        }

        private void ParseRead()
        {
            Match("read");
            Match("Identifier");
        }

        private void ParseWrite()
        {
            Match("write");
            ParseExpression();
        }

        private void ParseBlock()
        {
            Match("{");
            ParseStatements();
            Match("}");
        }

        private void ParseCondition()
        {
            ParseExpression();
            if (currentToken.Value == "<" || currentToken.Value == ">" || currentToken.Value == "=" || currentToken.Value == "<>")
            {
                Match(currentToken.Value);
                ParseExpression();
            }
            else
            {
                throw new Exception("Expected Relational Operator (<, >, =, <>)");
            }
        }

        private void ParseExpression()
        {
            ParseTerm();
            while (index < tokens.Count && (currentToken.Value == "+" || currentToken.Value == "-"))
            {
                Match(currentToken.Value);
                ParseTerm();
            }
        }

        private void ParseTerm()
        {
            ParseFactor();
            while (index < tokens.Count && (currentToken.Value == "*" || currentToken.Value == "/"))
            {
                Match(currentToken.Value);
                ParseFactor();
            }
        }

        private void ParseFactor()
        {
            if (currentToken.Type == "Identifier") Match("Identifier");
            else if (currentToken.Type == "Number") Match("Number");
            else if (currentToken.Value == "(")
            {
                Match("(");
                ParseExpression();
                Match(")");
            }
            else
            {
                throw new Exception($"Expected Identifier or Number but found {currentToken.Value}");
            }
        }
    }
}