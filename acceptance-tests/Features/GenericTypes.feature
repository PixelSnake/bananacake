Feature: Generic types

    Types can take an arbitrary amount of type arguments inside <> brackets.

    Scenario: Generic type can be used in declaration
        Given the following class is defined:
            """
            class Collection<T> {}
            """
        And the main function contains the following code:
            """
            Collection<int> collection;
            """
        When the code is compiled
        Then there are no errors

    Scenario: Generic type constructor can be invoked
        Given the following class is defined:
            """
            class Collection<T> {
                public Collection() {}
            }
            """
        And the main function contains the following code:
            """
            new Collection<int>();
            """
        When the code is compiled
        Then there are no errors

    Scenario: Empty generic type constructor is automatically generated
        Given the following class is defined:
            """
            class Collection<T> {}
            """
        And the main function contains the following code:
            """
            new Collection<int>();
            """
        When the code is compiled
        Then there are no errors

    @Error
    Scenario: Not providing type argument to constructor returns error
        Given the following class is defined:
            """
            class Collection<T> {
                public Collection() {}
            }
            """
        And the main function contains the following code:
            """
            new Collection();
            """
        When the code is compiled
        Then an error is returned
        And the error contains "Illegal use of generic symbol"

    @Error
    Scenario: Not providing type argument to default constructor returns error
        Given the following class is defined:
            """
            class Collection<T> {}
            """
        And the main function contains the following code:
            """
            new Collection();
            """
        When the code is compiled
        Then an error is returned
        And the error contains "Illegal use of generic symbol"

    Scenario: Constructors of generic classes can have parameters
        Given the following class is defined:
            """
            class Collection<T> {
                public Collection(int x) {
                    TEST("x", x);
                }
            }
            """
        And the main function contains the following code:
            """
            new Collection<int>(1337);
            """
        When the code is compiled
        Then there are no errors
        And "x" evaluates to 1337

    Scenario: Constructors of generic classes can have parameters of the generic type
        Given the following class is defined:
            """
            class Collection<T> {
                public Collection(T x) {}
            }
            """
        And the main function contains the following code:
            """
            new Collection<int>(1337);
            """
        When the code is compiled
        Then there are no errors
        
    @Regression
    Scenario: Assignment of generic types with equal type arguments works
        Given the following class is defined:
            """
            class Collection<T> {}
            """
        And the main function contains the following code:
            """
            Collection<int> collection = new Collection<int>();
            """
        When the code is compiled
        Then there are no errors

    @Regression @Error
    Scenario: Assignment of generic types with different type arguments does not work
        Given the following class is defined:
            """
            class Collection<T> {}
            """
        And the main function contains the following code:
            """
            Collection<string> collection = new Collection<int>();
            """
        When the code is compiled
        Then an error is returned
        And the error contains "cannot be implicitly converted"
        
    Scenario: Constructors of generic classes can have parameters of the generic type (2)
        Given the following class is defined:
            """
            class Collection<T> {
                public T x;

                public Collection(T _x) {
                    x = _x;
                }
            }
            """
        And the main function contains the following code:
            """
            Collection<int> c = new Collection<int>(1337);
            TEST("c.x", c.x);
            """
        When the code is compiled
        Then there are no errors
        And "c.x" evaluates to 1337

    Scenario: Constructors of generic classes can have initializer parameters of the generic type
        Given the following class is defined:
            """
            class Collection<T> {
                public T x;

                public Collection(this.x) {}
            }
            """
        And the main function contains the following code:
            """
            Collection<int> c = new Collection<int>(1337);
            TEST("c.x", c.x);
            """
        When the code is compiled
        Then there are no errors
        And "c.x" evaluates to 1337
