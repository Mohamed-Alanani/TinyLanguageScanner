using System.Data;
using System.Text.RegularExpressions;
using TinyLanguageScanner;

namespace CompilerProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputString = textBox1.Text;

            string comments = @"(/\*.*?\*/)";
            string strings = @"("".*?"")";
            string keywords = @"\b(int|float|string|read|write|repeat|until|if|then|else|end|return|main)\b";
            string numbers = @"\b\d+(\.\d+)?\b";
            string identifiers = @"\b[a-zA-Z][a-zA-Z0-9]*\b";
            string operators = @"(:=)|(<>|<|>|=)|(&&|\|\|)|([+\-*/])";
            string symbols = @"([;(),{}])";

            string masterPattern = $"{comments}|{strings}|{keywords}|{numbers}|{identifiers}|{operators}|{symbols}";

            DataTable dt = new DataTable();
            dt.Columns.Add("Lexeme");
            dt.Columns.Add("Token Type");

            MatchCollection matches = Regex.Matches(inputString, masterPattern);
            List<Token> allTokens = new List<Token>();

            foreach (Match m in matches)
            {
                string lex = m.Value.Trim();
                string type = "Unknown";

                if (Regex.IsMatch(lex, "^" + keywords + "$")) type = "Keyword";
                else if (Regex.IsMatch(lex, "^" + numbers + "$")) type = "Number";
                else if (Regex.IsMatch(lex, "^" + identifiers + "$")) type = "Identifier";
                else if (Regex.IsMatch(lex, "^" + strings + "$")) type = "String";
                else if (Regex.IsMatch(lex, "^" + comments + "$")) type = "Comment_Statement";
                else if (lex == ":=") type = "Assignment_Statement";
                else if (lex == "+" || lex == "-" || lex == "*" || lex == "/") type = "Arithmatic_Operator";
                else if (lex == "<" || lex == ">" || lex == "=" || lex == "<>") type = "Condition_Operator";
                else if (lex == "&&" || lex == "||") type = "Boolean_Operator";
                else if (Regex.IsMatch(lex, "^" + symbols + "$")) type = "Symbol";

                allTokens.Add(new Token { Value = lex, Type = type });
                dt.Rows.Add(lex, type);
            }

            dataGridView1.DataSource = dt;

            try
            {
                if (allTokens.Count == 0) return;
                TinyLanguageParser parser = new TinyLanguageParser(allTokens);
                parser.ParseProgram();
                MessageBox.Show("Success: Code is structurally correct!", "Parser Result");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Syntax Error: " + ex.Message, "Parser Error");
            }
        }
    }
}
