(define (problem one)
    (:domain plushistics)

    (:objects
        V1 - van
        C1 C2 C3 C4 C5 C6 C7   - city
    )

    (:init
        (= (gas) 0)
        (= (capacity V1) 10)

        (= (cargo V1) 0)

        (path C1 C2)
        (path C2 C1)
        (path C3 C2)
        (path C2 C3)
        (path C3 C4)
        (path C4 C3)
        (path C3 C5)
        (path C5 C3)
        (path C6 C4)
        (path C4 C6)
        (path C6 C7)
        (path C7 C6)
        (path C1 C7)
        (path C7 C1)

        (parked V1 C2)
        (= (sharks C1) 2)
        (= (sharks C2) 6)
        (= (sharks C3) 5)   
        (= (sharks C4) 5)
        (= (sharks C5) 6)
        (= (sharks C6) 8)   
        (= (sharks C7) 3)

        (= (demand C1) 4)
        (= (demand C2) 2)
        (= (demand C3) 3)
        (= (demand C4) 9)
        (= (demand C5) 7)
        (= (demand C6) 3)
        (= (demand C7) 4)

        (= (cost C1 C2) 4)
        (= (cost C2 C1) 4)
        (= (cost C2 C3) 5)
        (= (cost C3 C2) 5)
        (= (cost C3 C4) 1)
        (= (cost C4 C3) 1)
        (= (cost C3 C5) 2)
        (= (cost C5 C3) 2)
        (= (cost C6 C4) 1)
        (= (cost C4 C6) 1)
        (= (cost C6 C7) 2)
        (= (cost C7 C6) 2)
        (= (cost C7 C1) 10)
        (= (cost C1 C7) 10)



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