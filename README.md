![](/bundle/docs/assets/banner.jpg)

Most people like bananas and we all love cake, so this language is just like banana cake: A combination of the best of two worlds. What does that really mean? I dont know, but it certainly sounds nice. And I like banana cake.

## TL;DR
[Complete Documentation](SPECS.md)

## Project structure

The project contains a central visual studio solution `bcake.sln` that contains all other proejcts. The repo is separated into the following folders:
- bundle: Invokes the entry points of parser and interpreter and references the standard lib
- parser: The parser that reads the code and builds a syntax tree
- runtime: The interpreter that takes the syntax tree and runs it
- stdlib: Some standard methods and types like int and print
- acceptance-tests: Cucumber tests

## Tests

- [Acceptance tests](https://github.com/PixelSnake/bananacake-tests)

### Most important features

* Namespaces, same as in C#
* Global functions outside of classes, because I miss these in C#

## TODO List:

[TODO List](https://github.com/PixelSnake/bananacake/projects/1)
