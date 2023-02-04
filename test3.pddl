(define (problem one)
    (:domain plushistics)
    
    (:objects
        V1 - van
        C1 C2 C3 C4 C5   - city
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
        (path C5 C4)
        (path C4 C5)
        (path C5 C1)
        (path C1 C5)


        (parked V1 C2)
        (= (sharks C1) 1)
        (= (sharks C2) 1)
        (= (sharks C3) 0)   
        (= (sharks C4) 2)
        (= (sharks C5) 1)


        (= (demand C1) 1)
        (= (demand C2) 1)
        (= (demand C3) 1)
        (= (demand C4) 1)
        (= (demand C5) 1)


        (= (cost C1 C2) 3)
        (= (cost C2 C1) 3)
        (= (cost C2 C3) 3)
        (= (cost C3 C2) 3)
        (= (cost C3 C4) 5)
        (= (cost C4 C3) 5)
        (= (cost C4 C5) 2)
        (= (cost C5 C4) 2)
        (= (cost C1 C5) 4)
        (= (cost C5 C1) 4)




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