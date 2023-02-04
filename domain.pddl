(define (domain plushistics)

    (:requirements :adl :fluents :equality)

    (:types
        van - object
        city - object
    )


    (:functions
        (capacity ?v - van)
        (cargo ?v - van)
        (sharks ?c - city)
        (demand ?c - city)
        (gas)
        (cost ?c1 ?c2 - city)
    )

    (:predicates
        (parked ?v - van ?c - city)
        (path ?c1 - city ?c2 - city)
        
    ) 

    (:action move
        :parameters (

            ?v - van
            ?from - city
            ?to - city
        )

        :precondition (and 
            (parked ?v ?from)
            (path ?from ?to)
        )
        :effect (and
            (parked ?v ?to)
            (increase (gas) (cost ?from ?to))
            (not (parked ?v ?from))
            
        )
    )

    (:action load           
        :parameters
        (
            ?v - van
            ?c - city
        )

        :precondition
        (and
            (parked ?v ?c)
            (> (sharks ?c) 0)
            (< (cargo ?v) (capacity ?v))
            
        )
        :effect
        (and
            (increase (cargo ?v) 1)
            (decrease (sharks ?c) 1)
            
        )
    )

    (:action unload         
        :parameters
        (
            ?v - van
            ?c - city
        )

        :precondition
        (and
            (parked ?v ?c)
            (> (cargo ?v) 0)
            
        )
        :effect
        (and
            (increase (sharks ?c) 1)
            (decrease (cargo ?v) 1)
        )
    )

)