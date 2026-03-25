# Calculator
A complex calculator project focusing on:
- State mechanics,
- Compiler design,
- Formal language,
- Use of data structures,
- Mathematics,
- Object-oriented programming.

## Calculator types
There is currently only one calculator type:
1. **`Arithmetic`**: Contains brackets, the four basic mathematical operations along with exponentiation `^` and modulo `%`, numbers, and boolean values `TRUE` and `FALSE`.

## Tokenization
When calculating an expression, it yields an array of tokens for the code to parse and read. Here is an example for the `Arithmetic` calculator type:
```
(-2+6PI) * 3
```
|Token type|String|
|-|-|
|Operator|`(`|
|Operator|`-`|
|Number|2|
|Operator|`+`|
|Number|6|
|Number|`PI` = 3.14...|
|Operator|`)`|
|Whitespace *(ignored)*|` `|
|Operator|`*`|
|Whitespace *(ignored)*|` `|
|Number|3|

Tokenization can be customied for each calculator type.

## Parsing
The `Arithmetic` calculator type uses an extended version of Edgster Dijkstra's [shunting yard algorithm](https://en.wikipedia.org/wiki/Shunting_yard_algorithm) to turn an array of tokens into [reverse Polish notation](https://en.wikipedia.org/wiki/Reverse_Polish_notation).

The `Arithmetic` parser contains the extended features:
- **Conversion of unary and binary operations**: Can identify if the `-` in `-5` is an unary (single operand) `u-` or binary `-` subtraction operations.
- **Parsing errors**: Can detect where operators and operands (numbers, variables, etc.) are supposed to go.
- **Transitivity of operations**: Can convert an expression `5 > 4 > 3` to make sure that the evaluator correctly performs the operations `5 > 4` and `4 > 3` instead of `(5 > 4) = TRUE`, making it `TRUE > 3`.
- **Functions**: Can correctly identify which function given the name and number of parameters to use.
    - **Bracket detection**: Can correctly determine if brackets `(x)` invocates a function or not.
    - **Automatic function argument**: Can accept a bracketless invoked function and takes the first value such as `SQRTPI` or `SQRT PI` being `SQRT(PI)`. (Note: `SQRT2PI` or `SQRT 2PI` would be converted to `SQRT(2) * PI`.)
- **Implicit multiplication**: Can join a coefficient, variables, and brackets together such as `2PISQRT(5)` or `2 PI SQRT(5)` turning to `2 * PI * SQRT(5)`.

The example array of tokens would then be parsed:
|Token type|String|
|-|-|
|Number|2|
|Number|6|
|Number|`PI` = 3.14...|
|Operator|`*`|
|Operator|`+`|
|Operator|`u-` *(unary subtraction)*|
|Number|3|
|Operator|`*`|

## List of operators
Below is a table of operators in the `Arithmetic` calculator type:
|Operator|Name|Description|
|-|-|---|
|`+`|**Addition**|Adds two numbers together. Unary variant is `u+` as in `+5`.|
|`-`|**Subtraction**|Subtracts two numbers together. Unary variant is `u-` as in `-5`.|
|`*`|**Multiplcation**|Multiplies two numbers together. Can be implicit.|
|`/`|**Division**|Divides two numbers together.|
|`%`|**[Modulo](https://en.wikipedia.org/wiki/Modulo)**|Takes the remainder of two numbers.|
|`^`|**Exponentiation**|Where `a ^ b`, it performs the operation $a^b$.|
|`=`|**Is equal to**|Yields `TRUE` if `a` and `b` have the same value for `a = b`, else `FALSE`. However, if `a` is an unknown variable, it assigns `a` to the value of `b` and yields `TRUE`, noting that the variable was assigned.|
|`!=`|**Is not equal to**|Yields the opposite value for `a = b` for `a != b`.|
|`>`|**Is greater than**|Yields `TRUE` if `a` has a greater value than `b` for `a > b`, else `FALSE`.|
|`<`|**Is less than**|Yields `TRUE` if `a` has a lesser value than `b` for `a < b`, else `FALSE`.|
|`>=`|**Is greater than or equal to**|Combination of `=` and `>`.|
|`<=`|**Is less than or equal to**|Combination of `=` and `<`.|
|`&&`|**And**|If `a` and `b` are `TRUE`, yield `TRUE`, else `FALSE`.|
|`not!`|**Not**|Yield `FALSE` if `TRUE` and `TRUE` if `FALSE`.|
|`->`|**Assign**|Assigns the given variable on the left to the value on the right, and returns the value on the right. For example, `TAU -> 2PI` would assign `TAU` to `2 * PI` and return the value of `2 * PI`.|

## List of functions
Below is a list of functions:
|Function|Expression|Description|
|-|--|---|
|`SQRT(n)`|`n^0.5`, or $n^{0.5}$|Takes the square root of a number.|
|`CBRT(n)`|`n^(1/3)`, or $n^\frac{1}{3}$|Takes the cube root of a number.|
