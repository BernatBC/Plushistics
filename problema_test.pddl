(define (problem one)
    (:domain plushistics)
    
    (:objects
        V1 - van
        C1 C2 C3 C4  - city
    )

    (:init
        (= (gas) 0)
        (= (capacity V1) 10)

        (= (cargo V1) 0)

        (path C1 C3)
        (path C3 C1)
        (path C1 C2)
        (path C2 C1)
        (path C3 C4)
        (path C4 C3)
        (path C2 C4)
        (path C4 C2)
        (parked V1 C4)
        (= (sharks C1) 10)
        (= (sharks C2) 5)
        (= (sharks C3) 5)
        (= (sharks C4) 1)

        (= (demand C1) 3)
        (= (demand C2) 6)
        (= (demand C3) 2)
        (= (demand C4) 4)

        (= (cost C1 C3) 1)
        (= (cost C3 C1) 1)
        (= (cost C1 C2) 2)
        (= (cost C2 C1) 2)
        (= (cost C3 C4) 1)
        (= (cost C4 C3) 1)
        (= (cost C2 C4) 2)
        (= (cost C4 C2) 2)


    )

    (:goal
        (forall (?c - city)
            (>= (sharks ?c) (demand ?c))
        )
    )

    (:metric minimize
        (gas)
    )
)