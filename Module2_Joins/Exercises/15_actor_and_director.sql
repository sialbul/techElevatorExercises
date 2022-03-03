-- 15. The title of the movie and the name of director for movies where the director was also an actor in the same movie (73 rows)

SELECT title, person_name
FROM person
JOIN movie_actor ON person_id=actor_id
JOIN movie ON movie.movie_id= movie_actor.movie_id
WHERE person_id=director_id;