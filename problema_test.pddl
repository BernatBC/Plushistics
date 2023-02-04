(define (problem plushistics)
    (:domain plushistics)

    (:objects
v1 - van
c1 c2 c3 -city
)

(:init
(
    (path ?c1 ?c2)
    (path ?c2 ?c1)
    (path ?c1 ?c3)
    (path ?c3 ?c1)
    (path ?c2 ?c3)
    (path ?c3 ?c2)
    (parked ?v1 ?c1)
    


)

(:goal
(and
(forall (?c - city) 
    (>=(sharks ?c) (demand ?c)))

)
)
(:metric maximize
(sum-petitions) )

)