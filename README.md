# Tiny Language Scanner & Parser
**Course:** CS321 Compiler Design & Theory  
**Academy:** El-Shorouk Academy  

## Project Overview
This repository contains the implementation of the first two phases of the compiler lifecycle for the **Tiny Language**. The project is built using C# (Windows Forms) and focuses on the transition from raw source code to a structurally validated token stream.

## Phase 1: Atomic Lexical Analysis
The Scanner uses a **Regex-based "Master Pattern"** to break source code into individual lexemes. Per the recent architectural clarification, the scanner is strictly **atomic**. 

* **Atomic Processing:** Unlike typical scanners that might group function calls, this implementation separates the identifier, parentheses, and arguments into distinct tokens. 
* **Token Classification:** Each lexeme is categorized into types such as `Keyword`, `Identifier`, `Number`, `Assignment_Statement`, or `Symbol`.
* **Reliability:** The implementation handles nested comments `/* ... */` and prevents "token gluing" in mathematical expressions.

## Phase 2: Syntax Analysis (Recursive Descent Parser)
The Parser receives a `List<Token>` from the Scanner and validates the code against the Tiny Language **Context-Free Grammar (CFG)**.

* [cite_start]**Top-Down Parsing:** Implemented using a recursive descent strategy, starting from `ParseProgram()` and drilling down into statements, expressions, and terms [cite: 278-307].
* **Grammar Support:**
    * Variable declarations (`int`, `float`, `string`).
    * Control flow (`if-then-else-end`, `repeat-until`).
    * I/O operations (`read`, `write`).
    * Operator precedence (handles `* /` before `+ -`).
* [cite_start]**Error Handling:** Triggers clear **Syntax Errors** when the code sequence violates the grammar, such as improper statement starters or missing delimiters [cite: 316, 338-356].

## Technical Integration
[cite_start]The **Token Class** serves as the bridge between the two phases, encapsulating the raw lexeme and its classification [cite: 3-8]. [cite_start]The Parser "consumes" these objects one by one using a pointer index to verify structural integrity [cite: 312-314].

## How to Test
1. Clone the repository and open `.sln` in Visual Studio.
2. Run the application and use the **Factorial Sample** for a full success test.
3. Use a test case like `10 := x;` to verify the Parser's error-detection capabilities.
